namespace pizzeria.Interfaces
{
    public interface ILogger
    {
        void LogError(string message);
        void LogWarning(string message);
        void LogInfo(string message);
    }
}
