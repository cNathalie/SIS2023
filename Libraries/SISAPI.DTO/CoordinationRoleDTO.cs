using System.ComponentModel.DataAnnotations;

namespace SISAPI.DTO
{
    public class CoordinationRoleDTO
    {
        public int CoordinationRoleId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public int AssignmentPercentage { get; set; }
    }
}
