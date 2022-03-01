using System.IO;
using System.Reflection;
using Avace.Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Backend
{
    [TestClass]
    public class Setup
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            Assembly.LoadFile(Path.Combine(assemblyLocation, "Tests.Utils.dll"));

            Main.Initialize();
        }
    }
}
