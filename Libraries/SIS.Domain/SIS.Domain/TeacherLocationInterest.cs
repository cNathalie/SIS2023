using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Domain
{
    public class TeacherLocationInterest
    {
        public int TeacherLocationInterestId { get; set; }
        public DateTime AcademicYearStart { get; set; }
        public DateTime AcademicYearStop { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string TeacherPreferenceDescription { get; set; }
        public string LocationName { get; set; }
    }
}
