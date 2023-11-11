using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAPI.DTO
{
    public class TeacherLocationInterestDTO
    {
        public int TeacherLocationInterestId { get; set; }
        public DateOnly AcademicYearStart { get; set; }
        public DateOnly AcademicYearStop { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string TeacherPreferenceDescription { get; set; }
        public string LocationName { get; set; }
    }
}
