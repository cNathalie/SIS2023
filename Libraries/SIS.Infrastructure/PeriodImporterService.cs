using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SIS.Domain;
using SIS.Domain.Interfaces;

namespace SIS.Infrastructure
{
    public class PeriodImporterService : IImporter
    {
        private readonly ILogger<PeriodImporterService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISISPeriodRepository _repository;

        public PeriodImporterService(ILogger<PeriodImporterService> logger, IConfiguration configuration, ISISPeriodRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        public void Import()
        {
            string json = File.ReadAllText(Path.Combine(_configuration["JsonDataPath"], "Periods.json"));
            var periods = JsonConvert.DeserializeObject<List<Period>>(json);
            if (periods != null)
            {
                foreach (var p in periods)
                {
                    _repository.Insert(p);
                }
            }
        }
    }
}
