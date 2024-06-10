using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;


namespace DBObjectsViewer
{
    internal class FilesManager
    {
        /// <summary>
        /// Create directory in app dir. ExtraPath - path between app dir and name directory
        /// </summary>
        public static void AppCreateDirectory(string directoryName, string exraPath = null)
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + exraPath + directoryName);
        }

        /// <summary>
        /// Checks on existing path and create if False
        /// </summary>
        public static void CheckPath(string pathToFile)
        {
            string[] dirsToFile = pathToFile.Split('\\');
            string dirsChain = "";

            foreach (string dir in dirsToFile)
            {
                dirsChain += $@"\{dir}";
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + dirsChain))
                    AppCreateDirectory(dirsChain);
            }
        }

        public static string ReturnSupportedFormats(Dictionary<string, string> supportedFormats)
        {
            string formats = null;
            foreach (string key in supportedFormats.Keys)
                formats += $"{supportedFormats[key]} (*.{key})|*.{key}|";
            return formats;
        }

        public static string MakeUniqueFileName(string firstPart, string secondPart)
        {
            return $"{firstPart}_{secondPart}_" + DateTime.Now.ToString("MMddyyHHmmss");
        }

        public static string SelectFileOnPC(string startDir, string dialogTitle, Dictionary<string, string> supportedFormats = null)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = startDir;
                openFileDialog.Title = dialogTitle;
                openFileDialog.Filter = $"{(supportedFormats != null && supportedFormats.Count > 0 ? ReturnSupportedFormats(supportedFormats) : "")}All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] filePathParts = openFileDialog.FileName.Split('.');
                    if (!supportedFormats.Keys.Contains(filePathParts[filePathParts.Length - 1]))
                        return AppConsts.FileDialogSupportedFormats.UnsupportFormatStatus;
                    else
                        return openFileDialog.FileName;
                }
                return null;
            }
        }
    }
}
