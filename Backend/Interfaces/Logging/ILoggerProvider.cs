namespace Avace.Backend.Interfaces.Logging
{
    public interface ILoggerProvider
    {
        ILogger GetLogger(string name);
    }
}