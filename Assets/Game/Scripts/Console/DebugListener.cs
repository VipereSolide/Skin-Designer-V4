using UnityEngine;

namespace FeatherLight.Pro.Console
{
    public static class DebugListener
    {
        [RuntimeInitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            // removing the callback first makes sure it is only added once
            Application.logMessageReceived -= HandleLog;
            Application.logMessageReceived += HandleLog;
        }

        private static void HandleLog(string logString, string stackTrace, UnityEngine.LogType type)
        {
            Console.instance.FromListener(logString, stackTrace, type);
        }
    }
}