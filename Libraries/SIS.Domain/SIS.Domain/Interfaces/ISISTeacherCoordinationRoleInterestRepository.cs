namespace SIS.Domain.Interfaces
{
    public interface ISISTeacherCoordinationRoleInterestRepository
    {
        public Dictionary<string, TeacherCoordinationRoleInterest> TeacherCoordinationRoleInterests { get; }

        public Dictionary<string, TeacherCoordinationRoleInterest> RefreshTeacherCoordinationRoleInterests();
        public bool Exists(TeacherCoordinationRoleInterest teacherCoordinationRoleInterest);
        public int Insert(TeacherCoordinationRoleInterest teacherCoordinationRoleInterest);
        public void Update(TeacherCoordinationRoleInterest teacherCoordinationRoleInterestToUpdate, TeacherCoordinationRoleInterest newTeacherCoordinationRoleInterest);
        public void Delete(TeacherCoordinationRoleInterest teacherCoordinationRoleInterest);

    }
}
