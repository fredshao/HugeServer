using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.IO;

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

        public LogInfo(LogType _logType, string _logStr)
        {
            logType = _logType;
            logStr = _logStr;
        }
    }

    public static bool logConsole = true;
    public static bool logFile = true;
    public static bool rollingLogFile = true;

    public static int rollingMB {
        get {
            return rollingBytes / 1024 / 1024;
        }
        set {
            rollingBytes = value * 1024 * 1024;
        }
    }

    private static int rollingBytes = 0;

    private static ConcurrentQueue<LogInfo> logQueue = new ConcurrentQueue<LogInfo>();
    private static BlockingCollection<LogInfo> logList = new BlockingCollection<LogInfo>(logQueue);

    private static string logRoot;
    private static int rollingLogFileIndex;
    private static int rollingErrorFileIndex;
    private static int rollingWarningFileIndex;

    private static void LoadConfig()
    {
        rollingMB = 1;
    }

    public static void Init()
    {
        logRoot = System.Environment.CurrentDirectory + "/Log/";

        if (!Directory.Exists(logRoot))
        {
            Directory.CreateDirectory(logRoot);
        }

        LoadConfig();

        DetectRollingIndex();

        System.Console.WriteLine(logRoot);

        

        Task.Factory.StartNew(() =>
        {
            foreach(LogInfo info in logList.GetConsumingEnumerable())
            {
                if (logFile)
                {
                    if (info.logType == LogType.Log)
                    {
                        WriteToLogFile(info.logStr);
                    }
                    else if (info.logType == LogType.Error)
                    {
                        WriteToErrorFile(info.logStr);
                    }
                    else if (info.logType == LogType.Warning)
                    {
                        WriteToWarningFile(info.logStr);
                    }
                }

                if (logConsole)
                {
                    LogConsole(info.logStr);
                }
            }
        });
        
    }


    private static void DetectRollingIndex()
    {
        while (true)
        {
            string rollingLogFileName = GetRollingLogName(LogType.Log, rollingLogFileIndex);
            if (IsNeedRollingFile(rollingLogFileName))
            {
                rollingLogFileIndex += 1;
            }
            else
            {
                break;
            }
        }


        while (true)
        {
            string rollingErrorFileName = GetRollingLogName(LogType.Error, rollingErrorFileIndex);
            if (IsNeedRollingFile(rollingErrorFileName))
            {
                rollingErrorFileIndex += 1;
            }
            else
            {
                break;
            }
        }

        while (true)
        {
            string rollingWarningFileName = GetRollingLogName(LogType.Warning, rollingWarningFileIndex);
            if (IsNeedRollingFile(rollingWarningFileName))
            {
                rollingWarningFileIndex += 1;
            }
            else
            {
                break;
            }
        }
    }
   

    public static void Log(string _logStr)
    {
        string str = string.Format("{0} - {1}: {2}\n", GetTime(), "LOG", _logStr);
        LogInfo info = new LogInfo(LogType.Log, str);
        logList.TryAdd(info);
    }

    public static void LogFormat(string _format, params object[] _args)
    {
        string str = string.Format(_format, _args);
        str = string.Format("{0} - {1}: {2}\n", GetTime(), "LOG", str);
        LogInfo info = new LogInfo(LogType.Log, str);
        logList.TryAdd(info);
    }

    public static void LogError(string _logStr)
    {
        string str = string.Format("{0} - {1}: {2}\n", GetTime(), "ERROR", _logStr);
        LogInfo info = new LogInfo(LogType.Error, str);
        logList.TryAdd(info);
    }

    public static void LogErrorFormat(string _format, params object[] _args)
    {
        string str = string.Format(_format, _args);
        str = string.Format("{0} - {1}: {2}\n", GetTime(), "ERROR", str);
        LogInfo info = new LogInfo(LogType.Error, str);
        logList.TryAdd(info);
    }

    public static void LogWarning(string _logStr)
    {
        string str = string.Format("{0} - {1}: {2}\n", GetTime(), "WARNING", _logStr);
        LogInfo info = new LogInfo(LogType.Warning, str);
        logList.TryAdd(info);
    }

    public static void LogWarningFormat(string _format, params object[] _args)
    {
        string str = string.Format(_format, _args);
        str = string.Format("{0} - {1}: {2}\n", GetTime(), "WARNING", str);
        LogInfo info = new LogInfo(LogType.Warning, str);
        logList.TryAdd(info);
    }

    private static void LogConsole(string _logStr)
    {
        System.Console.WriteLine(_logStr);
    }

    private static string GetTime()
    {
        return DateTime.Now.ToString();
    }

    private static string GetBaseLogName(LogType _logType)
    {
        string dtStr = DateTime.Now.ToString("yyyyMMdd");
        if(_logType == LogType.Log)
        {
            return string.Format("{0}{1}_{1}", logRoot, "Log", dtStr);
        }
        else if(_logType == LogType.Error)
        {
            return string.Format("{0}{1}_{1}", logRoot, "Error", dtStr);
        }
        else
        {
            return string.Format("{0}{1}_{1}", logRoot, "Warning", dtStr);
        }
    }

    private static string GetLogName(LogType _logType)
    {
        return string.Format("{0}{1}.{2}", logRoot, GetBaseLogName(_logType), "log");
    }


    private static string GetRollingLogName(LogType _logType, int _index)
    {
        return string.Format("{0}_{1}.{2}", GetBaseLogName(_logType), _index,"log");
    }

    private static string GetRollingLogName(LogType _logType)
    {
        string currFileName;
        if(_logType == LogType.Log)
        {
            currFileName = GetRollingLogName(_logType, rollingLogFileIndex);
        }
        else if(_logType == LogType.Error)
        {
             currFileName = GetRollingLogName(_logType, rollingErrorFileIndex);
        }
        else
        {
             currFileName = GetRollingLogName(_logType, rollingWarningFileIndex);
        }

        if (IsNeedRollingFile(currFileName))
        {
            if (_logType == LogType.Log)
            {
                rollingLogFileIndex += 1;
                currFileName = GetRollingLogName(_logType, rollingLogFileIndex);
            }
            else if (_logType == LogType.Error)
            {
                rollingErrorFileIndex += 1;
                currFileName = GetRollingLogName(_logType, rollingErrorFileIndex);
            }
            else
            {
                rollingWarningFileIndex += 1;
                currFileName = GetRollingLogName(_logType, rollingWarningFileIndex);
            }
        }

        return currFileName;
    }


    private static bool IsNeedRollingFile(string _filePath)
    {
        if (!File.Exists(_filePath))
        {
            return false;
        }

        FileInfo info = new FileInfo(_filePath);
        if(info.Length >= rollingBytes)
        {
            return true;
        }

        return false;
    }

    private static void WriteToLogFile(string _logStr)
    {
        string fileName = string.Empty;
        if (rollingLogFile)
        {
            fileName = GetRollingLogName(LogType.Log);
        }
        else
        {
            fileName = GetLogName(LogType.Log);
        }

        WriteToFile(fileName, _logStr);
    }

    private static void WriteToErrorFile(string _logStr)
    {
        string fileName = string.Empty;
        if (rollingLogFile)
        {
            fileName = GetRollingLogName(LogType.Error);
        }
        else
        {
            fileName = GetLogName(LogType.Error);
        }

        WriteToFile(fileName, _logStr);
    }

    private static void WriteToWarningFile(string _logStr)
    {
        string fileName = string.Empty;
        if (rollingLogFile)
        {
            fileName = GetRollingLogName(LogType.Warning);
        }
        else
        {
            fileName = GetLogName(LogType.Warning);
        }

        WriteToFile(fileName, _logStr);
    }


    private static void WriteToFile(string _filePath, string _logStr)
    {
        File.AppendAllText(_filePath, _logStr);
    }

}
