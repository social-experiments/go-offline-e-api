﻿namespace goOfflineE.Services
{
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using goOfflineE.Repository;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Defines the <see cref="ClassService" />.
    /// </summary>
    public class ClassService : IClassService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Defines the _studentService.
        /// </summary>
        private readonly IStudentService _studentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        /// <param name="studentService">The studentService<see cref="IStudentService"/>.</param>
        public ClassService(ITableStorage tableStorage, IStudentService studentService)
        {
            _tableStorage = tableStorage;
            _studentService = studentService;
        }

        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="model">The model<see cref="ClassRoom"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateUpdate(ClassRoom model)
        {
            var classRooms = await _tableStorage.GetAllAsync<Entites.ClassRoom>("ClassRoom");
            var classRoom = classRooms.SingleOrDefault(user => user.RowKey == model.ClassId);

            // Validate class room availabilty
            if (!String.IsNullOrEmpty(model.ClassRoomName))
            {
                var isClassAvailable = classRooms.Any(
                    cls => cls.PartitionKey == model.SchoolId &&
                cls.ClassRoomName.ToLower() == model.ClassRoomName.ToLower() &&
                cls.ClassDivision.ToLower() == model.ClassDivision.ToLower()
                );
                if (isClassAvailable)
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict)
                    {
                        Content = new StringContent(string.Format($"Class is not available in ${ model.SchoolId } school.")),

                    };
                    throw new HttpResponseException(resp);
                }

            }

            if (classRoom != null)
            {

                //Update class information
                classRoom.ClassDivision = model.ClassDivision;
                classRoom.ClassRoomName = model.ClassRoomName;
                classRoom.Active = true;
                classRoom.UpdatedOn = DateTime.UtcNow;
                classRoom.UpdatedBy = model.CreatedBy;

                try
                {
                    await _tableStorage.UpdateAsync("ClassRoom", classRoom);
                }
                catch (Exception ex)
                {
                    throw new AppException("update class room error: ", ex.InnerException);
                }
            }
            else
            {

                // Register new class
                var classRoomId = String.IsNullOrEmpty(model.ClassId) ? Guid.NewGuid().ToString() : model.ClassId;

                var newClassRoom = new Entites.ClassRoom(model.SchoolId, classRoomId)
                {
                    ClassDivision = model.ClassDivision,
                    ClassRoomName = model.ClassRoomName,
                    Active = true,
                    CreatedBy = model.CreatedBy,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = model.CreatedBy,
                };
                try
                {
                    await _tableStorage.AddAsync("ClassRoom", newClassRoom);
                }
                catch (Exception ex)
                {

                    throw new AppException("Create class room error: ", ex.InnerException);
                }
            }
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Delete(string classId)
        {
            TableQuery<Entites.ClassRoom> classQuery = new TableQuery<Entites.ClassRoom>()
                  .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, classId));
            var classRooms = await _tableStorage.QueryAsync<Entites.ClassRoom>("ClassRoom", classQuery);

            var classRoom = classRooms.FirstOrDefault();

            classRoom.Active = false;

            try
            {
                await _tableStorage.UpdateAsync("ClassRoom", classRoom);
            }
            catch (Exception ex)
            {
                throw new AppException("update class Room error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The Get.
        /// </summary>
        /// <param name="classRoomId">The classRoomId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{ClassRoom}"/>.</returns>
        public async Task<ClassRoom> Get(string classRoomId)
        {
            TableQuery<Entites.ClassRoom> classQuery = new TableQuery<Entites.ClassRoom>()
                 .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, classRoomId));
            var classRoomQuery = await _tableStorage.QueryAsync<Entites.ClassRoom>("ClassRoom", classQuery);
            var classRoom = classRoomQuery.FirstOrDefault();

            return new ClassRoom
            {
                ClassId = classRoom.RowKey,
                SchoolId = classRoom.PartitionKey,
                ClassRoomName = classRoom.ClassRoomName,
                ClassDivision = classRoom.ClassDivision
            };
        }

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{ClassRoom}}"/>.</returns>
        public async Task<IEnumerable<ClassRoom>> GetAll(string schoolId = "")
        {

            TableQuery<Entites.ClassRoom> classQuery = new TableQuery<Entites.ClassRoom>()
                  .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, schoolId));
            var classRoomQuery = await _tableStorage.QueryAsync<Entites.ClassRoom>("ClassRoom", classQuery);
            var classList = from classRoom in classRoomQuery
                            where classRoom.Active.GetValueOrDefault(false)
                            orderby classRoom.UpdatedOn descending
                            select classRoom;
            var classRoomList = new List<ClassRoom>();
            foreach (var classRoom in classList)
            {
                var students = await _studentService.GetAll(classRoom.PartitionKey, classRoom.RowKey);
                classRoomList.Add(new ClassRoom
                {
                    ClassId = classRoom.RowKey,
                    SchoolId = classRoom.PartitionKey,
                    ClassRoomName = classRoom.ClassRoomName,
                    ClassDivision = classRoom.ClassDivision,
                    Students = students.ToList()
                });
            }

            return classRoomList;
        }
    }
}
