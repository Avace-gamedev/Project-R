using System.Reflection;
using Avace.Backend.Interfaces.Logging;
using Avace.Backend.Kernel.Injection;
using UnityEngine;
using ILogger = Avace.Backend.Interfaces.Logging.ILogger;

namespace Characters
{
    public class Player : MonoBehaviour
    {
        private static ILogger _log;
        
        private void Start()
        {
            _log = Injector.Get<ILoggerProvider>().GetLogger(MethodBase.GetCurrentMethod().Name);
            Injector.BindSingleton(this);
        }
    }
}
