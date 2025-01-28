using FluentResults;
using PiSpyBackend.Domain;
using PiSpyBackend.Infrastructure;

namespace PiSpyBackend.Application {
    public class UserService {

        private IDbUserRepository Repo { get; set; }

        public UserService(AppDbContext context)
        {
            this.Repo = new DbUserRepository(context);
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
            if (result.IsFailed){
                return Result.Fail("Failed to create useraccount");
            }
            return Result.Ok();
        }

        public Result ChangeUserPassword(int userId, string newPassword)
        {
            //Validate the given userId & password
            if (userId == 0 || userId < 0 || string.IsNullOrEmpty(newPassword))
            {
                return Result.Fail("Failed to change password of user because the given id was wrong");
            }

            // Update user in Database
            var result = Repo.UpdateData(userId, BCrypt.Net.BCrypt.HashPassword(newPassword, workFactor: 12));
            
            //Check for failed Result
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
            if(result.IsFailed)
            {
                return Result.Fail("Failed to delete user");
            }
            return Result.Ok();
        }
    }
}