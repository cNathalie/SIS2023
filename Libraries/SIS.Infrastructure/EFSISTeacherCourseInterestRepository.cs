using Microsoft.Extensions.Logging;
using SIS.Domain;
using SIS.Domain.Interfaces;
using SIS.Infrastructure.EFRepository.Context;

namespace SIS.Infrastructure
{
    public class EFSISTeacherCourseInterestRepository : ISISTeacherCourseInterestRepository
    {
        private readonly ILogger<EFSISTeacherCourseInterestRepository> _logger;
        private readonly SisDbContext _context;
        private readonly Dictionary<string, TeacherCourseInterest> _teacherCourseInterests = new();

        public Dictionary<string, TeacherCourseInterest> TeacherCourseInterests
        {
            get
            {
                if( _teacherCourseInterests != null) return _teacherCourseInterests;
                else return RefreshTeacherCourseInterests();
            }
        }

        public void Delete(TeacherCourseInterest courseInterest)
        {
            try
            {
                //get ef object
                var efObject = _context.TeacherCourseInterests.Where(x => x.TeacherCourseInterestId == courseInterest.TeacherCourseInterestId).FirstOrDefault();
                if (efObject != null) { throw new Exception("TeacherCourseInterest not found"); }
                //Delete
                _context.TeacherCourseInterests.Remove(efObject);
                _context.SaveChanges();
                //refresh
                RefreshTeacherCourseInterests();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting TeacherCourseInterest");
                throw;
            }
        }

        public bool Exists(TeacherCourseInterest courseInterest)
        {
            //check if exists in dictionary
            if (_teacherCourseInterests.ContainsKey($"{courseInterest.AcademicYearId}-{courseInterest.TeacherId}")) return true;
            else return false;
        }

        public void Insert(TeacherCourseInterest courseInterest)
        {
            try
            {
                //creta ef object
                var efInterest = new Infrastructure.EFRepository.Models.TeacherCourseInterest()
                {
                    AcademicYearId = courseInterest.AcademicYearId,
                    TeacherId = courseInterest.TeacherId,
                    TeacherPreferenceId = courseInterest.TeacherPreferenceId,
                    CourseId = courseInterest.CourseId
                };
                //insert
                _context.TeacherCourseInterests.Add(efInterest);
                //Save  
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting TeacherCourseInterest");
                throw;
            }
        }

        public Dictionary<string, TeacherCourseInterest> RefreshTeacherCourseInterests()
        {
            _teacherCourseInterests.Clear();
            //Get all ef objects
            var efObjects = _context.TeacherCourseInterests.ToList();
            //Convert to domain objects
            foreach(var courseInterest in efObjects)
            {
                var interest = new TeacherCourseInterest
                {
                    TeacherCourseInterestId = courseInterest.TeacherCourseInterestId,
                    AcademicYearId = courseInterest.AcademicYearId,
                    TeacherId = courseInterest.TeacherId,
                    TeacherPreferenceId = courseInterest.TeacherPreferenceId,
                    CourseId = courseInterest.CourseId
                };
                _teacherCourseInterests.Add($"{interest.AcademicYearId}-{interest.TeacherId}", interest);
            }
            return _teacherCourseInterests;
        }

        public void Update(TeacherCourseInterest courseInterest, TeacherCourseInterest newCourseInterest)
        {
            try
            {
                //get ef object
                var efObject = _context.TeacherCourseInterests.Where(x => x.TeacherCourseInterestId == courseInterest.TeacherCourseInterestId).FirstOrDefault();
                if (efObject != null) { throw new Exception("TeacherCourseInterest not found"); }
                //update
                efObject.AcademicYearId = newCourseInterest.AcademicYearId;
                efObject.TeacherId = newCourseInterest.TeacherId;
                efObject.TeacherPreferenceId = newCourseInterest.TeacherPreferenceId;
                efObject.CourseId = newCourseInterest.CourseId;
                //Save
                _context.SaveChanges();
                //refresh
                RefreshTeacherCourseInterests();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating TeacherCourseInterest");
                throw;
            }
        }
    }
}
