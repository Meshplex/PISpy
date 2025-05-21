using FluentResults;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Infrastructure
{
    public interface IDbUserRepository
    {
        public Result<User[]> GetAllUsers();
        public Result<User> FindUserFromId(int id);
        public Result<User> GetUserByUsername(string username);
        public Result AddUserToDatabase(User user);

        public Result DeleteUserFromDatabase(int userId);

        public Result UpdateData(int userId, string? password, string? username);
    }
}