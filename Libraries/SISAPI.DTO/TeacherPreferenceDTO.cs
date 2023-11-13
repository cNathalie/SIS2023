using System.ComponentModel.DataAnnotations;

namespace SIS.API.DTO
{
    public class TeacherPreferenceDTO
    {
        public int TeacherPreferenceId { get; set; }
        [Required] [Range(1,10)] public int Preference { get; set; }
        [Required] public string Description { get; set; }
    }
}
