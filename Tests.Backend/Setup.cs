using Avace.Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Backend
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
