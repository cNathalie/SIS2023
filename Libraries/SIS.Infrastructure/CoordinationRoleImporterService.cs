using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SIS.Domain;
using SIS.Domain.Interfaces;

namespace SIS.Infrastructure
{
    public class CoordinationRoleImporterService : IImporter
    {
        private readonly ILogger<CoordinationRoleImporterService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISISCoordinationRoleRepository _repository;

        public CoordinationRoleImporterService(ILogger<CoordinationRoleImporterService> logger, IConfiguration configuration, ISISCoordinationRoleRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        public void Import()
        {
            string json = File.ReadAllText(Path.Combine(_configuration["JsonDataPath"], "CoordinationRoles.json"));
            var coordinationRole = JsonConvert.DeserializeObject<List<CoordinationRole>>(json);
            if (coordinationRole != null)
            {
                foreach (var role in coordinationRole)
                {
                    _repository.Insert(role);
                }
            }
        }

    }
}
