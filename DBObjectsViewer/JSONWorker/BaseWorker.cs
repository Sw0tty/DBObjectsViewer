using System;
using System.IO;
using DBObjectsViewer;


namespace BaseJsonWorker
{
    abstract class BaseWorker
    {
        /// <summary>
        /// Create directory in app dir. ExtraPath - path between app dir and name directory
        /// </summary>
        public static void AppCreateDirectory(string directoryName, string exraPath = null)
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + exraPath + directoryName);
        }

        /// <summary>
        /// Create file in app dir. directoryName - path between app dir and name of file
        /// </summary>
        public static void AppCreateFile(string fileName, string directoryName = null)
        {
            File.Create(AppDomain.CurrentDomain.BaseDirectory + directoryName + fileName).Dispose();
        }

        /// <summary>
        /// Write file with default data. <para/>
        /// 1. fileNameDefaultData - name file to search default data <br/>
        /// 2*. filePath - path in app dir <br/>
        /// </summary>
        public static void WriteDefaultData(string fileNameDefaultData, string filePath = null)
        {
            using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + filePath + fileNameDefaultData))
            {
                writer.Write(JSONFilesDefaultData.DefaultData[fileNameDefaultData]);
            }
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
    }
}
