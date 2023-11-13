using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAPI.DTO
{
    public class TeacherCoordinationRoleInterestDTO
    {
        public int TeacherCoordinationRoleInterestId { get; set; }
        [Required]
        public DateOnly AcademicYearStart { get; set; }
        [Required]
        public DateOnly AcademicYearStop { get; set; }
        [Required]
        public string TeacherFirstName { get; set; }
        [Required]
        public string TeacherLastName { get; set; }
        [Required]
        public string TeacherPreference { get; set; }
        [Required]
        public string CoordinationRole { get; set; }
    }
}
