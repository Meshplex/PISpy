namespace PiSpyBackend.Domain.Models
{
    public class UpdateUserRequest
    {
        public int UserId { get; set; }

        public string? NewUsername { get; set; }
        public string? NewPassword { get; set; }
    }
}