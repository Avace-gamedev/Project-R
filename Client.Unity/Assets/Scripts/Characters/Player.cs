using System.Reflection;
using Avace.Backend.Interfaces.Logging;
using Avace.Backend.Kernel.Injection;
using UnityEngine;
using World;
using ILogger = Avace.Backend.Interfaces.Logging.ILogger;

namespace Characters
{
    public class Player : MonoBehaviour
    {
        private static readonly ILogger _log = _log = Injector.Get<ILoggerProvider>().GetLogger(MethodBase.GetCurrentMethod().Name);
        
        private void Start()
        {
            Injector.BindSingleton(this);

            Vector2Int cell = Injector.Get<IPlayerStartPositionProvider>().PlayerStartPosition;
            transform.position = Injector.Get<ICoordinatesConverter>().Convert(cell);
        }
    }
}
