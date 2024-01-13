namespace Timelapse.CLI.Entities
{
    public class Period
    {
        public int PeriodId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? StoppedAt { get; set; }
        public string? Commentary { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; } = default!;

        public TimeSpan GetDuration()
        {
            var stoppedAt = StoppedAt ?? DateTime.UtcNow;

            return stoppedAt.ToLocalTime() - StartedAt.ToLocalTime();
        }
    }
}
