using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using StarCitizenLog.LogProcessing;

namespace StarCitizenLog
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            AskLogPath();
            
            LogWatcher.StartWatching();
            StartOverlay();
            
            Console.WriteLine("Appuyez sur une touche pour arrêter...");
            Console.ReadLine();

            // Lancer l'application principale
            //Application.Run(new MainForm());
        }
        
        public static void AskLogPath()
        {
            string logDirectoryPath = "";
            if (LogFinder.IsPathSave())
            {
                logDirectoryPath = LogFinder.LoadLogDirectory();
            }
            else
            {
                logDirectoryPath = LogFinder.GetLogDirectory();
                if (logDirectoryPath != string.Empty)
                {
                    logDirectoryPath +="\\Game.log";
                    LogFinder.SaveLogDirectory(logDirectoryPath);

                }
                else
                {
                    return;
                }
            }
            
            LogWatcher.logFilePath = logDirectoryPath;
            
        }

        public static void StartOverlay()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OverlayForm());
        }
    }
}