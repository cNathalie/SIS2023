using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Domain.Interfaces
{
    public interface ISISTeacherInterestRepository
    {
        public Dictionary<string, TeacherInterest> TeacherInterests { get; }

        public Dictionary<string, TeacherInterest> RefreshInterests();
        public bool Exists(TeacherInterest ti);
        public TeacherInterest Insert(TeacherInterest ti);
        public void Update(TeacherInterest ti, TeacherInterest newTi);
        public void Delete(TeacherInterest ti);
    }
}
