using Avace.Backend.Interfaces.Logging;

namespace Tests.Utils
{
    public class LoggerProvider : ILoggerProvider
    {
        private ILogger Logger { get; }

        public LoggerProvider(): this(new LoggerForTest())
        {
        }

        public LoggerProvider(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger GetLogger(string name)
        {
            return Logger;
        }
    }
}
