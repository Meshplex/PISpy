using System.Security.Cryptography.X509Certificates;
using FluentResults;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Infrastructure
{
    public class DbKeyRepository : IDbKeyRepository
    {
        private AppDbContext context;

        public DbKeyRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Result<Key> FindKey(string keyId)
        {
            var keyToFind = context.keys.FirstOrDefault(keyToFind => keyToFind.KeyId == keyId);
            if (keyToFind is null)
            {
                return Result.Fail("Es wurde kein key mit der gegebenen ID gefunden");
            }
            return Result.Ok<Key>(keyToFind);
        }

        public Result AddKey(string keyId, int userId, string? keyValue)
        {
            var newKey = new Key{
                KeyId = keyId,
                UserId = userId,
                KeyValue = keyValue ?? ""
            };
            Console.WriteLine("context.keys: ");
            Console.Write(context.keys);
            context.keys.Add(newKey);
            return Result.Ok();
        }

        public Result RemoveKey(string keyId)
        {
            var keyToDelete = context.keys.FirstOrDefault(d => d.KeyId == keyId);
            if (keyToDelete is null || keyToDelete.KeyId != keyId)
            {
                return Result.Fail("The Key given was not in the list");
            }
            context.keys.Remove(keyToDelete);
            return Result.Ok();
        }

        public Result<Key[]> GetKeysFromUser(int userId)
        {
            var keys = context.keys.Where(k => k.UserId == userId);
            if (keys is null || keys.Count() == 0)
            {
                return Result.Fail("No keys found for the given user");
            }
            return Result.Ok(keys.ToArray());
        }
    }
}