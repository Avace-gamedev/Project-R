using Avace.Backend.Interfaces.Logging;

namespace Logging
{
    public class UnityLoggerProvider: ILoggerProvider
    {
        public ILogger GetLogger(string name)
        {
            return new UnityLogger(name);
        }
    }
}