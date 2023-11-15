using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
using Tasks.Services;
using Tasks.Interfaces;
using System.Security.Claims;
using Tasks.Models;
using Microsoft.AspNetCore.Authorization;  
using Tasks.Services;
namespace Tasks.Controllers{
[ApiController]
    [Route("[controller]")]
    public class UserManagerControler: ControllerBase
    {
        private List<TaskUser> users;
        public UserManagerControler()
        {
            users = new List<TaskUser>
            {
                new TaskUser { UserId = 1, Username = "Leale", Password = "2144!", UserManager = true},
                new TaskUser { UserId = 2, Username = "Noa", Password = "57667?"},
                new TaskUser { UserId = 3, Username = "Yael", Password = "Y1234#"}
            };
        }
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] TaskUser User)
        {
            var dt = DateTime.Now;

            var user = users.FirstOrDefault(u =>
                u.Username == User.Username 
                && u.Password == User.Password
            );        

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.UserManager ? "UserManager" : "TaskUser"),
                new Claim("userId", user.UserId.ToString()),

            };

            var token =TaskTokenService.GetToken(claims);

            return new OkObjectResult(TaskTokenService.WriteToken(token));
        }
    }




}