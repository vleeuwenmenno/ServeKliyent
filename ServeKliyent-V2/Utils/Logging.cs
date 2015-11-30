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
        Info = 0,
        Warning = 1,
        Error = 2,
        Critical = 3,
        Debug = 4
    }

    public enum OutputMode
    {
        InfoOnly = 0,
        Essential = 1,
        Verbose = 2,
        Mute = 3
    }

    public class Logging
    {
        public bool IsLogging = false;
        public OutputMode outputMode;
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
            bool allowWrite = false;

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

            if (allowWrite)
            {
                if (timeStamp)
                    Console.Write("[{0:hh:mm:ss}] [" + level.ToString() + "] : " + message, time);
                else
                    Console.Write(message);
            }

            if (IsLogging)
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
            bool allowWrite = false;

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

            if (allowWrite)
            {
                if (timeStamp)
                    if (plugin != "")
                    {
                        Console.Write("[{0:hh:mm:ss}] [" + level.ToString() + "] ["  + plugin + "] : " + message + "\n", time);
                    }
                    else
                        Console.Write("[{0:hh:mm:ss}] [" + level.ToString() + "] : " + message + "\n", time);
                else
                    Console.Write(message + "\n");
            }

            if (IsLogging)
            {
                if (timeStamp)
                    if (plugin != "")
                    {
                        currentLog += "[" + time.ToString("hh:mm:ss") + "] [" + level.ToString() + "] [" + plugin + "] : " + message + "\n";
                    }
                    else
                        currentLog += "[" + time.ToString("hh:mm:ss") + "] [" + level.ToString() + "] [" + plugin + "] : " + message + "\n";
                else
                    currentLog += message + "\n";
            }
        }

        public void BeginLog()
        {
            IsLogging = true;
            logSaver.Elapsed += LogSaver_Elapsed;
            currentLog = "\nBEGIN LOG - " + DateTime.Now.ToString("hh:mm:ss dd-MM-yyyy") + "\n################################################################################################\n\n";

            logSaver.Start();
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
    }
}
