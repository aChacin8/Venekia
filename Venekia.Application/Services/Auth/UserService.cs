using Venekia.Application.DTOs.Auth;
using Venekia.Application.DTOs.Users;
using Venekia.Application.DTOs.Security;
using Venekia.Application.Interfaces.Auth;
using Venekia.Application.Interfaces.Users;
using Venekia.Domain.Entities.Users;

namespace Venekia.Application.Services.Auth
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; 
        private readonly IPasswordHasher _passwordHash;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHash, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHash = passwordHash;
            _jwtService = jwtService; 
        }

        public async Task<UserResponseDto>RegisterUser(RegisterUserDto registerUserDto)
        {
            var existing = await _userRepository.GetByEmailAsync(registerUserDto.Email);

            if(existing != null)
                throw new Exception ("Email already exists");

            var hashedPassword = _passwordHash.HashPassword(registerUserDto.Password);

            var user = new User (registerUserDto.FirstName, registerUserDto.LastName, registerUserDto.Email, hashedPassword, registerUserDto.PhoneNumber, registerUserDto.Address);

            await _userRepository.AddAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Status = user.Status.ToString()
            };
        }

        public async Task<string> LoginUser (LoginUserDto loginUserDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginUserDto.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid Credentials");

            var isValid = _passwordHash.VerifyPassword(loginUserDto.Password, user.PasswordHash);

            if(!isValid)
                throw new UnauthorizedAccessException ("Invalid Credentials");

            if (!user.IsActive())
                throw new UnauthorizedAccessException("User is inactive");

            var identity = new UserClaims
            {
                Id = user.Id,
                Email = user.Email
            };
            return _jwtService.GenerateToken(identity);
        }

        public async Task <UserResponseDto> GetByIdAsync(string token)
        {
            var claims = _jwtService.VerifyToken(token);

            var user = await _userRepository.GetByIdAsync(claims.Id);
            if (user == null)
                throw new Exception("User not found");

            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Status = user.Status.ToString()
            };
        }

        public async Task<UserResponseDto> UpdateUser(string token, UpdateUserDto updateUserDto)
        {
            var claims = _jwtService.VerifyToken(token);

            var user = await _userRepository.GetByIdAsync(claims.Id);

            if (user == null)
                throw new Exception("User not found");

            var isValidPassword = _passwordHash.VerifyPassword(updateUserDto.CurrentPassword!, user.PasswordHash);
            if (!isValidPassword)
                throw new UnauthorizedAccessException("Invalid Password");

            if (!string.IsNullOrWhiteSpace(updateUserDto.FirstName))
                user.UpdateFirstName(updateUserDto.FirstName);

            if (!string.IsNullOrWhiteSpace(updateUserDto.LastName))
                user.UpdateLastName(updateUserDto.LastName);

            if (!string.IsNullOrWhiteSpace(updateUserDto.PhoneNumber))
                user.UpdatePhoneNumber(updateUserDto.PhoneNumber);

            if (!string.IsNullOrWhiteSpace(updateUserDto.Address))
                user.UpdateAddress(updateUserDto.Address);

            if (!string.IsNullOrWhiteSpace(updateUserDto.NewPassword))
            {
                //if (user.Status == UserStatus.ChangePassword)
                //    throw new InvalidOperationException("Status Changed. Update your password using ChangePassword flow");

                var newPasswordHash = _passwordHash.HashPassword(updateUserDto.NewPassword);
                user.UpdatePassword(newPasswordHash);
            }

            await _userRepository.UpdateAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Status = user.Status.ToString()
            };
        }
    }
}