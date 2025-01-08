namespace PiSpyBackend.Domain
{
    public class Event
    {
        public int KeyId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

        public Event(int keyId, int userId, string description, DateTime timestamp)
        {
            this.KeyId = keyId;
            this.UserId = userId;
            this.Description = description;
            this.Timestamp = timestamp;
        }
    }
}