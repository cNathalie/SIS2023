namespace SIS.Domain
{
    public class TeacherCourseInterest
    {
        public int TeacherCourseInterestId { get; set; }

        public int AcademicYearId { get; set; }

        public int TeacherId { get; set; }

        public int TeacherPreferenceId { get; set; }

        public int CourseId { get; set; }
    }
}
