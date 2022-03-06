namespace Avace.Backend.Interfaces.Logging
{
    public interface ICustomLoggerProvider
    {
        ICustomLogger GetLogger(string name);
    }
}