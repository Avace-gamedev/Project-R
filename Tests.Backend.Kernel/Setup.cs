using Avace.Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Backend.Kernel
{
    public class Setup
    {
        [AssemblyInitialize]
        public void Initialize()
        {
            Main.Initialize();
        }
    }
}
