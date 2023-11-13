using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SIS.Domain.Interfaces;

internal partial class Program
{
    internal sealed class ConsoleHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHostApplicationLifetime _appLifetime;

        private readonly IImporter _teacherPreferenceImporterService;
        private readonly IImporter _coordinationRoleImporterService;
        private readonly IImporter _teacherCoordinationRoleInterestImporterService;
        private readonly IImporter _teacherLocationInterestImporterService;
        private readonly IImporter _periodImporterService;

        private readonly ISISTeacherPreferenceRepository _repository;
        private int? _exitCode;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IConfiguration configuration,
            IHostApplicationLifetime appLifetime,
            IImporter teacherPreferenceImporterService,
            IImporter coordinationRoleImporterService,
            IImporter teacherCoordinationRoleInterestImporterService,
            IImporter teacherLocationInterestImporterService,
            IImporter periodImporterService)
        {
            _logger = logger;
            _configuration = configuration;
            _appLifetime = appLifetime;
            _teacherPreferenceImporterService = teacherPreferenceImporterService;
            _coordinationRoleImporterService = coordinationRoleImporterService;
            _teacherCoordinationRoleInterestImporterService = teacherCoordinationRoleInterestImporterService;
            _teacherLocationInterestImporterService = teacherLocationInterestImporterService;
            _periodImporterService = periodImporterService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation("Importing...");

                        //_teacherPreferenceImporterService.Import(); // ok
                        _coordinationRoleImporterService.Import(); //  test!
                        _teacherCoordinationRoleInterestImporterService.Import(); // test!
                        _teacherLocationInterestImporterService.Import(); // test!
                        _periodImporterService.Import(); // test!

                        _exitCode = 0;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled exception!");
                        _exitCode = 1;
                    }
                    finally
                    {
                        // Stop the application once the work is done
                        _appLifetime.StopApplication();
                    }
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Exiting with return code: {_exitCode}");

            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
            return Task.CompletedTask;
        }
    }
}