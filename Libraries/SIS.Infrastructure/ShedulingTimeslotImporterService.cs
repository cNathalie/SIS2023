using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SIS.Domain;
using SIS.Domain.Interfaces;

namespace SIS.Infrastructure
{
    public class ShedulingTimeslotImporterService : IImporter
    {
        private readonly ILogger<ShedulingTimeslotImporterService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISISShedulingTimeslotRepository _repository;

        public ShedulingTimeslotImporterService(ILogger<ShedulingTimeslotImporterService> logger, IConfiguration configuration, ISISShedulingTimeslotRepository repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        public void Import()
        {
            string json = File.ReadAllText(Path.Combine(_configuration["JsonDataPath"], "ShedulingTimeslot.json"));
            var timeslots = JsonConvert.DeserializeObject<List<ShedulingTimeslot>>(json, new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            });

            if (timeslots != null)
            {
                foreach (var ts in timeslots)
                {
                    _repository.Insert(ts);
                }
            };
        }
    }
}
