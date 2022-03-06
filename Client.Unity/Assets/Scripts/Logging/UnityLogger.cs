using Avace.Backend.Interfaces.Logging;

namespace Logging
{
    public class UnityLogger : ICustomLogger
    {
        public string Name { get; }

        public UnityLogger(string name)
        {
            Name = name;
        }

        public void Debug(string message)
        {
            if (UnityEngine.Debug.isDebugBuild)
            {
                UnityEngine.Debug.Log($"{GetPrefix(true)} {message}");
            }
        }

        private string GetPrefix(bool debug = false)
        {
            return $"{(debug ? "[DEBUG]" : "")}[{Name}]";
        }

        public void Info(string message)
        {
            UnityEngine.Debug.Log($"{GetPrefix()} {message}");
        }

        public void Warn(string message)
        {
            UnityEngine.Debug.LogWarning($"{GetPrefix()} {message}");
        }

        public void Error(string message)
        {
            UnityEngine.Debug.LogError($"{GetPrefix()} {message}");
        }
    }
}