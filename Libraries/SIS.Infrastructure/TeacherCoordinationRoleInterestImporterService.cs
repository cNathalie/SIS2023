using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SIS.Domain;
using SIS.Domain.Interfaces;

namespace SIS.Infrastructure
{
    public class TeacherCoordinationRoleInterestImporterService : IImporter
    {
        private readonly ILogger<TeacherCoordinationRoleInterestImporterService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISISTeacherCoordinationRoleInterestRepository _repository;

        public TeacherCoordinationRoleInterestImporterService(ILogger<TeacherCoordinationRoleInterestImporterService> logger, IConfiguration configuration, ISISTeacherCoordinationRoleInterestRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        public void Import()
        {
            string json = File.ReadAllText(Path.Combine(_configuration["JsonDataPath"], "TeacherCoordinationRoleInterests.json"));
            var coordinationRoleInterests = JsonConvert.DeserializeObject<List<TeacherCoordinationRoleInterest>>(json);
            if (coordinationRoleInterests != null)
            {
                foreach (var role in coordinationRoleInterests)
                {
                    _repository.Insert(role);
                }
            }
        }
    }
}
