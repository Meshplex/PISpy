namespace PiSpyBackend.Domain
{
    public class Key
    {
        public int KeyId { get; set; }
        public int UserId { get; set; }
        public string KeyValue { get; set; }

        public Key(int keyId, int userId, string keyValue)
        {
            this.KeyId = keyId;
            this.UserId = userId;
            this.KeyValue = keyValue;
        }
    }
}