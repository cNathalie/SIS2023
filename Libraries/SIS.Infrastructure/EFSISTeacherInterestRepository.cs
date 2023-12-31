using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIS.Domain;
using SIS.Domain.Interfaces;
using SIS.Infrastructure.EFRepository.Context;

namespace SIS.Infrastructure
{
    public class EFSISTeacherInterestRepository : ISISTeacherInterestRepository
    {
        private readonly ILogger<EFSISTeacherInterestRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly SisDbContext _context;
        private static Dictionary<string, TeacherInterest> _teacherInterests = new();

        public Dictionary<string, TeacherInterest> TeacherInterests
        {
            get
            {
                if(_teacherInterests != null) return _teacherInterests;
                return RefreshInterests();
            }
        }

        //Ctor
        public EFSISTeacherInterestRepository(ILogger<EFSISTeacherInterestRepository> logger, IConfiguration configuration, SisDbContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;

            RefreshInterests();
        }


        public void Delete(TeacherInterest ti)
        {
            try
            {
                //get ef entity
                var efTi = _context.TeacherInterests.Find(ti.TeacherInterestId);
                if(efTi == null) return;
                //remove ef entity
                var efRemoved = _context.Remove(efTi).Entity;
                //save changes
                var count = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public bool Exists(TeacherInterest ti)
        {
            //check if the key exists in the dictionary
            return _teacherInterests.ContainsKey($"{ti.AcademicYearId}-{ti.TeacherId}");
        }

        public TeacherInterest Insert(TeacherInterest ti)
        {
            try
            {
                //create the ef entity
                var efTi = new SIS.Infrastructure.EFRepository.Models.TeacherInterest
                {
                    AcademicYearId = ti.AcademicYearId,
                    TeacherId = ti.TeacherId,
                    Description = ti.Description,
                };
                //add the ef entity to the dbcontext
                var efAdded = _context.TeacherInterests.Add(efTi).Entity;
                //save changes
                var count = _context.SaveChanges();

                //return domain entity
                ti.TeacherInterestId = efAdded.TeacherInterestId;
                return ti;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public Dictionary<string, TeacherInterest> RefreshInterests()
        {
            _teacherInterests.Clear();

            var dbInterests = _context.TeacherInterests.Include(t => t.AcademicYear).Include(t => t.Teacher).ToList();
            foreach(var dbInterest in dbInterests)
            {
                var interest = new TeacherInterest
                {
                    TeacherInterestId = dbInterest.TeacherInterestId,
                    AcademicYearId = dbInterest.AcademicYearId,
                    AcademicYear = dbInterest.AcademicYear.StartDate.ToString() + "-" + dbInterest.AcademicYear.StopDate.ToString(), // "2021-2022"
                    TeacherId = dbInterest.TeacherId,                                                                                                                
                    Teacher = dbInterest.Teacher.Person.FirstName + " " + dbInterest.Teacher.Person.LastName,
                    Description = dbInterest.Description,
                };
                _teacherInterests.Add($"{interest.AcademicYearId}-{interest.TeacherId}", interest);
            }
            return _teacherInterests;
        }

        public void Update(TeacherInterest ti, TeacherInterest newTi)
        {
            try
            {
                //get the ef entity
                var efTi = _context.TeacherInterests.Find(ti.TeacherInterestId);
                if(efTi == null) return;

                //update the ef entity
                efTi.Description = newTi.Description;

                //save changes
                _context.SaveChanges();
                RefreshInterests();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
