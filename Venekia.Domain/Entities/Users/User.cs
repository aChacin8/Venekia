using Venekia.Domain.Entities.Finance.Wallets;

namespace Venekia.Domain.Entities.Users
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string? PhoneNumber { get; private set; }
        public string? Address { get; private set; }

        public UserStatus Status { get; private set; } = UserStatus.Active;

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? DeactivatedAt { get; private set; }
        public Wallet Wallet { get; private set; } = null!;

        private User() { }
        public User(string firstName, string lastName, string email, string passwordHash, string? phoneNumber, string? address)
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateEmail(email);
            ValidatePhoneNumber(phoneNumber);
            ValidateAddress(address);

            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;
            Address = address;
            Status = UserStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        private void ValidateFirstName (string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new Exception("First Name is required");

            if (firstName.Length > 50)
                throw new Exception("First Name cannot exceed 50 characters");

            if (firstName.Length < 2)
                throw new Exception("First Name cannot be under 2 characters");
        }

        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last Name is required");

            if (lastName.Length > 50)
                throw new ArgumentException("Last Name cannot exceed 50 characters");

            if (lastName.Length < 2)
                throw new ArgumentException("Last Name cannot be under 2 characters");
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
                throw new ArgumentException("Email format is invalid");

            if (email.Length > 100)
                throw new ArgumentException ("Email cannot exceed 100 characters");
        }

        private void ValidatePhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return;
            var phonePattern = @"^\+?[1-9]\d{1,14}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, phonePattern))
                throw new ArgumentException("Phone number format is invalid");
        }

        private void ValidateAddress(string? address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return;
            if (address.Length > 200)
                throw new ArgumentException("Address cannot exceed 200 characters");
        }

        public void UpdateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new Exception("First Name is required");

            FirstName = firstName;
            Touch();
        }

        public void UpdateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new Exception("First Name is required");

            LastName = lastName;
            Touch();
        }

        public void UpdatePhoneNumber(string? phoneNumber) {
            PhoneNumber = phoneNumber;
            Touch();
        }
        public void UpdateAddress(string? address) {
           Address = address;
           Touch();
        } 
        public void UpdatePassword(string newPassword)
        {
            PasswordHash = newPassword;
            Touch();
        }

        public bool CanLogin(DateTime now)
        {
            return IsActive();
        }

        public void ChangeStatus(UserStatus status)
        {
            Status = status;

            if (status == UserStatus.Inactive)
            {
                DeactivatedAt = DateTime.UtcNow;
            }
            else
            {
                DeactivatedAt = null;
            }
        }

        //public void CompletePasswordChange()
        //{
        //    if (Status != UserStatus.ChangePassword)
        //        throw new InvalidOperationException("User status is not set to ChangePassword");
        //    Status = UserStatus.Active;
        //}
        public bool IsActive()
        {
            return Status == UserStatus.Active;
        }
        public void Deactivate()
        {
            Status = UserStatus.Inactive;
            DeactivatedAt = DateTime.UtcNow;
        }
        private void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }

    }
}