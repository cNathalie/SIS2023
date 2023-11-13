using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIS.Domain;
using SIS.Domain.Interfaces;
using SIS.Infrastructure.EFRepository.Context;

namespace SIS.Infrastructure
{
    public class EFSISTeacherCoordinationRoleInterestRepository : ISISTeacherCoordinationRoleInterestRepository
    {
        private readonly ILogger<EFSISTeacherCoordinationRoleInterestRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly SisDbContext _context;

        // private readonly ISISAcademicYearRepository _academicYearRepository;
        private readonly ISISTeacherRepository _teacherRepository;
        private readonly ISISTeacherPreferenceRepository _preferenceRepository;
        private readonly ISISCoordinationRoleRepository _coordinationRoleRepository;

        private static Dictionary<int, TeacherCoordinationRoleInterest> _teacherCoordinationRoleInterests = new();

        public Dictionary<int, TeacherCoordinationRoleInterest> TeacherCoordinationRoleInterests
        {
            get
            {
                if (_teacherCoordinationRoleInterests != null) return _teacherCoordinationRoleInterests;
                return RefreshTeacherCoordinationRoleInterests();
            }
        }

        public EFSISTeacherCoordinationRoleInterestRepository(ILogger<EFSISTeacherCoordinationRoleInterestRepository> logger,
            IConfiguration configuration, SisDbContext context, ISISTeacherRepository teacherRepository, ISISTeacherPreferenceRepository preferenceRepository,
            ISISCoordinationRoleRepository coordinationRoleRepository /* , ISISAcademicYearRepository academicYearRepository */)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;

            _teacherRepository = teacherRepository;
            _preferenceRepository = preferenceRepository;
            _coordinationRoleRepository = coordinationRoleRepository;
            // TODO: _academicYearRepository = academicYearRepository;

            RefreshTeacherCoordinationRoleInterests();
        }

