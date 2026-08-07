using Authentication.Api.Domain.Enums;

namespace Authentication.Api.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;

        public UserRole Role { get; private set; }

        public AccountStatus Status { get; private set; }

        private User()
        {
            // Required by Entity Framework Core
        }

        public static User Create(
            string firstName,
            string lastName,
            string email,
            string passwordHash)
        {
            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = passwordHash,
                Role = UserRole.Customer,
                Status = AccountStatus.PendingVerification
            };
        }

        public void VerifyEmail()
        {
            if (Status != AccountStatus.PendingVerification)
            {
                throw new InvalidOperationException(
                    "Only users awaiting verification can verify their email.");
            }

            Status = AccountStatus.Active;
        }

        public void Suspend()
        {
            if (Status == AccountStatus.Suspended)
            {
                return;
            }

            Status = AccountStatus.Suspended;
        }

        public void ChangePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public void PromoteToAdministrator()
        {
            Role = UserRole.Admin;
        }
    }
}