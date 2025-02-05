using FluentResults;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Infrastructure
{
    public class DbUserRepository : IDbUserRepository
    {
        private AppDbContext context { get; set; }

        public DbUserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Result<User> GetUserByUsername(string username)
        {
            var userToFind = context.Users.FirstOrDefault(userToFind => userToFind.Username == username);
            if (userToFind == null)
            {
                return Result.Fail("Es wurde kein Nutzer mit gegebenen Username Gefunden");
            }
            return Result.Ok(userToFind);
        }

        public Result<User> FindUserFromId(int id){
            var userToFind = context.Users.FirstOrDefault(userToFind => userToFind.Id == id);
            if (userToFind == null)
            {
                return Result.Fail("Es wurde kein Nutzer mit gegebenen ID Gefunden");
            }
            return Result.Ok(userToFind);
        }

        public Result AddUserToDatabase(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return Result.Ok();
        }

        public Result DeleteUserFromDatabase(int userId)
        {
            // Check if the given User ID is Matching to a user account
            var userToDel = context.Users.FirstOrDefault(userToDel => userToDel.Id == userId);
            if (userToDel is null)
            {
                return Result.Fail("Es wurde kein nutzer mit der gegebenen ID gefunden");
            }

            // Remove user from Database
            context.Users.Remove(userToDel);
            context.SaveChanges();
            return Result.Ok();
        }

        public Result UpdateData(int userId, string password)
        {
             // Check if the given User ID is Matching to a user account
            var userToUpdate = context.Users.FirstOrDefault(userToDel => userToDel.Id == userId);
            if (userToUpdate is null)
            {
                return Result.Fail("Es wurde kein nutzer mit der gegebenen ID gefunden");
            }
            userToUpdate.Password = password;
            context.Users.Update(userToUpdate);
            context.SaveChanges();
            return Result.Ok();
        }
    }
}