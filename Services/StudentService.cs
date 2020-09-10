﻿namespace goOfflineE.Services
{
    using Aducati.Azure.TableStorage.Repository;
    using AutoMapper;
    using goOfflineE.Entites;
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="StudentService" />.
    /// </summary>
    public class StudentService : IStudentService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Defines the _mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Defines the _profileService.
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// Defines the _emailService.
        /// </summary>
        private readonly IEmailService _emailService;

        /// <summary>
        /// Defines the _azureBlobService.
        /// </summary>
        private readonly IAzureBlobService _azureBlobService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentService"/> class.
        /// </summary>
        /// <param name="emailService">The emailService<see cref="IEmailService"/>.</param>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        /// <param name="mapper">The mapper<see cref="IMapper"/>.</param>
        /// <param name="profileService">The profileService<see cref="IProfileService"/>.</param>
        /// <param name="azureBlobService">The azureBlobService<see cref="IAzureBlobService"/>.</param>
        public StudentService(IEmailService emailService, ITableStorage tableStorage, IMapper mapper, IProfileService profileService, IAzureBlobService azureBlobService)
        {
            _tableStorage = tableStorage;
            _mapper = mapper;
            _profileService = profileService;
            _emailService = emailService;
            _azureBlobService = azureBlobService;
        }

        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="model">The model<see cref="StudentRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateUpdate(StudentRequest model)
        {
            TableQuery<Entites.Student> query = new TableQuery<Entites.Student>()
                 .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, model.Id));
            var studentQuery = await _tableStorage.QueryAsync<Entites.Student>("Student", query);
            var student = studentQuery.SingleOrDefault();

            if (student != null)
            {
                // Update profile
                ProfileUpdateRequest profileUpdateRequest = new ProfileUpdateRequest
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Id = model.Id
                };

                await _profileService.UpdateProfile(profileUpdateRequest);

                //Update student information
                student.ClassId = model.ClassId;
                student.Address1 = model.Address1;
                student.Address2 = model.Address2;
                student.Country = model.Country;
                student.State = model.State;
                student.City = model.City;
                student.Zip = model.Zip;
                student.Latitude = model.Latitude;
                student.Longitude = model.Longitude;
                student.Active = true;
                student.CreatedBy = model.CreatedBy;
                student.UpdatedOn = DateTime.UtcNow;
                student.UpdatedBy = model.CreatedBy;

                try
                {
                    await _tableStorage.UpdateAsync("Student", student);
                }
                catch (Exception ex)
                {
                    throw new AppException("update student error: ", ex.InnerException);
                }
            }
            else
            {
                // Register user as student
                var userId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;
                var defaultPasswrod = "test@123"; //SettingConfigurations.GetRandomPassword(10);
                RegisterRequest registerRequest = new RegisterRequest
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = Enum.GetName(typeof(Role), Role.Student),
                    AcceptTerms = true,
                    Id = userId,
                    SchoolId = model.SchoolId,
                    Password = defaultPasswrod,
                    ConfirmPassword = defaultPasswrod
                };

                var newStudent = new Entites.Student(model.SchoolId, userId)
                {
                    ClassId = model.ClassId,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Country = model.Country,
                    State = model.State,
                    City = model.City,
                    Zip = model.Zip,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Active = true,
                    CreatedBy = model.CreatedBy,
                    UpdatedOn = DateTime.UtcNow,
                    UpdatedBy = model.CreatedBy,
                };
                try
                {
                    await _profileService.Register(registerRequest);
                    await _tableStorage.AddAsync("Student", newStudent);
                }
                catch (Exception ex)
                {

                    throw new AppException("Create student error: ", ex.InnerException);
                }
            }
        }

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{StudentResponse}}"/>.</returns>
        public async Task<IEnumerable<StudentResponse>> GetAll(string schoolId, string classId)
        {
            TableQuery<Entites.User> userQuery = new TableQuery<Entites.User>()
                  .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, schoolId));
            var users = await _tableStorage.QueryAsync<Entites.User>("User", userQuery);

            var filterString = TableQuery.CombineFilters(
                                TableQuery.GenerateFilterCondition("PartitionKey",
                                    QueryComparisons.Equal,
                                    schoolId),
                                TableOperators.And,
                                TableQuery.GenerateFilterCondition("ClassId",
                                    QueryComparisons.Equal,
                                    classId)
                                );

            TableQuery<Entites.Student> studentQuery = new TableQuery<Entites.Student>().Where(filterString);
            var students = await _tableStorage.QueryAsync<Entites.Student>("Student", studentQuery);

            var studentList = from user in users
                              join student in students
                                   on user.RowKey equals student.RowKey
                              orderby student.UpdatedOn descending
                              select new StudentResponse
                              {
                                  Id = user.RowKey,
                                  FirstName = user.FirstName,
                                  LastName = user.LastName,
                                  Email = user.Email,
                                  Address1 = student.Address1,
                                  Address2 = student.Address2,
                                  Country = student.Country,
                                  State = student.State,
                                  City = student.City,
                                  Zip = student.Zip,
                                  SchoolId = student.PartitionKey,
                                  ClassId = student.ClassId,
                                  ProfileStoragePath = student.ProfileStoragePath,
                                  TrainStudentModel = student.TrainStudentModel
                              };

            return studentList;
        }

        /// <summary>
        /// The UpdateStatusToTrainStudentModel.
        /// </summary>
        /// <param name="studentId">The studentId<see cref="string"/>.</param>
        /// <param name="studentPhotos">The studentPhotos<see cref="List{string}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task UpdateStudentProfile(string studentId, List<string> studentPhotos)
        {
            TableQuery<Entites.Student> query = new TableQuery<Entites.Student>()
                 .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, studentId));
            var studentQuery = await _tableStorage.QueryAsync<Entites.Student>("Student", query);
            var student = studentQuery.SingleOrDefault();
            student.TrainStudentModel = true;

            var existingPhotURLs = JsonConvert.DeserializeObject<StudentPhotos>(student.ProfileStoragePath);

            if (existingPhotURLs != null)
            {
                studentPhotos = studentPhotos.Union(existingPhotURLs.Photos).ToList();
            }

            var studentPhotoBlobURLs = JsonConvert.SerializeObject(new StudentPhotos { Photos = studentPhotos });

            student.ProfileStoragePath = studentPhotoBlobURLs;

            try
            {
                await _tableStorage.UpdateAsync("Student", student);
            }
            catch (Exception ex)
            {
                throw new AppException("UpdateStatusToTrainStudentModel error: ", ex.InnerException);
            }
        }
    }
}
