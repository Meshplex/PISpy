using Microsoft.AspNetCore.Mvc;

namespace PiSpyBackend.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPut("register")]
        public void RegisterUser(){}
        
        [HttpPost("login")]
        public void LoginUser(){}
        
        [HttpPost("update")]
        public void UpdateUser(){}
        
        [HttpDelete("delete")]
        public void DeleteUser(){}
    }
}