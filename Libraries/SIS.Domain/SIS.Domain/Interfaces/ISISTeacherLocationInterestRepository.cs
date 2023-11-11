using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Domain.Interfaces
{
    public interface ISISTeacherLocationInterestRepository
    {
        public Dictionary<int, TeacherLocationInterest> TeacherLocationInterests { get; }
        public Dictionary<int, TeacherLocationInterest> RefreshTeacherLocationInterests();
        public bool Exists(TeacherLocationInterest teacherLocationInterest);
        public int Insert(TeacherLocationInterest teacherLocationInterest);
        public void Update(TeacherLocationInterest teacherLocationInterestToUpdate, TeacherLocationInterest newteacherLocationInterest);
        public void Delete(TeacherLocationInterest teacherLocationInterest);
    }
}
