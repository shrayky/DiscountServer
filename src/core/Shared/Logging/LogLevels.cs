namespace Shared.Logging
{
    namespace Shared.Logging
    {
        public static class LogLevels
        {
            public const string Verbose = "verbose";
            public const string Debug = "debug";
            public const string Information = "information";
            public const string Warning = "warning";
            public const string Error = "error";
            public const string Fatal = "fatal";

            public static string GetDefaultLevel() => Information;

            public static bool IsValidLevel(string level)
            {
                if (string.IsNullOrWhiteSpace(level))
                    return false;

                if (string.IsNullOrEmpty(level))
                    return false;

                var normalizedLevel = level.ToLower();

                return normalizedLevel is
                    Verbose or
                    Debug or
                    Information or
                    Warning or
                    Error or
                    Fatal;
            }
        }
    }
}
