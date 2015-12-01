using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServeKliyent_V2.Utils
{
    public enum LogLevel
    {
        Info = 0, // Green
        Warning = 1, // Yellow
        Error = 2, // Orange
        Critical = 3, // Red
        Debug = 4 // Cyan
    }

    public enum OutputMode
    {
        InfoOnly = 0,
        Essential = 1,
        Verbose = 2,
        Mute = 3
    }

    public enum LoggingMode
    {
        InfoOnly = 0,
        Essential = 1,
        Verbose = 2,
        Mute = 3,
        Null = 4
    }

    public class Logging
    {
        public bool IsLogging = false;

        public OutputMode outputMode;
        public LoggingMode loggingMode;

        public string currentLog = "";
        public Timer logSaver = new Timer(1000);

        public string ReadLine()
        {
            string returnVal = Console.ReadLine();
            currentLog += returnVal + "\n";

            return returnVal;
        }

        public void Write(string message, LogLevel level, bool timeStamp = false)
        {
            DateTime time = DateTime.Now;

            bool allowWrite = checkAllow(level);
            bool allowLog = checkAllow(level, loggingMode);

            if (allowWrite)
            {
                if (timeStamp)
                {
                    Console.Write("[{0:hh:mm:ss}]", time);

                    if (level == LogLevel.Info)
                        Console.ForegroundColor = ConsoleColor.DarkGreen; // Green
                    else if (level == LogLevel.Warning)
                        Console.ForegroundColor = ConsoleColor.DarkYellow; // Yellow
                    else if (level == LogLevel.Error)
                        Console.ForegroundColor = ConsoleColor.DarkRed; // Orange
                    else if (level == LogLevel.Critical)
                        Console.ForegroundColor = ConsoleColor.Red; // Red
                    else if (level == LogLevel.Debug)
                        Console.ForegroundColor = ConsoleColor.Cyan;// Cyan

                    Console.Write(" [" + level.ToString() + "]");
                    Console.ResetColor();

                    Console.Write(" : " + message + "\n");
                }
                else
                    Console.Write(message);
            }

            if (IsLogging && allowLog)
            {
                if (timeStamp)
                    currentLog += "[" + time.ToString("hh:mm:ss") + "] [" + level.ToString() + "] : " + message;
                else
                    currentLog += message;
            }
        }

        public void WriteLine(string message, LogLevel level, bool timeStamp = true, string plugin = "")
        {
            DateTime time = DateTime.Now;

            bool allowWrite = checkAllow(level);
            bool allowLog = checkAllow(level, loggingMode);

            if (allowWrite)
            {
                if (timeStamp)
                {
                    if (plugin != "")
                    {
                        Console.Write("[{0:hh:mm:ss}]", time);

                        if (level == LogLevel.Info)
                            Console.ForegroundColor = ConsoleColor.DarkGreen; // Green
                        else if (level == LogLevel.Warning)
                            Console.ForegroundColor = ConsoleColor.DarkYellow; // Yellow
                        else if (level == LogLevel.Error)
                            Console.ForegroundColor = ConsoleColor.DarkRed; // Orange
                        else if (level == LogLevel.Critical)
                            Console.ForegroundColor = ConsoleColor.Red; // Red
                        else if (level == LogLevel.Debug)
                            Console.ForegroundColor = ConsoleColor.Cyan;// Cyan

                        Console.Write(" [" + level.ToString() + "]");
                        Console.ResetColor();

                        Console.Write(" [" + plugin + "] : " + message + "\n");
                    }
                    else
                    {
                        Console.Write("[{0:hh:mm:ss}]", time);

                        if (level == LogLevel.Info)
                            Console.ForegroundColor = ConsoleColor.DarkGreen; // Green
                        else if (level == LogLevel.Warning)
                            Console.ForegroundColor = ConsoleColor.DarkYellow; // Yellow
                        else if (level == LogLevel.Error)
                            Console.ForegroundColor = ConsoleColor.DarkRed; // Orange
                        else if (level == LogLevel.Critical)
                            Console.ForegroundColor = ConsoleColor.Red; // Red
                        else if (level == LogLevel.Debug)
                            Console.ForegroundColor = ConsoleColor.Cyan;// Cyan

                        Console.Write(" [" + level.ToString() + "]");
                        Console.ResetColor();

                        Console.Write(" : " + message + "\n");
                    }
                }
                else
                    Console.Write(message + "\n");
            }

            if (IsLogging && allowLog)
            {
                if (timeStamp)
                    if (plugin != "")
                    {
                        currentLog += "[" + time.ToString("hh:mm:ss") + "] [" + level.ToString() + "] [" + plugin + "] : " + message + "\n";
                    }
                    else
                        currentLog += "[" + time.ToString("hh:mm:ss") + "] [" + level.ToString() + "] : " + message + "\n";
                else
                    currentLog += message + "\n";
            }
        }

        public bool checkAllow(LogLevel level, LoggingMode logging = LoggingMode.Null)
        {
            bool allowWrite = false;

            if (logging != LoggingMode.Null) // LOGGING CHECK
            {
                if (level == LogLevel.Info)
                {
                    if (loggingMode == LoggingMode.Mute)
                        allowWrite = false;
                    else
                        allowWrite = true;
                }
                else if (level == LogLevel.Warning)
                {
                    if (loggingMode == LoggingMode.InfoOnly)
                        allowWrite = false;
                    else if (loggingMode == LoggingMode.Essential)
                        allowWrite = true;
                    else if (loggingMode == LoggingMode.Verbose)
                        allowWrite = true;
                    else if (loggingMode == LoggingMode.Mute)
                        allowWrite = false;
                }
                else if (level == LogLevel.Error)
                {
                    if (loggingMode == LoggingMode.InfoOnly)
                        allowWrite = false;
                    else if (loggingMode == LoggingMode.Essential)
                        allowWrite = true;
                    else if (loggingMode == LoggingMode.Verbose)
                        allowWrite = true;
                    else if (loggingMode == LoggingMode.Mute)
                        allowWrite = false;
                }
                else if (level == LogLevel.Critical)
                {
                    if (loggingMode == LoggingMode.InfoOnly)
                        allowWrite = false;
                    else if (loggingMode == LoggingMode.Essential)
                        allowWrite = true;
                    else if (loggingMode == LoggingMode.Verbose)
                        allowWrite = true;
                    else if (loggingMode == LoggingMode.Mute)
                        allowWrite = false;
                }
                else if (level == LogLevel.Debug)
                {
                    if (loggingMode == LoggingMode.InfoOnly)
                        allowWrite = false;
                    else if (loggingMode == LoggingMode.Essential)
                        allowWrite = false;
                    else if (loggingMode == LoggingMode.Verbose)
                        allowWrite = true;
                    else if (loggingMode == LoggingMode.Mute)
                        allowWrite = false;
                }
            }
            else //NORMAL CHECK
            {
                if (level == LogLevel.Info)
                {
                    if (outputMode == OutputMode.Mute)
                        allowWrite = false;
                    else
                        allowWrite = true;
                }
                else if (level == LogLevel.Warning)
                {
                    if (outputMode == OutputMode.InfoOnly)
                        allowWrite = false;
                    else if (outputMode == OutputMode.Essential)
                        allowWrite = true;
                    else if (outputMode == OutputMode.Verbose)
                        allowWrite = true;
                    else if (outputMode == OutputMode.Mute)
                        allowWrite = false;
                }
                else if (level == LogLevel.Error)
                {
                    if (outputMode == OutputMode.InfoOnly)
                        allowWrite = false;
                    else if (outputMode == OutputMode.Essential)
                        allowWrite = true;
                    else if (outputMode == OutputMode.Verbose)
                        allowWrite = true;
                    else if (outputMode == OutputMode.Mute)
                        allowWrite = false;
                }
                else if (level == LogLevel.Critical)
                {
                    if (outputMode == OutputMode.InfoOnly)
                        allowWrite = false;
                    else if (outputMode == OutputMode.Essential)
                        allowWrite = true;
                    else if (outputMode == OutputMode.Verbose)
                        allowWrite = true;
                    else if (outputMode == OutputMode.Mute)
                        allowWrite = false;
                }
                else if (level == LogLevel.Debug)
                {
                    if (outputMode == OutputMode.InfoOnly)
                        allowWrite = false;
                    else if (outputMode == OutputMode.Essential)
                        allowWrite = false;
                    else if (outputMode == OutputMode.Verbose)
                        allowWrite = true;
                    else if (outputMode == OutputMode.Mute)
                        allowWrite = false;
                }
            }

            return allowWrite;
        }

        public void BeginLog()
        {
            IsLogging = true;
            logSaver.Elapsed += LogSaver_Elapsed;
            currentLog = "\nBEGIN LOG - " + DateTime.Now.ToString("hh:mm:ss dd-MM-yyyy") + "\n################################################################################################\n\n";

            logSaver.Start();
        }

        public void StopLog(bool restart = false)
        {
            if (IsLogging)
            {
                currentLog += "\n################################################################################################\nEND OF LOG - " + DateTime.Now.ToString("hh:mm:ss dd-MM-yyyy");

                System.Threading.Thread.Sleep(1000);
                logSaver.Stop();

                currentLog = "";
                IsLogging = false;
                logSaver = new Timer();

                if (restart)
                {
                    BeginLog();
                    WriteLine("Logging has been restarted.", LogLevel.Info);
                }
                else
                    WriteLine("Logging is disabled.", LogLevel.Info);

            }
            else
            {
                WriteLine("Cannot dispose the log because you are not logging at the moment!", LogLevel.Warning);
            }
        }

        private void LogSaver_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Save the log string to a file.
            try
            {
                File.WriteAllText(Environment.CurrentDirectory + "/log " + DateTime.Now.ToString("dd-MM-yyyy") + ".log", currentLog);
            }
            catch (Exception ex)
            {
                WriteLine("Failed to save log on default location!... using fallback name.", LogLevel.Error);

                try
                {
                    File.WriteAllText(Environment.CurrentDirectory + "/log " + DateTime.Now.ToString("dd-MM-yyyy") + "-fallback.log", currentLog);
                }
                catch (Exception exx)
                { WriteLine("Failed to save log on fallback location! Log has not been saved!", LogLevel.Error); }
            }
        }
    }
}
