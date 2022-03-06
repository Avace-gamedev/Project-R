namespace Avace.Backend.Interfaces.Logging;

public interface ILoggerProvider
{
    ICustomLogger GetLogger(string name);
}