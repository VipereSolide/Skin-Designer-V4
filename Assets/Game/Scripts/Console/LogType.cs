namespace FeatherLight.Pro.Console
{
    public enum LogType
    {
        Log,
        Success,
        Error,
        Warning
    }

    public static class LogTypeHelper
    {
        public static UnityEngine.LogType From(LogType type)
        {
            switch(type)
            {
                case LogType.Log:
                    return UnityEngine.LogType.Log;
                case LogType.Success:
                    return UnityEngine.LogType.Log;
                case LogType.Error:
                    return UnityEngine.LogType.Error;
                case LogType.Warning:
                    return UnityEngine.LogType.Warning;
                default:
                    return UnityEngine.LogType.Log;
            }
        }

        public static LogType To(UnityEngine.LogType type)
        {
            switch(type)
            {
                case UnityEngine.LogType.Log:
                    return LogType.Log;
                case UnityEngine.LogType.Warning:
                    return LogType.Warning;
                case UnityEngine.LogType.Error:
                    return LogType.Error;
                case UnityEngine.LogType.Assert:
                    return LogType.Error;
                case UnityEngine.LogType.Exception:
                    return LogType.Error;
                default:
                    return LogType.Log;
            }
        }
    }
}