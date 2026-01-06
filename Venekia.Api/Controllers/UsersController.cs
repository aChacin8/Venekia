using Microsoft.AspNetCore.Mvc;

using Venekia.Application.Interfaces.Auth;
using Venekia.Application.DTOs.Auth;
using Venekia.Domain.Entities;

namespace Venekia.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersControllers : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersControllers(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task <IActionResult> Register (RegisterUserDto registerUserDto)
        {
            await _userService.RegisterUser(registerUserDto);
            return Ok(new
            {
                registerUserDto,
                message = "User registered succesfully"
            });
        }

        [HttpPost("login")]
        public async Task <IActionResult> Login (LoginUserDto loginUserDto)
        {
            var token = await _userService.LoginUser(loginUserDto);
            return Ok(new
            {
                token,
                message = "User login succesfully"
            });
        }

        [HttpGet("{id:guid}")]
        public async Task <IActionResult> GetUser (Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(new
            {
                id,
                message = "User retrieved succesfully"
            });
        }

        [HttpPatch("update/{id:guid}")]
        public async Task <IActionResult> Update ([FromRoute] Guid id, [FromBody]UpdateUserDto updateUserDto)
        {
            updateUserDto.Id = id;

            await _userService.UpdateUser(updateUserDto);
            return Ok(new
            {
                message = "User Update succesfully"
            });
        }
    }
}