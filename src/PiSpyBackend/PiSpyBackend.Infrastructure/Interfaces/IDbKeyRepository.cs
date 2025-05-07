using FluentResults;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Infrastructure
{
    public interface IDbKeyRepository
    {
        public Result<Domain.Key> FindKey (string keyId); 
        public Result AddKey(string keyId, int userId, string? keyValue);
        public Result RemoveKey(string keyId);
        public Result<Key[]> GetKeysFromUser(int userId);
    }
}