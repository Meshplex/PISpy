using FluentResults;
using PiSpyBackend.Domain;
using PiSpyBackend.Domain.Models;
using PiSpyBackend.Infrastructure;

namespace PiSpyBackend.Application
{
    public class UserService
    {

        private IDbUserRepository Repo { get; set; }

        public UserService(AppDbContext context)
        {
            this.Repo = new DbUserRepository(context);
        }

        public Result<UserDto[]> GetAllUsers()
        {
            var users = Repo.GetAllUsers();
            if (users.IsFailed)
            {
                return Result.Fail("Failed to get all users");
            }

            var userDtos = new List<UserDto>();
            foreach (var user in users.Value)
            {
                userDtos.Add(new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username
                });
            }
            return Result.Ok(userDtos.ToArray());
        }

        public Result CreateUser(User blankUser)
        {
            //Verify the input is valid
            if (string.IsNullOrEmpty(blankUser.Username) || string.IsNullOrEmpty(blankUser.Password))
            {
                Result.Fail("the given blank User object was empty or its values are empty");
            }

            // Hash password before adding the user to the Database
            User user = new()
            {
                Username = blankUser.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(blankUser.Password, workFactor: 12)
            };

            // Add User to database
            var result = Repo.AddUserToDatabase(user);
            if (result.IsFailed)
            {
                return Result.Fail("Failed to create useraccount");
            }
            return Result.Ok();
        }

        public Result ChangeUsername(int userId, string newUsername)
        {
            if (userId == 0 || userId < 0 || string.IsNullOrEmpty(newUsername))
            {
                return Result.Fail("Failed to change password of user because the given id was wrong");
            }
            var userToUpdate = Repo.FindUserFromId(userId);
            if (userToUpdate.IsFailed || userToUpdate.Value == null)
            {
                return Result.Fail("Failed to change password the user does not exsist");
            }

            var result = Repo.UpdateData(userId, null, newUsername);
            if (result.IsFailed)
            {
                return Result.Fail("Failed to change password of user");
            }
            return Result.Ok();
        }

        public Result ChangeUserPassword(int userId, string newPassword)
        {
            if (userId == 0 || userId < 0 || string.IsNullOrEmpty(newPassword))
            {
                return Result.Fail("Failed to change password of user because the given id was wrong");
            }
            var userToUpdate = Repo.FindUserFromId(userId);
            if (userToUpdate.IsFailed || userToUpdate.Value == null)
            {
                return Result.Fail("Failed to change password the user does not exsist");
            }

            var result = Repo.UpdateData(userId, BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 12), null);
            if (result.IsFailed)
            {
                return Result.Fail("Failed to change password of user");
            }
            return Result.Ok();
        }

        public Result DeleteUser(int userId)
        {
            //Validate User ID 
            if (userId == 0 || userId < 0)
            {
                return Result.Fail("Failed to delete user because the id was wrong");
            }

            // Delete User from Database
            var result = Repo.DeleteUserFromDatabase(userId);

            //Check for Failed result
            if (result.IsFailed)
            {
                return Result.Fail("Failed to delete user");
            }
            return Result.Ok();
        }

        public Result<User> LoginUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Result.Fail("Failed to login user because the given username or password was empty");
            }

            var user = Repo.GetUserByUsername(username);
            if (user.IsFailed)
            {
                return Result.Fail("Failed to login user because the given username was not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Value.Password))
            {
                return Result.Fail("Failed to login user because the given password was wrong");
            }
            return Result.Ok(user.Value);
        }
    }
}