namespace SIS.Domain.Interfaces
{
    public interface ISISShedulingTimeslotRepository
    {
        public Dictionary<string, ShedulingTimeslot> ShedulingTimeslots { get; }
        public Dictionary<string, ShedulingTimeslot> RefreshShedulingTimeslots();
        public bool Exists(ShedulingTimeslot timeslot);
        public int Insert(ShedulingTimeslot timeslot);
        public void Update(ShedulingTimeslot timeslotToUpdate, ShedulingTimeslot newTimeslot);
        public void Delete(ShedulingTimeslot timeslot);
    }
}
