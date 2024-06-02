using System;
using System.IO;
using DBObjectsViewer;


namespace BaseJsonWorker
{
    abstract class BaseWorker
    {
        /// <summary>
        /// Create file in app dir. directoryName - path between app dir and name of file
        /// </summary>
        public static void AppCreateFile(string fileName, string directoryName = null)
        {
            File.Create(AppDomain.CurrentDomain.BaseDirectory + directoryName + fileName + ".json").Dispose();
        }

        /// <summary>
        /// Write file with default data. <para/>
        /// 1. fileNameDefaultData - name file to search default data <br/>
        /// 2*. filePath - path in app dir <br/>
        /// </summary>
        public static void WriteDefaultData(string fileNameDefaultData, string filePath = null)
        {
            using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + filePath + fileNameDefaultData + ".json"))
            {
                writer.Write(JSONFilesDefaultData.DefaultData[fileNameDefaultData]);
            }
        }
    }
}
