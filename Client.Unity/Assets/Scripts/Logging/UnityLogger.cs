using Avace.Backend.Interfaces.Logging;

namespace Logging
{
    public class UnityLogger : ILogger 
    {
        public UnityLogger(string name)
        {
        }

        public void Debug(string message)
        {
            if (UnityEngine.Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log($"DEBUG: {message}");
            }
        }

        public void Info(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        public void Warn(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public void Error(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}