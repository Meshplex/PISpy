namespace PiSpyBackend.Domain
{
    public class Event
    {
        public int EventId {get; set; }
        public string? KeyId { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public DateTime Timestamp { get; set; }
    }
}