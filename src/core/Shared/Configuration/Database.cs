namespace Shared.Configuration
{
    public class Database
    {
        public string NetAddress { get; set; } = "http://localhost:5984";
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int BulkBatchSize { get; set; } = 1000;
        public int BulkMaxParallelTasks { get; set; } = 4;
        
        public bool Validate()
        {
            return !string.IsNullOrEmpty(NetAddress)
                && !string.IsNullOrWhiteSpace(NetAddress)
                && !string.IsNullOrEmpty(UserName)
                && !string.IsNullOrWhiteSpace(UserName)
                && !string.IsNullOrEmpty(Password)
                && !string.IsNullOrWhiteSpace(Password);
        }

    }
}
