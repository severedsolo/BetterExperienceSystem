using System;
using UnityEngine;

namespace BetterExperienceSystem
{
    public static class Logging
    {
        public static void Log(string messageToLog, LogLevel logLevel)
        {
            string logColor = String.Empty;
            switch (logLevel)
            {
                case LogLevel.Error:
                    logColor = "<color=red>";
                    break;
                case LogLevel.Warning:
                    logColor = "<color=yellow>";
                    break;
            }
            Debug.Log(logColor+"[BetterExperienceSystem]: "+messageToLog);
        }
    }

    public enum LogLevel
    {
        Error,
        Warning,
        Info
    }
}