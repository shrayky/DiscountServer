namespace Shared.Configuration
{
    public class Exchange
    {
        public string FolderPath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(FolderPath) 
                && !string.IsNullOrEmpty(FolderPath)
                && !string.IsNullOrWhiteSpace(FileName)
                && !string.IsNullOrEmpty(FileName);
        }

        public string FullFileName()
        {
            return Path.Combine(FolderPath, FileName);
        }
    }
}
