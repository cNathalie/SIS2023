
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using SIS.API;
using SIS.API.Extensions;
using SIS.Domain.Interfaces;
using SIS.Infrastructure;
using SIS.Infrastructure.EFRepository.Context;
using SISApi.Extensions;

//--Nathalie:
//------------
//-OK- 1 CoordinationRole 
//-OK- 2 TeacherPreference 
//-OK- 3 TeacherCoordinationRoleInterest 
//-k- 4 TeacherCourseInterest 
//-K- 5 TeacherInterest 
//-OK- 6 TeacherLocationInterest 
//-OK- 7 Period 
//-OK- 8 SchedulingTimeslot 

namespace SISApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddLogging(loggingBuilder =>
            {
                var configuration = new ConfigurationBuilder()
                                                    .AddJsonFile("appsettings.json")
                                                    .Build();
                var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
                loggingBuilder.AddSerilog(logger, dispose: true);
            });

            builder.Services.AddDbContext<SisDbContext>()
                  .AddScoped<ISISTeacherTypeRepository, EFSISTeacherTypeRepository>() // here I could pick the ADO.NET alternative
                  .AddScoped<ISISRegistrationStateRepository, EFSISRegistrationStateRepository>() // here I could pick the ADO.NET alternative
                  .AddScoped<ISISPersonRepository, EFSISPersonRepository>() // here I could pick the ADO.NET alternative
                  .AddScoped<ISISTeacherRepository, EFSISTeacherRepository>() // here I could pick the ADO.NET alternative

                  .AddScoped<ISISTeacherPreferenceRepository, EFSISTeacherPreferenceRepository>() // BertEnErnie Nathalie
                  .AddScoped<ISISCoordinationRoleRepository, EFSISCoordinationRoleRepository>() // BertEnErnie Nathalie
                  .AddScoped<ISISPeriodRepository, EFSISPeriodRepository>() // BertEnErnie Nathalie
                  .AddScoped<ISISTeacherCoordinationRoleInterestRepository, EFSISTeacherCoordinationRoleInterestRepository>() // BertEnErnie Nathalie
                  .AddScoped<ISISTeacherLocationInterestRepository, EFSISTeacherLocationInterestRepository>() // BertEnErnie Nathalie
                  .AddScoped<ISISShedulingTimeslotRepository, EFSISShedulingTimeslotRepository>() //BertEnErnie Nathalie
                  .AddScoped<ISISTeacherInterestRepository, EFSISTeacherInterestRepository>() // BertEnErnie Nathalie
                  .AddScoped<ISISTeacherCourseInterestRepository, EFSISTeacherCourseInterestRepository>() // BertEnErnie Nathalie

                  .AddScoped<ISISRoomRepository, EFSISRoomRepository>() // Da engineering
                  .AddScoped<ISISRoomTypeRepository, EFSISRoomTypeRepository>() // Da engineering
                  .AddScoped<ISISRoomKindRepository, EFSISRoomKindRepository>() // Da engineering
                  .AddScoped<ISISBuildingRepository, EFSISBuildingRepository>() // Da engineering
                  .AddScoped<ISISLocationRepository, EFSISLocationRepository>() // Da engineering
                  .AddScoped<ISISCampusRepository, EFSISCampusRepository>(); // Da engineering

            builder.Services.AddAutoMapper(typeof(MappingConfig));

            builder.Services.AddControllers();

            // Added so swagger handles TimeOnly and DateOnly (Nathalie)
            builder.Services.AddDateOnlyTimeOnlyStringConverters();

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            // Configure the API versioning properties of the project. 
            builder.Services.AddApiVersioningConfigured();

            // Add a Swagger generator and Automatic Request and Response annotations:
            builder.Services.AddSwaggerSwashbuckleConfigured();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Specific middleware to check if HTTP status codes are specified
            app.UseSwaggerResponseCheck();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}