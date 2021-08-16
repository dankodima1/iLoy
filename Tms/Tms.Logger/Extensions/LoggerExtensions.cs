using System.IO;
using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Http;

namespace Tms.Logger
{
    public static class LoggerExtensions
    {
        public static string GetClassName(this string callerFilePath)
        {
            try { callerFilePath = Path.GetFileNameWithoutExtension(callerFilePath); } catch { }
            return callerFilePath;
        }

        /// <summary>
        /// get base user info - user id and external email
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static string GetUserInfo(this NLog.Logger logger)
        {
            //string userId = "";
            //string userEmail = "";
            //try
            //{
            //}
            //catch { }
            //return $"userId=({userId}) userEmail=({userEmail})";
            return "";
        }

        public static void LogRequest(this ITmsLogger logger, HttpRequest request,
           [CallerFilePath] string callerFilePath = "",
           [CallerMemberName] string callerMemberName = "",
           [CallerLineNumber] int callerLineNumber = 0)
        {
            try
            {
                string body = "";

                if (request?.ContentLength > 0)
                    body = $" request.ContentLength = ({request.ContentLength}) ";

                logger.Debug($"api: ({request.Method} {request.Path}{request.QueryString}){body}", callerFilePath, callerMemberName, callerLineNumber);
            }
            catch { }
        }

    }
}
