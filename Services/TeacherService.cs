using Aducati.Azure.TableStorage.Repository;
using AutoMapper;
using Educati.Azure.Function.Api.Helpers;
using Educati.Azure.Function.Api.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educati.Azure.Function.Api.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITableStorage _tableStorage;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IProfileService _profileService;

        public TeacherService(ITableStorage tableStorage, IMapper mapper, IAccountService accountService, IProfileService profileService)
        {
            _tableStorage = tableStorage;
            _mapper = mapper;
            _accountService = accountService;
            _profileService = profileService;
        }
        public async Task CreateUpdate(TeacherRequest model)
        {
           
            TableQuery<Entites.Teacher> query = new TableQuery<Entites.Teacher>()
                  .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, model.Id));
            var teacherQuery = await _tableStorage.QueryAsync<Entites.Teacher>("Teacher", query);
            var teacher = teacherQuery.SingleOrDefault();

            if (teacher != null)
            {
                // Update profile
                ProfileUpdateRequest profileUpdateRequest = new ProfileUpdateRequest
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                await _profileService.UpdateProfile(profileUpdateRequest);

                //Update teacher information
                teacher.Address1 = model.Address1;
                teacher.Address2 = model.Address2;
                teacher.Country = model.Country;
                teacher.State = model.State;
                teacher.City = model.City;
                teacher.Zip = model.Zip;
                teacher.Latitude = model.Latitude;
                teacher.Longitude = model.Longitude;
                teacher.Active = true;
                teacher.CreatedBy = model.CreatedBy;
                teacher.UpdatedOn = DateTime.UtcNow;
                teacher.UpdatedBy = model.CreatedBy;

                try
                {
                    await _tableStorage.UpdateAsync("Teacher", teacher);
                }
                catch (Exception ex)
                {
                    throw new AppException("update teacher error: ", ex.InnerException);
                }
            }
            else
            {
                // Register user as teacher
                var userId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;
                RegisterRequest registerRequest = new RegisterRequest
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role,
                    AcceptTerms = true,
                    Id = userId,
                    SchoolId = model.SchoolId
                };
                await _accountService.Register(registerRequest);

                // Create new teacher 
                
                var newSchool = new Entites.Teacher(model.SchoolId, userId)
                {
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
                    await _tableStorage.AddAsync("Teacher", newSchool);
                }
                catch (Exception ex)
                {
                    throw new AppException("Create teacher error: ", ex.InnerException);
                }
            }

        }

        public async Task<IEnumerable<TeacherResponse>> GetAll(string schoolId)
        {
            TableQuery<Entites.User> userQuery = new TableQuery<Entites.User>()
                  .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, schoolId));
            var users = await _tableStorage.QueryAsync<Entites.User>("User", userQuery);

            TableQuery<Entites.Teacher> teacherQuery = new TableQuery<Entites.Teacher>()
                  .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, schoolId));
            var teachers = await _tableStorage.QueryAsync<Entites.Teacher>("Teacher", teacherQuery);
            var teachersList= from user in users
                        join teacher in teachers
                             on user.RowKey equals teacher.RowKey
                             orderby teacher.UpdatedOn descending
                        select new TeacherResponse
                        {
                            Id = user.RowKey,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Address1 = teacher.Address1,
                            Address2 = teacher.Address2,
                            Country = teacher.Country,
                            State = teacher.State,
                            City = teacher.City,
                            Zip = teacher.Zip,
                            SchoolId = teacher.PartitionKey
                        };

            return teachersList;
        }
    }
}
