﻿using Aducati.Azure.TableStorage.Repository;
using AutoMapper;
using goOfflineE.Helpers;
using goOfflineE.Models;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ITableStorage _tableStorage;
        private readonly IMapper _mapper;
        private readonly IClassService _classService;
        private readonly ITeacherService _teacherService;

        public SchoolService(ITableStorage tableStorage, IMapper mapper, IClassService classService, ITeacherService teacherService)
        {
            _tableStorage = tableStorage;
            _mapper = mapper;
            _classService = classService;
            _teacherService = teacherService;
        }
        public async Task CreateUpdate(SchoolRequest model)
        {
            // validate
            TableQuery<Entites.School> query = new TableQuery<Entites.School>()
                   .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, model.Id));
            var schoolQuery = await _tableStorage.QueryAsync<Entites.School>("School", query);
            var school = schoolQuery.SingleOrDefault();

            if (school != null)
            {
                school.Name = model.Name;
                school.Address1 = model.Address1;
                school.Address2 = model.Address2;
                school.Country = model.Country;
                school.State = model.State;
                school.City = model.City;
                school.Zip = model.Zip;
                school.Latitude = model.Latitude;
                school.Longitude = model.Longitude;
                school.Active = true;
                school.CreatedBy = model.CreatedBy;
                school.UpdatedOn = DateTime.UtcNow;
                school.UpdatedBy = model.CreatedBy;

                try
                {
                    await _tableStorage.UpdateAsync("School", school);
                }
                catch (Exception ex)
                {
                    throw new AppException("update school error: ", ex.InnerException);
                }
            }
            else
            {
                var schoolId = String.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id;
                var newSchool = new Entites.School(schoolId, schoolId)
                {
                    Name = model.Name,
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
                    await _tableStorage.AddAsync("School", newSchool);
                }
                catch (Exception ex)
                {
                    throw new AppException("Create school error: ", ex.InnerException);
                }
            }


        }

        public async Task<IEnumerable<Models.School>> GetAll(string schoolId = "")
        {
            var allSchools = await _tableStorage.GetAllAsync<Entites.School>("School");
            var schools = string.IsNullOrEmpty(schoolId) ? allSchools : allSchools.Where(school => school.RowKey == schoolId);
            var schoolData = this._mapper.Map<IEnumerable<Models.School>>(schools);
            foreach (var school in schoolData)
            {
                school.ClassRooms = await _classService.GetAll(school.Id);
                school.Teachers = await _teacherService.GetAll(school.Id);
            }
            return schoolData;
        }

    }
}
