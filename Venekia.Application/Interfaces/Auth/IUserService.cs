using Venekia.Application.DTOs.Auth;
using Venekia.Application.Services.Auth;

namespace Venekia.Application.Interfaces.Auth
{
    public interface IUserService
    {
        Task<string?> RegisterUser(RegisterUserDto registerUserDto);
        Task<string> LoginUser (LoginUserDto loginUserDto);
        Task<string?> GetByIdAsync(Guid Id);
        Task <string?> UpdateUser (UpdateUserDto updateUserDto);
    }
}