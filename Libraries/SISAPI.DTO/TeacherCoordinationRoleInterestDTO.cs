using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAPI.DTO
{
    public class TeacherCoordinationRoleInterestDTO
    {
        public int TeacherCoordinationRoleInterestId { get; set; }
        public DateTime AcademicYearStart { get; set; }
        public DateTime AcademicYearStop { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string TeacherAbbreviation { get; set; }
        public string TeacherPreference { get; set; }
        public string CoordinationRole { get; set; }
    }
}
