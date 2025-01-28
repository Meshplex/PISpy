using FluentResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PiSpyBackend.Domain;
using PiSpyBackend.Infrastructure;

namespace PiSpyBackend.Application
{
    class MapKeyToUserService
    {
        private IDbKeyRepository keyRepo { get; set; }
        private IDbUserRepository userRepo { get; set; }

        public MapKeyToUserService(AppDbContext context)
        {
            keyRepo = new DbKeyRepository(context);
            userRepo = new DbUserRepository(context);
        }

        public Result AddKey(string keyId, int userId, string keyValue)
        {
            // Check if Key is already in DB
            var keyToFindResult = keyRepo.FindKey(keyId);
            if (keyToFindResult.IsSuccess)
            {
                return Result.Fail("Es exsistiert bereits ein Key mit dieser ID");
            }

            var result = keyRepo.AddKey(keyId, userId, keyValue);
            if (result.IsFailed)
            {
                return Result.Fail("Der Key konnte nicht hinzugefügt werden");
            }
            return Result.Ok();
        }

        public Result RemoveKey(string keyId)
        {
            // Check if Key exists
            var keyToFindResult = keyRepo.FindKey(keyId);
            if (keyToFindResult.IsFailed)
            {
                return Result.Fail("Es exsistiert kein Key mit dieser ID");
            }

            // Delete key from DB
            var result = keyRepo.RemoveKey(keyId);
            if (result.IsFailed)
            {
                return Result.Fail("Der Key konnte nicht gelöscht werden");
            }
            return Result.Ok();
        }

        public Result<User> FindUserFromKey(string keyId)
        {
            var key = keyRepo.FindKey(keyId);
            if (key == null)
            {
                return Result.Fail("Es konnte ein Schlüssel mit gegebenen ID gefunden werden");
            }
            var userId = key.Value.UserId;
            if (userId == 0 || userId <= 0 || userId >= int.MaxValue)
            {
                return Result.Fail("Die im Schlüssel hinterlegte User ID war fehlerhaft");
            }
            var userToFindResult = userRepo.FindUserFromId(userId);
            if (userToFindResult.IsFailed)
            {
                return Result.Fail("Es konnte kein Nutzer mit der hinterlegten ID gefunden werden");
            }
            return Result.Ok(userToFindResult.Value);
        }
    }
}