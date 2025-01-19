using System;
using System.Windows.Forms;

namespace StarCitizenLog.LogProcessing
{
    public class LogFinder
    {
        public static string SaveFilePath = "Path.txt";
        public static string GetLogDirectory()
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Sélectionnez le dossier où se trouvent les logs.";

                DialogResult result = folderDialog.ShowDialog();

                // Si l'utilisateur sélectionne un dossier
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    return folderDialog.SelectedPath;
                }
                else
                {
                    Console.WriteLine("Aucun dossier sélectionné.");
                    return string.Empty;
                }
            }
        }
        
        public static void SaveLogDirectory(string path)
        {
            try
            {
                
                File.WriteAllText(SaveFilePath, path);
                Console.WriteLine("Le chemin a été sauvegardé avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la sauvegarde du chemin : " + ex.Message);
            }
        }
        
        public static string LoadLogDirectory()
        {
            try
            {
                if (File.Exists(SaveFilePath))
                {
                    return File.ReadAllText(SaveFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors du chargement du chemin : " + ex.Message);
            }
            return string.Empty;
        }

        public static bool IsPathSave()
        {
            if (File.Exists(SaveFilePath))
            {
                FileInfo info = new FileInfo(SaveFilePath);
                return info.Length != 0;
            }
            return false;
        }
    }
    
    
}