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
                            try
                            {
                                TableTemplateData[key] = JsonSerializer.Deserialize<bool>(TableTemplateData[key].GetRawText());
                            }
                            catch
                            {
                                TableTemplateData[key] = JsonSerializer.Deserialize<Dictionary<string, string>>(TableTemplateData[key].GetRawText());
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
