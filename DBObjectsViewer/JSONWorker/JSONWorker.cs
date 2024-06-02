﻿using System;
using System.IO;
using BaseJsonWorker;
using System.Text.Json;
using System.Collections.Generic;
using System.Windows.Forms;


namespace DBObjectsViewer
{
    class JSONWorker : BaseWorker
    {
        public static Dictionary<string, List<string>> DataFromFile = new Dictionary<string, List<string>>();
        public static Dictionary<string, Deserializers.TestTableFields> SQLTestData = new Dictionary<string, Deserializers.TestTableFields>();
        public static Dictionary<string, Deserializers.TestIndexes> SQLTestIndexes = new Dictionary<string, Deserializers.TestIndexes>();
        public static Dictionary<string, Deserializers.TestForeigns> SQLTestForeigns = new Dictionary<string, Deserializers.TestForeigns>();
        public static Deserializers.ScannerSettings AppSettings = new Deserializers.ScannerSettings();
        //public static Dictionary<string, Deserializers.DataBaseInfo> MySQLDatabaseInfo = new Dictionary<string, Deserializers.DataBaseInfo>();
        //public static Dictionary<string, Deserializers.DataBaseInfo> PostgreSQLDatabaseInfo = new Dictionary<string, Deserializers.DataBaseInfo>();

        static string ReadJson(string fileName, string pathToFile = null, bool defAppPath = true)
        {
            if (defAppPath)
                using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName + ".json"))
                    return reader.ReadToEnd();
            using (StreamReader reader = new StreamReader(pathToFile + fileName + ".json"))
                return reader.ReadToEnd();
        }

        public static dynamic LoadAndReturnJSON(string fileName, string pathToFile = null, bool defAppPath = true)
        {
            string json = ReadJson(fileName, pathToFile: pathToFile, defAppPath: defAppPath);
            return JsonSerializer.Deserialize<Dictionary<string, Deserializers.DataBaseInfo>>(json);
        }

        public static void LoadJson(string fileName, string pathToFile = null, bool defAppPath = true)
        {
            if (defAppPath)
            {
                if (pathToFile != null)
                {
                    FilesManager.CheckPath(pathToFile);
                }
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName + ".json"))
                {
                    AppCreateFile(fileName, directoryName: pathToFile);
                    WriteDefaultData(fileName, filePath: pathToFile);
                }
            }
            
            string json = ReadJson(fileName, pathToFile: pathToFile, defAppPath: defAppPath);

            if (pathToFile != null && !defAppPath)
            {
                MessageBox.Show("Method not support!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                /*if (fileName.Contains(AppConsts.DatabaseType.MySQL))
                    MySQLDatabaseInfo = JsonSerializer.Deserialize<Dictionary<string, Deserializers.DataBaseInfo>>(json);
                else if (fileName.Contains(AppConsts.DatabaseType.PostgreSQL))
                    PostgreSQLDatabaseInfo = JsonSerializer.Deserialize<Dictionary<string, Deserializers.DataBaseInfo>>(json);
                else
                    MessageBox.Show("File not valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);*/
            }
            else
            {
                switch (fileName)
                {
                    case AppConsts.FileNamesConsts.AppSettingsFileName:
                        AppSettings = JsonSerializer.Deserialize<Deserializers.ScannerSettings>(json);
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

            if (pathToFile != null)
                FilesManager.CheckPath(pathToFile);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + pathToFile + fileName + ".json", json);
        }
    }
}
