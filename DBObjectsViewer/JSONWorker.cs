using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace RandomGame
{
    public static class JSONWorker
    {
        public static string TableTemplateFileName = "table_template.json";
        public static Dictionary<string, List<string>> DataFromFile = new Dictionary<string, List<string>>();
        public static Dictionary<string, dynamic> TableTemplateData = new Dictionary<string, dynamic>();

        public static void LoadJson()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + JSONWorker.TableTemplateFileName))
            {
                using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + JSONWorker.TableTemplateFileName))
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
                CreateTableTemplate();
                LoadJson();
            }
        }

        public static void CreateTableTemplate()
        {
            File.Create(AppDomain.CurrentDomain.BaseDirectory + TableTemplateFileName).Close();
            WriteDefaultParameters();
        }

        static void WriteDefaultParameters()
        {
            using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + JSONWorker.TableTemplateFileName))
            {
                writer.Write("{\r\n    \"ADD_INDEXES_INFO\": false,\r\n    \"ADD_FOREIGN_KEYS_INFO\": false,\r\n    \"ALL_ABOUT_DATA_TYPE\": false,\r\n    \"NOT_SELECTED_COLUMNS\": {\"name\": \"Наименование атрибута\", \"data_type\": \"Тип данных\", \"required\": \"Обязательное поле\"},\r\n    \"SELECTED_COLUMNS\": {}\r\n}");
            }
        }

/*        {
    "ADD_INDEXES_INFO": false,
    "ADD_FOREIGN_KEYS_INFO": false,
    "ALL_ABOUT_DATA_TYPE": true,
    "NOT_SELECTED_COLUMNS": {
        "data_type": "\u0422\u0438\u043F \u0434\u0430\u043D\u043D\u044B\u0445"
    },
    "SELECTED_COLUMNS": {
        "name": "\u041D\u0430\u0438\u043C\u0435\u043D\u043E\u0432\u0430\u043D\u0438\u0435 \u0430\u0442\u0440\u0438\u0431\u0443\u0442\u0430",
        "required": "\u041E\u0431\u044F\u0437\u0430\u0442\u0435\u043B\u044C\u043D\u043E\u0435 \u043F\u043E\u043B\u0435"
    },
    "TABLE_TITLE": ["dsf", "", "", "sdd"]
    }*/
    public static void SaveTableTemplate()
        {
            Dictionary<string, dynamic> categoryValues = new Dictionary<string, dynamic>();

            foreach (string key in TableTemplateData.Keys)
            {
                categoryValues[key] = TableTemplateData[key];
            }

            string json = JsonSerializer.Serialize(TableTemplateData);

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + TableTemplateFileName, json);
        }
    }
}
