namespace Authentication.Domain.Applications
{
    public sealed class ApplicationRole
    {
        private ApplicationRole()
        {
        }

        public Guid Id { get; private set; }

        public Guid ApplicationId { get; private set; }

        public string Name { get; private set; } = null!;

        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? UpdatedAt { get; private set; }

        public static ApplicationRole Create(
            Guid applicationId,
            string name)
        {
            return new ApplicationRole
            {
                Id = Guid.NewGuid(),
                ApplicationId = applicationId,
                Name = name,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}