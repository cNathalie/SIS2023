using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Domain
{
    public class TeacherInterest
    {
        public int TeacherInterestId { get; set; }

        public int AcademicYearId { get; set; }

        public string? AcademicYear { get; set; }

        public int TeacherId { get; set; }
        public string? Teacher { get; set; }

        public string? Description { get; set; }
    }
}
