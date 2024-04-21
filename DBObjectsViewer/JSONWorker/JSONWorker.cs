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
        public static Dictionary<string, SQLFieldParams> SQLTestData = new Dictionary<string, SQLFieldParams>();
        public static Dictionary<string, dynamic> TableTemplateData = new Dictionary<string, dynamic>();

        public static void LoadJson()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + AppConsts.JSONConsts.TableTemplateFileName))
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
            }
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public static void LoadTestData(/*Dictionary<string, dynamic> argToLoadData, string fileName, string pathToFile = null*/)
        {
            //if (pathToFile != null)
            //{
                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + DirectoryNameOfTestDataFiles))
                {
                    AppCreateDirectory(DirectoryNameOfTestDataFiles);
                }
            //}
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + DirectoryNameOfTestDataFiles + AppConsts.JSONConsts.SQLTestDataFuleName))
            {
                AppCreateFile(AppConsts.JSONConsts.SQLTestDataFuleName, directoryName: DirectoryNameOfTestDataFiles);
                WriteDefaultData(AppConsts.JSONConsts.SQLTestDataFuleName, filePath: DirectoryNameOfTestDataFiles);
            }
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + DirectoryNameOfTestDataFiles + AppConsts.JSONConsts.SQLTestDataFuleName))
            {
                string json = reader.ReadToEnd();
                SQLTestData = JsonSerializer.Deserialize<Dictionary<string, SQLFieldParams>>(json);


                /*Dictionary<string, dynamic> sdf = new Dictionary<string, dynamic>(JSONWorker.TableTemplateData);

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
                }*/
            }
            /*if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + JSONWorker.TableTemplateFileName))
            {
                
            }
            else
            {
                CreateTableTemplate();
                LoadJson();
            }*/
        }

        public static void SaveTableTemplate()
        {
            Dictionary<string, dynamic> categoryValues = new Dictionary<string, dynamic>();

            foreach (string key in TableTemplateData.Keys)
            {
                categoryValues[key] = TableTemplateData[key];
            }

            string json = JsonSerializer.Serialize(TableTemplateData);

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + AppConsts.JSONConsts.TableTemplateFileName, json);
        }
    }
}
