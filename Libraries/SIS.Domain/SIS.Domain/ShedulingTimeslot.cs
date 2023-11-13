namespace SIS.Domain
{
    public class ShedulingTimeslot

    {
        public int SchedulingTimeslotId {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
    }
}