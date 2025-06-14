using pizzeria.Interfaces;

namespace pizzeria.Services
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger(string? logFilePath = null)
        {
            if (string.IsNullOrEmpty(logFilePath))
            {
                var logsDir = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "logs");
                if (!Directory.Exists(logsDir))
                {
                    Directory.CreateDirectory(logsDir);
                }
                logFilePath = Path.Combine(logsDir, $"pizzeria-{DateTime.Now:yyyyMMdd-HHmmss}.log");
            }
            _logFilePath = logFilePath;

        }

        public void LogError(string message)
        {
            var errorMessage = $"{DateTime.Now}: ERROR: {message}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, errorMessage);
        }
        public void LogWarning(string message)
        {
            var warningMessage = $"{DateTime.Now}: WARNING: {message}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, warningMessage);
        }
        public void LogInfo(string message)
        {
            var infoMessage = $"{DateTime.Now}: INFO: {message}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, infoMessage);
        }
    }
}
