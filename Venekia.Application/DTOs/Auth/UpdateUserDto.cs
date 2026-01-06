namespace Venekia.Application.DTOs.Auth
{
    public class UpdateUserDto
    {
        public Guid Id {get; set;}
        public required string CurrentPassword {get; set;}

        public string? Email {get; set;}

        public string? FirstName { get; set;}
        public  string? LastName { get; set; }
        public  string? NewPassword { get; set; }
        public string?  PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
