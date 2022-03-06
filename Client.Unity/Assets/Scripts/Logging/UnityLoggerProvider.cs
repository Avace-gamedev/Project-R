using Avace.Backend.Interfaces.Logging;

namespace Logging
{
    public class UnityLoggerProvider: ILoggerProvider
    {
        public ICustomLogger GetLogger(string name)
        {
            return new UnityLogger(name);
        }
    }
}