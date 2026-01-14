using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Venekia.Application.DTOs.Auth;
using Venekia.Application.DTOs.Users;
using Venekia.Application.Interfaces.Users;

namespace Venekia.Api.Controllers.Auth
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
		public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
		{
			await _userService.RegisterUser(registerUserDto);
			return Ok(new
			{
				registerUserDto,
				message = "User registered succesfully"
			});
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginUserDto loginUserDto)
		{
			var token = await _userService.LoginUser(loginUserDto);
			return Ok(new
			{
				token,
				message = "User login succesfully"
			});
		}

		[Authorize]
		[HttpGet("me")]
		public async Task<IActionResult> GetUser([FromHeader(Name = "Authorization")] string authorization)
		{
			var token = authorization.Replace("Bearer ", "").Trim();

			var user = await _userService.GetByIdAsync(token);

			return Ok(user);
		}

		[Authorize]
		[HttpPatch("update/me")]
		public async Task<IActionResult> Update([FromHeader(Name = "Authorization")] string authorization, [FromBody] UpdateUserDto updateUserDto)
		{
			var token = authorization.Replace("Bearer ", "").Trim();

			var result = await _userService.UpdateUser(token, updateUserDto);

			return Ok(new
			{
				message = "User updated successfully"
			});
		}

	}
}