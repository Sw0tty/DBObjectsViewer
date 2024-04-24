using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BaseJsonWorker;
using Microsoft.Office.Interop.Word;


namespace DBObjectsViewer
{
    public class SQLFieldParams
    {
        public string AtributeName { get; set; }
        public string DataType { get; set; }
        public int MaxLength { get; set; }
        public bool Required { get; set; }
    }

    class JSONWorker : BaseWorker
    {
        //public static string DirectoryNameOfTestDataFiles = @"TestData\";
        public static Dictionary<string, List<string>> DataFromFile = new Dictionary<string, List<string>>();
        public static Dictionary<string, Deserializers.TestTableFields> SQLTestData = new Dictionary<string, Deserializers.TestTableFields>();
        public static Dictionary<string, Deserializers.TestIndexes> SQLTestIndexes = new Dictionary<string, Deserializers.TestIndexes>();
        public static Dictionary<string, Deserializers.TestForeigns> SQLTestForeigns = new Dictionary<string, Deserializers.TestForeigns>();
        public static Deserializers.TableTemplate TableTemplateData = new Deserializers.TableTemplate();

        static string ReadJson(string fileName, string pathToFile = null)
        {
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName))
                return reader.ReadToEnd();
        }

        public static void LoadJson(string fileName, string pathToFile = null/*string nameOfArgToDeserialize, string fileName, string pathToFile = null*/)
        {
/*            if (pathToFile != null)
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + DirectoryNameOfTestDataFiles))
                {
                    AppCreateDirectory(DirectoryNameOfTestDataFiles);
                }
            }*/
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName))
            {
                AppCreateFile(fileName, directoryName: AppConsts.JSONConsts.DirectoryOfTestDataFiles);
                WriteDefaultData(fileName, filePath: AppConsts.JSONConsts.DirectoryOfTestDataFiles);
            }
            string json = ReadJson(fileName, pathToFile: pathToFile);

            switch (fileName)
            {
                case AppConsts.JSONConsts.TableTemplateFileName:
                    TableTemplateData = JsonSerializer.Deserialize<Deserializers.TableTemplate>(json);
                    break;
                case AppConsts.JSONConsts.SQLTestDataFileName:
                    SQLTestData = JsonSerializer.Deserialize<Dictionary<string, Deserializers.TestTableFields>>(json);
                    break;
                case AppConsts.JSONConsts.SQLTestForeignsFileName:
                    SQLTestForeigns = JsonSerializer.Deserialize<Dictionary<string, Deserializers.TestForeigns>>(json);
                    break;
                case AppConsts.JSONConsts.SQLTestIndexesFileName:
                    SQLTestIndexes = JsonSerializer.Deserialize<Dictionary<string, Deserializers.TestIndexes>>(json);
                    break;
            }
        }

        public static void SaveJson(object data, string fileName, string pathToFile = null)
        {
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName, json);
        }
    }
}
