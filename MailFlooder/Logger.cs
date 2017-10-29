using System;
using System.IO;

namespace MailFlooder
{
    internal static class Logger
    {
        public static Stream Logs { get; } = openLogFile();

        private static Stream openLogFile()
        {
            return new FileStream($".//{DateTime.Now.ToString().Replace(' ','_')}.log", FileMode.OpenOrCreate);
        }

        internal static void WriteLine(string v)
        {
            using (StreamWriter sw = new StreamWriter(Logs))
            {
                sw.WriteLineAsync(v).GetAwaiter();
            }
        }
    }
}