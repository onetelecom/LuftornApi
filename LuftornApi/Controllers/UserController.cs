using BussinessLayer.Services.UserServices;
using DataLayer.DTOs.UserDTOS;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LuftornApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
           _userService = userService;
        }
        [Authorize]
        [HttpGet,Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUser());
        }
        
        [HttpGet, Route("DeleteAll")]
        public IActionResult DeleteAll()
        {
           
            return Ok(_userService.DeleteAllUser());
        }
        [Authorize]
        [HttpGet, Route("GetUserById")]
        public IActionResult GetUserById(Guid UserId)
        {
            return Ok(_userService.GetUserById(UserId));
        }
        [HttpPost, Route("RegisterUser")]
        public IActionResult RegisterUser(UserDTO user)
        {
            return Ok(_userService.AddUser(user));
        }
        [HttpPost, Route("SocialLogin")]
        public IActionResult SocialLogin(UserDTO user)
        {
            return Ok(_userService.LoginSocialFaceBook(user));
        }
        [Authorize]
        [HttpPost, Route("UpdateUser")]
        public IActionResult UpdateUser(UpdateUserDTO user)
        {
            return Ok(_userService.UpdateUser(user));
        }
        [Authorize]
        [HttpGet, Route("DeleteUser")]
        public IActionResult DeleteUser(Guid UserId)
        {
            return Ok(_userService.DeleteUser(UserId));
        }
        [HttpPost,Route("Login")]
        public IActionResult Login(LoginDTO Login)
        {
            return Ok(_userService.Login(Login));
        }
    }
}
