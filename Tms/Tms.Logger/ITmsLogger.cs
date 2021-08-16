using System;
using System.Runtime.CompilerServices;

namespace Tms.Logger
{
    public interface ITmsLogger
    {
        string Error(Exception ex, string msg = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0);

        string Debug(string msg = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0);
    }
}
