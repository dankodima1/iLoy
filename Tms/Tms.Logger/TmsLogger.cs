using System;
using System.IO;
using System.Runtime.CompilerServices;

using NLog;

namespace Tms.Logger
{
    public class TmsLogger : ITmsLogger
    {
        private readonly NLog.Logger Logger;

        public TmsLogger()
        {
            Logger = LogManager.GetCurrentClassLogger();

            string configFile = string.Concat(Directory.GetCurrentDirectory(), "/nlog.config");
            LogManager.LoadConfiguration(configFile);
        }

        public string Error(Exception ex, string msg = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            // class
            callerFilePath = callerFilePath.GetClassName();

            // method
            callerMemberName = callerMemberName.Replace(".ctor", callerFilePath);

            // base
            string info = $"[{callerFilePath}.{callerMemberName}.{callerLineNumber}] {Logger.GetUserInfo()}";
            if (ex == null)
            {
                if (string.IsNullOrEmpty(msg))
                    Logger.Error(info);
                else
                    Logger.Error($"{info} {msg}");
            }
            else
            {
                if (string.IsNullOrEmpty(msg))
                    msg = ex.Message;

                if (string.IsNullOrEmpty(msg))
                    Logger.Error(ex, info);
                else
                    Logger.Error(ex, $"{info} {msg}");
            }
            return info;
        }

        public string Debug(string msg = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            // class
            callerFilePath = callerFilePath.GetClassName();

            // method
            callerMemberName = callerMemberName.Replace(".ctor", callerFilePath);

            // base
            string info = $"[{callerFilePath}.{callerMemberName}.{callerLineNumber}] {Logger.GetUserInfo()}";
            if (string.IsNullOrEmpty(msg))
                Logger.Debug(info);
            else
                Logger.Debug($"{info} {msg}");
            return info;
        }
    }
}
