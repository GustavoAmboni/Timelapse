namespace Timelapse.CLI.Entities
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Anchor { get; set; } = string.Empty;
        public IEnumerable<Period> Periods { get; set; } = new HashSet<Period>();

        public TimeSpan GetTotalDuration()
        {
            return new TimeSpan(Periods
                .Select(s => s.GetDuration())
                .Sum(s => s.Ticks));
        }

        public bool IsRunning()
        {
            return Periods.Last().StoppedAt is null;
        }
    }
}
