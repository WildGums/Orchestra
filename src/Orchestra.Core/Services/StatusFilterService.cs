namespace Orchestra.Services
{
    public class StatusFilterService : IStatusFilterService
    {
        public bool IsSuspended { get; set; }

        public string GetStatus(string status)
        {
            if (IsSuspended)
            {
                return null;
            }

            // Default implementation just passes through
            return status;
        }
    }
}
