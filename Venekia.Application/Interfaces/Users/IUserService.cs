using Venekia.Application.DTOs.Auth;
using Venekia.Application.DTOs.Users;

namespace Venekia.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterUser(RegisterUserDto registerUserDto);
        Task<string> LoginUser (LoginUserDto loginUserDto);
        Task<UserResponseDto> GetByIdAsync(string token);
        Task <UserResponseDto> UpdateUser (string token, UpdateUserDto updateUserDto);
    }
}