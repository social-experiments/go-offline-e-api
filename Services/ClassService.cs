using Aducati.Azure.TableStorage.Repository;
using AutoMapper;
using goOfflineE.Helpers;
using goOfflineE.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goOfflineE.Azure.Function.Api.Services
{
    public class ClassService : IClassService
    {
        private readonly ITableStorage _tableStorage;
        private readonly IMapper _mapper;

        public ClassService(ITableStorage tableStorage, IMapper mapper)
        {
            _tableStorage = tableStorage;
            _mapper = mapper;
        }

        public async Task CreateUpdate(ClassRoom model)
        {
            TableQuery<Entites.ClassRoom> query = new TableQuery<Entites.ClassRoom>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, model.ClassId));
            var classQuery = await _tableStorage.QueryAsync<Entites.ClassRoom>("ClassRoom", query);
            var classRoom = classQuery.SingleOrDefault();

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

        public async Task<IEnumerable<ClassRoom>> GetAll(string schoolId = "")
        {

            TableQuery<Entites.ClassRoom> classQuery = new TableQuery<Entites.ClassRoom>()
                  .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, schoolId));
            var classRooms = await _tableStorage.QueryAsync<Entites.ClassRoom>("ClassRoom", classQuery);
            var classList = from classRoom in classRooms
                            orderby classRoom.UpdatedOn descending
                            select new ClassRoom
                            {
                                ClassId = classRoom.RowKey,
                                SchoolId = classRoom.PartitionKey,
                                ClassDivision = classRoom.ClassDivision,
                                ClassRoomName = classRoom.ClassRoomName,
                            };

            return classList;
        }
    }
}
