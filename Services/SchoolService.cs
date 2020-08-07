using Aducati.Azure.TableStorage.Repository;
using AutoMapper;
using Educati.Azure.Function.Api.Entites;
using Educati.Azure.Function.Api.Helpers;
using Educati.Azure.Function.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Educati.Azure.Function.Api.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ITableStorage _tableStorage;
        private readonly IMapper _mapper;
        public SchoolService(ITableStorage tableStorage, IMapper mapper)
        {
            _tableStorage = tableStorage;
            _mapper = mapper;
        }
        public async Task Create(SchoolRequest model)
        {
            // validate
            var schools = await _tableStorage.GetAllAsync<Entites.School>("School");

            if (schools.Any(x => x.Name == model.Name))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.AlreadyReported)
                {
                    Content = new StringContent(string.Format($"School { model.Name} already created!"))
                };
                throw new HttpResponseException(resp);
            }
            var schoolId = Guid.NewGuid().ToString();
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

        public async Task<IEnumerable<Models.School>> GetAll()
        {
            var schools = await _tableStorage.GetAllAsync<Entites.School>("School");

            return this._mapper.Map<IEnumerable<Models.School>>(schools);
        }
    }
}
