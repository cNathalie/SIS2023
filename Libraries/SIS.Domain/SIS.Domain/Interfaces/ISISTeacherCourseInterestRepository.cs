namespace SIS.Domain.Interfaces
{
    public interface ISISTeacherCourseInterestRepository
    {
        public Dictionary<string, TeacherCourseInterest> TeacherCourseInterests { get; }

        public Dictionary<string, TeacherCourseInterest> RefreshTeacherCourseInterests();
        public bool Exists(TeacherCourseInterest courseInterest);
        public void Insert(TeacherCourseInterest courseInterest);
        public void Update(TeacherCourseInterest courseInterest, TeacherCourseInterest newCourseInterest);
        public void Delete(TeacherCourseInterest courseInterest);
    }
}
