namespace SIS.Domain.Interfaces
{
    public interface ISISPeriodRepository
    {
        public Dictionary<string, Period> Periods { get; }

        public Dictionary<string, Period> RefreshPeriods();
        public bool Exists(Period period);
        public int Insert(Period newPeriod);
        public void Update(Period periodToUpdate, Period newPeriod);
        public void Delete(Period periodToDelete);
    }
}
