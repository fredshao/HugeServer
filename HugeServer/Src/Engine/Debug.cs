using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public static class Debug
{
    enum LogType
    {
        Log,
        Error,
        Warning,
    }

    struct LogInfo
    {
        public LogType logType;
        public string logStr;
    }

    public static bool logConsole = true;
    public static bool logFile = true;
    public static bool rolingLogFile = true;

    private static ConcurrentQueue<LogInfo> logQueue = new ConcurrentQueue<LogInfo>();
    private static BlockingCollection<LogInfo> logList = new BlockingCollection<LogInfo>(logQueue);


    public static void Init()
    {
        LoadConfig();

        Task.Factory.StartNew(() =>
        {
            foreach(LogInfo info in logList.GetConsumingEnumerable())
            {

            }
        });
        
    }

   

    private static void LoadConfig()
    {

    }

    public static void Log(string _logStr)
    {
        string str = string.Format("{0} - {1}: {2}", GetTime(), "LOG", _logStr);
    }

    public static void LogFormat(string _format, params object[] _args)
    {
        string str = string.Format(_format, _args);
        str = string.Format("{0} - {1}: {2}", GetTime(), "LOG", str);

    }

    public static void LogError(string _logStr)
    {
        string str = string.Format("{0} - {1}: {2}", GetTime(), "ERROR", _logStr);

    }

    public static void LogErrorFormat(string _format, params object[] _args)
    {
        string str = string.Format(_format, _args);
        str = string.Format("{0} - {1}: {2}", GetTime(), "ERROR", str);

    }

    public static void LogWarning(string _logStr)
    {

    }

    public static void LogWarningFormat(string _format, params object[] _args)
    {
        string str = string.Format(_format, _args);

    }

    private static void LogConsole(string _logStr)
    {
        System.Console.WriteLine(_logStr);
    }

    private static string GetTime()
    {
        return DateTime.Now;
    }


}
