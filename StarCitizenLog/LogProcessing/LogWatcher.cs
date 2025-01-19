using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace StarCitizenLog.LogProcessing
{
    public class LogWatcher
    {
        public static string logFilePath = "";
        private static long lastFileSize = 0;
        private static Thread watcherThread;

        public static void StartWatching()
        {
            watcherThread = new Thread(CheckFileSize)
            {
                IsBackground = true
            };
            watcherThread.Start();
        }

        private static void CheckFileSize()
        {
            try
            {
                lastFileSize = new FileInfo(logFilePath).Length;
                while (true)
                {
                    long currentFileSize = new FileInfo(logFilePath).Length;
                    
                    if (currentFileSize > lastFileSize)
                    {
                        ReadNewLines(lastFileSize, currentFileSize);
                        lastFileSize = currentFileSize;
                    }
                    
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur dans le watcher : " + ex.Message);
            }
        }

        private static void ReadNewLines(long start, long end)
        {
            try
            {
                using (FileStream fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                {
                    // Se positionner à la position de départ (start) pour ne lire que les nouvelles lignes
                    fs.Seek(start, SeekOrigin.Begin);

                    // Lire chaque nouvelle ligne ajoutée au fichier
                    while (fs.Position < end)
                    {
                        string newLine = reader.ReadLine();
                        if (newLine != null)
                        {
                            // Traitez la nouvelle ligne ici
                            List<string> lines = LogParser.ParseLog(newLine);

                            // Simplifiez ou affichez les logs via une méthode dédiée
                            LogParser.SimplifyLogs(lines);

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la lecture du fichier : " + ex.Message);
            }
        }
    }
}
