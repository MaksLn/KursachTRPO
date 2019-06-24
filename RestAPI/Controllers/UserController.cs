using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KursachTRPO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RestAPI.Services;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    [ClaimRequirement]
    public class UserController : ControllerBase
    {
        private DataBaseContext dataBaseContext;
        private GetHashPassword getHashPassword;

        public UserController(DataBaseContext context, GetHashPassword hashPassword)
        {
            getHashPassword = hashPassword;
            dataBaseContext = context;
        }

        [HttpGet]
        public IEnumerable<UserModel> GetUsers()
        {
            return dataBaseContext.Users.Include(e => e.Role).Select(e => ConvertToUserModel(e));
        }

        [HttpGet("{login}")]
        public IEnumerable<UserModel> GetUser(string login)
        {
            return dataBaseContext.Users.Include(e => e.Role).Where(e => e.Login == login).Select(e => ConvertToUserModel(e));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                User user = dataBaseContext.Users.Where(e => e.Login == userModel.Name).FirstOrDefault();

                if (user == null)
                {
                    dataBaseContext.Users.Add(new User
                    {
                        Email = userModel.Email,
                        Login = userModel.Name,
                        Password = getHashPassword.GetHashString(userModel.Name, userModel.Password),
                        Role = await dataBaseContext.Roles.Where(e => e.Name == userModel.Role).FirstOrDefaultAsync()
                    });
                    await dataBaseContext.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{login}")]
        public async Task<IActionResult> PutUser(string login, [FromBody] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                User user = dataBaseContext.Users.Where(e => e.Login == login).FirstOrDefault();

                if (user == null)
                {
                    return BadRequest("пользователь не существует");
                }
                else
                {
                    user.Email = userModel.Email;
                    user.Login = userModel.Name;
                    user.Password = getHashPassword.GetHashString(userModel.Name, userModel.Password);
                    user.Role = await dataBaseContext.Roles.Where(e => e.Name == userModel.Role).FirstOrDefaultAsync();

                    dataBaseContext.Users.Update(user);

                    await dataBaseContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status201Created);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{login}")]
        public async Task<IActionResult> DeleteUser(string login)
        {
            try
            {
                dataBaseContext.Remove(await dataBaseContext.Users.Where(e=>e.Login==login).FirstOrDefaultAsync());
                await dataBaseContext.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        private UserModel ConvertToUserModel(User user) => new UserModel {
            Email = user.Email,
            Name = user.Login,
            Password = user.Password,
            Role = user.Role.Name };

    }
}