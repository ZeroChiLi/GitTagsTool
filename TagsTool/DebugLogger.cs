using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsTool
{
    internal class DebugLogger
    {
        public const string PROPS_FOLDER_PATH = @"%userprofile%\appdata\local\TagsTool";
        private static string _logFilePath;
        private static readonly object _logLock = new object();
        public static string LogFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_logFilePath))
                    _logFilePath = $"{Environment.ExpandEnvironmentVariables(PROPS_FOLDER_PATH)}\\~tags_tool_log.txt";
                return _logFilePath;
            }
        }

        public static Action<Exception> ExceptionAction { get; set; }

        private static void InitLog()
        {
            if (!File.Exists(LogFilePath))
            {
                File.Create(LogFilePath).Close();
            }
        }

        public static void Log(string msg)
        {
            lock (_logLock)
            {
                _Log(msg);
            }
        }

        public static void Error(string msg)
        {
            _Log($"[Error]{msg}");
        }

        private static void _Log(string msg)
        {
            try
            {
                InitLog();
                using (StreamWriter w = File.AppendText(LogFilePath))
                {
                    Write(msg, w);
                }
            }
            catch (Exception e)
            {
                ExceptionAction?.Invoke(e);
            }
        }

        static private void Write(string msg, TextWriter w)
        {
            try
            {
                w.WriteLine($"[{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}] {msg}");
            }
            catch (Exception e)
            {
                ExceptionAction?.Invoke(e);
            }
        }
    }
}
