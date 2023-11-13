using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SIS.Domain;
using SIS.Domain.Interfaces;

namespace SIS.Infrastructure
{
    public class TeacherLocationInterestImporterService : IImporter
    {
        private readonly ILogger<TeacherLocationInterestImporterService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISISTeacherLocationInterestRepository _repository;

        public TeacherLocationInterestImporterService(ILogger<TeacherLocationInterestImporterService> logger, IConfiguration configuration, ISISTeacherLocationInterestRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        public void Import()
        {
            string json = File.ReadAllText(Path.Combine(_configuration["JsonDataPath"], "TeacherLocationInterests.json"));
            var locationInterest = JsonConvert.DeserializeObject<List<TeacherLocationInterest>>(json, new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            });

            if (locationInterest != null)
            {
                foreach (var role in locationInterest)
                {
                    _repository.Insert(role);
                }
            }
        }
    }
}
