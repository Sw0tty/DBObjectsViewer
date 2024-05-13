using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;
using System.IO;
using BaseJsonWorker;
using Microsoft.Office.Interop.Word;


namespace DBObjectsViewer
{
    class JSONWorker : BaseWorker
    {
        public static Dictionary<string, List<string>> DataFromFile = new Dictionary<string, List<string>>();
        public static Dictionary<string, Deserializers.TestTableFields> SQLTestData = new Dictionary<string, Deserializers.TestTableFields>();
        public static Dictionary<string, Deserializers.TestIndexes> SQLTestIndexes = new Dictionary<string, Deserializers.TestIndexes>();
        public static Dictionary<string, Deserializers.TestForeigns> SQLTestForeigns = new Dictionary<string, Deserializers.TestForeigns>();
        public static Deserializers.TableTemplate TableTemplateData = new Deserializers.TableTemplate();
        public static Dictionary<string, Deserializers.DataBaseInfo> MYSQLDatabaseInfo = new Dictionary<string, Deserializers.DataBaseInfo>();

        static string ReadJson(string fileName, string pathToFile = null)
        {
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName))
                return reader.ReadToEnd();
        }

        public static void LoadJson(string fileName, string pathToFile = null/*string nameOfArgToDeserialize, string fileName, string pathToFile = null*/)
        {
            if (pathToFile != null)
            {           
                string[] dirsToFile = pathToFile.Split('\\');
                string dirsChain = "";
                
                foreach (string dir in dirsToFile)
                {
                    dirsChain += $@"\{dir}";
                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + dirsChain))
                    {
                        AppCreateDirectory(dirsChain);
                    }
                }      
            }
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName))
            {
                AppCreateFile(fileName, directoryName: pathToFile);
                WriteDefaultData(fileName, filePath: pathToFile);
            }
            string json = ReadJson(fileName, pathToFile: pathToFile);

            if (pathToFile != null && pathToFile.Contains(AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles))
            {
                if (fileName.Contains(AppConsts.DatabaseType.MYSQL))
                    MYSQLDatabaseInfo = JsonSerializer.Deserialize<Dictionary<string, Deserializers.DataBaseInfo>>(json);
                /*else if (fileName.Contains(AppConsts.DatabaseType.PostgreSQL))
                    PostgreSQLDatabaseInfo = JsonSerializer.Deserialize<Dictionary<string, Deserializers.DataBaseInfo>>(json);*/
            }
            else
            {
                switch (fileName)
                {
                    case AppConsts.FileNamesConsts.TableTemplateFileName:
                        TableTemplateData = JsonSerializer.Deserialize<Deserializers.TableTemplate>(json);
                        break;
                    case AppConsts.FileNamesConsts.SQLTestDataFileName:
                        SQLTestData = JsonSerializer.Deserialize<Dictionary<string, Deserializers.TestTableFields>>(json);
                        break;
                    case AppConsts.FileNamesConsts.SQLTestForeignsFileName:
                        SQLTestForeigns = JsonSerializer.Deserialize<Dictionary<string, Deserializers.TestForeigns>>(json);
                        break;
                    case AppConsts.FileNamesConsts.SQLTestIndexesFileName:
                        SQLTestIndexes = JsonSerializer.Deserialize<Dictionary<string, Deserializers.TestIndexes>>(json);
                        break;
                }
            }
        }

        public static void SaveJson(object data, string fileName, string pathToFile = null)
        {
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName, json);
        }

        public static string MakeUniqueFileName(string databaseName, string databaseType)
        {
            return $"{databaseName}_{databaseType}_" + DateTime.Now.ToString("MMddyyHHmmss") + ".json";
        }
    }
}
