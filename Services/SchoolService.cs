namespace goOfflineE.Services
{
    using Aducati.Azure.TableStorage.Repository;
    using AutoMapper;
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="SchoolService" />.
    /// </summary>
    public class SchoolService : ISchoolService
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
        /// Defines the _classService.
        /// </summary>
        private readonly IClassService _classService;

        /// <summary>
        /// Defines the _teacherService.
        /// </summary>
        private readonly ITeacherService _teacherService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        /// <param name="mapper">The mapper<see cref="IMapper"/>.</param>
        /// <param name="classService">The classService<see cref="IClassService"/>.</param>
        /// <param name="teacherService">The teacherService<see cref="ITeacherService"/>.</param>
        public SchoolService(ITableStorage tableStorage, IMapper mapper, IClassService classService, ITeacherService teacherService)
        {
            _tableStorage = tableStorage;
            _mapper = mapper;
            _classService = classService;
            _teacherService = teacherService;
        }

        /// <summary>
        /// The CreateUpdate.
        /// </summary>
        /// <param name="model">The model<see cref="SchoolRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CreateUpdate(SchoolRequest model)
        {
            // validate
            var school = await this.GetSchool(model.Id);

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

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task Delete(string schoolId)
        {
            var school = await this.GetSchool(schoolId);
            school.Active = false;

            try
            {
                await _tableStorage.UpdateAsync("School", school);
            }
            catch (Exception ex)
            {
                throw new AppException("update school error: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IEnumerable{Models.School}}"/>.</returns>
        public async Task<IEnumerable<Models.School>> GetAll(string schoolId = "")
        {
            var allSchools = await _tableStorage.GetAllAsync<Entites.School>("School");
            var schools = string.IsNullOrEmpty(schoolId) ?
                allSchools.Where(school => school.Active.GetValueOrDefault(false)) :
                allSchools.Where(school => school.Active.GetValueOrDefault(false) && school.RowKey == schoolId);
            var schoolData = this._mapper.Map<IEnumerable<Models.School>>(schools);
            foreach (var school in schoolData)
            {
                school.ClassRooms = await _classService.GetAll(school.Id);
                school.Teachers = await _teacherService.GetAll(school.Id);
            }
            return schoolData;
        }

        /// <summary>
        /// The GetSchool.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{Entites.School}"/>.</returns>
        private async Task<Entites.School> GetSchool(string schoolId)
        {
            TableQuery<Entites.School> query = new TableQuery<Entites.School>()
                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, schoolId));
            var schoolQuery = await _tableStorage.QueryAsync<Entites.School>("School", query);
            return schoolQuery.SingleOrDefault();
        }
    }
}
