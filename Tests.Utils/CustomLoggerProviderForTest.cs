using Avace.Backend.Interfaces.Logging;

namespace Tests.Utils
{
    public class CustomLoggerProviderForTest : ICustomLoggerProvider
    {
        private ICustomLogger Logger { get; }

        public CustomLoggerProviderForTest(): this(new LoggerForTest())
        {
        }

        public CustomLoggerProviderForTest(ICustomLogger logger)
        {
            Logger = logger;
        }

        public ICustomLogger GetLogger(string name)
        {
            return Logger;
        }
    }
}