        public void Delete(TeacherCoordinationRoleInterest teacherCoordinationRoleInterest)
        {
            if (!Exists(teacherCoordinationRoleInterest)) return;

            var efEntity = GetEFEntity(teacherCoordinationRoleInterest);

            try
            {
                var efRemove = _context.Remove(efEntity).Entity;
                var count = _context.SaveChanges();

                RefreshTeacherCoordinationRoleInterests();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }

        public bool Exists(TeacherCoordinationRoleInterest teacherCoordinationRoleInterest)
        {
            var efTeacherCoordinationRoleInterest = GetEFEntity(teacherCoordinationRoleInterest);
            return efTeacherCoordinationRoleInterest != null;
        }

        public int Insert(TeacherCoordinationRoleInterest newRoleInterest)
        {

            //check if roleInterest already exists for this AcademicYear, this Teacher, this Role
            var checkDictionary = _teacherCoordinationRoleInterests.Values
                                    .Where(tli => tli.AcademicYearStart == newRoleInterest.AcademicYearStart && tli.AcademicYearStop == newRoleInterest.AcademicYearStop
                                    && tli.TeacherLastName == newRoleInterest.TeacherLastName && tli.TeacherFirstName == newRoleInterest.TeacherFirstName
                                    && tli.CoordinationRole == newRoleInterest.CoordinationRole)
                                    .FirstOrDefault();

            if (checkDictionary != null) 
            { 
                return checkDictionary.TeacherCoordinationRoleInterestId;
            }

            //else try insert
            try
            {
                var efAcademicYear = _context.AcademicYears
                                            .Where(ay => ay.StartDate.Date == newRoleInterest.AcademicYearStart.Date
                                                      && ay.StopDate.Date == newRoleInterest.AcademicYearStop.Date)
                                                      .FirstOrDefault();


                var efTeacher = _context.Teachers
                                      .Include(t => t.Person)
                                      .Where(t => t.Person.FirstName == newRoleInterest.TeacherFirstName && t.Person.LastName == newRoleInterest.TeacherLastName)
                                      .FirstOrDefault();

                var efTeacherPreference = _context.TeacherPreferences
                                                .Where(tp => tp.Description == newRoleInterest.TeacherPreference)
                                                .FirstOrDefault();


                var efCoordinationRole = _context.CoordinationRoles
                                                .Where(r => r.Name == newRoleInterest.CoordinationRole)
                                                .FirstOrDefault();


                EFRepository.Models.TeacherCoordinationRoleInterest newEfRoleInterest = new()
                {
                    //id generated by db on SaveChanges
                    AcademicYearId = efAcademicYear.AcademicYearId,
                    TeacherId = efTeacher.TeacherId,
                    TeacherPreferenceId = efTeacherPreference.TeacherPreferenceId,
                    CoordinationRoleId = efCoordinationRole.CoordinationRoleId,
                };

                var efTeacherCoordinationRoleInterest = _context.TeacherCoordinationRoleInterests.Add(newEfRoleInterest).Entity;
                var count = _context.SaveChanges();
                _logger.LogInformation($"SaveChanges effected {count} row(s)");
                RefreshTeacherCoordinationRoleInterests();
                return efTeacherCoordinationRoleInterest.TeacherCoordinationRoleInterestId; //Gives back the newly generated id of the entity
            }
            catch (Exception ex)
            {
                // if inserting in db throws an exception, we log it and re-throw to client
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public Dictionary<int, TeacherCoordinationRoleInterest> RefreshTeacherCoordinationRoleInterests()
        {
            _teacherCoordinationRoleInterests.Clear();
            var dbTeacherCoordinationRoleInterests = _context.TeacherCoordinationRoleInterests
                                                                .Include(roleInterest => roleInterest.AcademicYear)
                                                                .Include(roleInterest => roleInterest.Teacher)
                                                                .Include(roleInterest => roleInterest.TeacherPreference)
                                                                .Include(roleInterest => roleInterest.CoordinationRole)
                                                                .ToList();
            foreach (var roleInterest in dbTeacherCoordinationRoleInterests)
            {

                var newRoleInterest = new TeacherCoordinationRoleInterest()
                {
                    TeacherCoordinationRoleInterestId = roleInterest.TeacherCoordinationRoleInterestId,
                    AcademicYearStart = roleInterest.AcademicYear.StartDate,
                    AcademicYearStop = roleInterest.AcademicYear.StopDate,
                    TeacherFirstName = roleInterest.Teacher.Person.FirstName,
                    TeacherLastName = roleInterest.Teacher.Person.LastName,
                    TeacherAbbreviation = roleInterest.Teacher.Abbreviation,
                    TeacherPreference = roleInterest.TeacherPreference.Description,
                    CoordinationRole = roleInterest.CoordinationRole.Name
                };


                _teacherCoordinationRoleInterests.Add(newRoleInterest.TeacherCoordinationRoleInterestId, newRoleInterest);
            }

            return _teacherCoordinationRoleInterests;

        }

        public void Update(TeacherCoordinationRoleInterest teacherCoordinationRoleInterestToUpdate, TeacherCoordinationRoleInterest newRoleInterest)
        {
            if (!Exists(teacherCoordinationRoleInterestToUpdate)) return;

            var efToUpdate = GetEFEntity(teacherCoordinationRoleInterestToUpdate);

            try
            {
                var newEfAcademicYear = _context.AcademicYears
                                            .Where(ay => ay.StartDate == newRoleInterest.AcademicYearStart
                                                      && ay.StopDate == newRoleInterest.AcademicYearStop)
                                                      .FirstOrDefault();
                var efTeacher = _context.Teachers
                                      .Include(t => t.Person)
                                      .Where(t => t.Person.FirstName == newRoleInterest.TeacherFirstName && t.Person.LastName == newRoleInterest.TeacherLastName)
                                      .FirstOrDefault();

                var efTeacherPreference = _context.TeacherPreferences
                                                .Where(tp => tp.Description == newRoleInterest.TeacherPreference)
                                                .FirstOrDefault();


                var efCoordinationRole = _context.CoordinationRoles
                                                .Where(r => r.Name == newRoleInterest.CoordinationRole)
                                                .FirstOrDefault();


                //id is not updated
                efToUpdate.AcademicYearId = newEfAcademicYear.AcademicYearId;
                efToUpdate.TeacherId = efTeacher.TeacherId;
                efToUpdate.TeacherPreferenceId = efTeacherPreference.TeacherPreferenceId;
                efToUpdate.CoordinationRoleId = efCoordinationRole.CoordinationRoleId;

                var efUpdate = _context.Update(efToUpdate).Entity;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // if updating in db throws an exception, we log it and re-throw
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        private SIS.Infrastructure.EFRepository.Models.TeacherCoordinationRoleInterest GetEFEntity(TeacherCoordinationRoleInterest roleInterest)
        {
            var efTeacherCoordinationRoleInterest = _context.TeacherCoordinationRoleInterests
                                                .Where(roleInterest => roleInterest.TeacherCoordinationRoleInterestId == roleInterest.TeacherCoordinationRoleInterestId)
                                                .FirstOrDefault();
            return efTeacherCoordinationRoleInterest;
        }
    }
}
