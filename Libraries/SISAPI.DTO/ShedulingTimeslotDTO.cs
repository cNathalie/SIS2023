using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SISAPI.DTO
{
    public class ShedulingTimeslotDTO
    {
        public int SchedulingTimeslotId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public TimeOnly StartTime { get; set; }
        [Required] public TimeOnly StopTime { get; set; }
    }
}
