using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Context;
using MyToDo.Api.Services;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            this._loginService = loginService;
        }

        [HttpGet]
        public async Task<ApiResponse?> Login([FromBody] UserDto userDto) => await _loginService.LoginAsync(userDto);

        [HttpPost]
        public async Task<ApiResponse?> Register([FromBody]UserDto userDto) => await _loginService.RegisterAsync(userDto);
    }
}
