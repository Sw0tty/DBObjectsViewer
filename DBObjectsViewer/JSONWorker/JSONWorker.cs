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
        public static string DirectoryNameOfTestDataFiles = @"TestData\";
        public static Dictionary<string, List<string>> DataFromFile = new Dictionary<string, List<string>>();
        public static Dictionary<string, Deserializers.TestTableFields> SQLTestData = new Dictionary<string, Deserializers.TestTableFields>();
        public static Deserializers.TableTemplate TableTemplateData = new Deserializers.TableTemplate();

        public static void LoadJson()
        {
            /*if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + AppConsts.JSONConsts.TableTemplateFileName))
            {
                using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + AppConsts.JSONConsts.TableTemplateFileName))
                {
                    string json = reader.ReadToEnd();
                    if (json != "")
                    {
                        TableTemplateData = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(json);

                        Dictionary<string, dynamic> sdf = new Dictionary<string, dynamic>(JSONWorker.TableTemplateData);

                        foreach (string key in sdf.Keys)
                        {
                            
                            switch (TableTemplateData[key].ValueKind.ToString())
                            {
                                case "True":
                                case "False":
                                    TableTemplateData[key] = JsonSerializer.Deserialize<bool>(TableTemplateData[key].GetRawText());
                                    break;
                                case "Object":
                                    TableTemplateData[key] = JsonSerializer.Deserialize<Dictionary<string, string>>(TableTemplateData[key].GetRawText());
                                    break;
                                case "Array":
                                    TableTemplateData[key] = JsonSerializer.Deserialize<List<string>>(TableTemplateData[key].GetRawText());
                                    break;
                                default:
                                    MessageBox.Show($"Undefined type: '{TableTemplateData[key].ValueKind.ToString()}'");
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                AppCreateFile(AppConsts.JSONConsts.TableTemplateFileName);
                WriteDefaultData(AppConsts.JSONConsts.TableTemplateFileName);
                //CreateTableTemplate();
                LoadJson();
            }*/
        }

        static string ReadJson(string fileName, string pathToFile = null)
        {
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName))
                return reader.ReadToEnd();
        }

        public static void LoadTestData(string fileName, string pathToFile = null/*string nameOfArgToDeserialize, string fileName, string pathToFile = null*/)
        {
/*            if (pathToFile != null)
            {
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + DirectoryNameOfTestDataFiles))
                {
                    AppCreateDirectory(DirectoryNameOfTestDataFiles);
                }
            }*/
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + fileName + fileName))
            {
                AppCreateFile(fileName, directoryName: DirectoryNameOfTestDataFiles);
                WriteDefaultData(fileName, filePath: DirectoryNameOfTestDataFiles);
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
            }

            //SQLTestData = JsonSerializer.Deserialize<Dictionary<string, Deserializers.TestTableFields>>(json_NEW);

            /*using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + DirectoryNameOfTestDataFiles + AppConsts.JSONConsts.SQLTestDataFileName))
            {
                string json = reader.ReadToEnd();
                SQLTestData = JsonSerializer.Deserialize<Dictionary<string, Deserializers.TestTableFields>>(json);


                *//*Dictionary<string, dynamic> sdf = new Dictionary<string, dynamic>(JSONWorker.TableTemplateData);

                foreach (string key in sdf.Keys)
                {

                    switch (TableTemplateData[key].ValueKind.ToString())
                    {
                        case "True":
                        case "False":
                            TableTemplateData[key] = JsonSerializer.Deserialize<bool>(TableTemplateData[key].GetRawText());
                            break;
                        case "Object":
                            TableTemplateData[key] = JsonSerializer.Deserialize<Dictionary<string, string>>(TableTemplateData[key].GetRawText());
                            break;
                        case "Array":
                            TableTemplateData[key] = JsonSerializer.Deserialize<List<string>>(TableTemplateData[key].GetRawText());
                            break;
                        default:
                            MessageBox.Show($"Undefined type: '{TableTemplateData[key].ValueKind.ToString()}'");
                            break;
                    }
                }*//*
            }*/
            /*if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + JSONWorker.TableTemplateFileName))
            {
                
            }
            else
            {
                CreateTableTemplate();
                LoadJson();
            }*/
        }

        /*public static void SaveTableTemplate()
        {
            Dictionary<string, dynamic> categoryValues = new Dictionary<string, dynamic>();

            foreach (string key in TableTemplateData.Keys)
            {
                categoryValues[key] = TableTemplateData[key];
            }

            string json = JsonSerializer.Serialize(TableTemplateData);

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + AppConsts.JSONConsts.TableTemplateFileName, json);
        }*/
    }
}
