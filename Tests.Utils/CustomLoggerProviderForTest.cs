using Avace.Backend.Interfaces.Logging;

namespace Tests.Utils
{
    public class CustomLoggerProviderForTest : ICustomLoggerProvider
    {
        public CustomLoggerProviderForTest() : this(new LoggerForTest())
        {
        }

        public CustomLoggerProviderForTest(ICustomLogger logger)
        {
            Logger = logger;
        }

        private ICustomLogger Logger { get; }

        public ICustomLogger GetLogger(string name)
        {
            return Logger;
        }
    }
}