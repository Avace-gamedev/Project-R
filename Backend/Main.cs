using System;
using System.IO;
using System.Reflection;
using Avace.Backend.Interfaces.Logging;
using Avace.Backend.Kernel.Injection;

namespace Avace.Backend
{
    public static class Main
    {
        private static ILogger? _logger;

        public static void Initialize()
        {
            LoadAllAssemblies();
            Injector.Initialize();
            
            _logger = Injector.Get<ILoggerProvider>().GetLogger(MethodBase.GetCurrentMethod().Name);
            _logger.Info("Initialization done.");
        }

        private static void LoadAllAssemblies()
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                         ?? throw new InvalidOperationException("Could not locate executing assembly");
            foreach (string assemblyPath in Directory.EnumerateFiles(dir, "Avace*.dll"))
            {
                Assembly.LoadFile(assemblyPath);
            }
        }
    }
}