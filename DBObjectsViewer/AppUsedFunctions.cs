using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;


namespace DBObjectsViewer
{
    public static class AppUsedFunctions
    {
        public static T DeepClone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static string ReturnSupportedFormats(Dictionary<string, string> supportedFormats)
        {
            string formats = null;
            foreach (string key in supportedFormats.Keys)
                formats += $"{supportedFormats[key]} (*.{key})|*.{key}|";
            return formats;
        }

        public static string SelectFileOnPC(string startDir, Dictionary<string, string> supportedFormats = null)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = startDir;
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

        private static string MakeRequestHeader(string header)
        {
            return $"SELECT '{header}' AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[0]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[1]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[2]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[3]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[4]}"; ;
        }

        public static List<Tuple<int, int>> SplitOnParts(int tablesCount)
        {
            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();

            if (tablesCount < AppConsts.CountOfTablesInRequest)
            {
                pairs.Add(new Tuple<int, int>(0, tablesCount));
                return pairs;
            }
            else if (tablesCount == AppConsts.CountOfTablesInRequest)
            {
                pairs.Add(new Tuple<int, int>(0, AppConsts.CountOfTablesInRequest));
                return pairs;
            }

            pairs.Add(new Tuple<int, int>(0, AppConsts.CountOfTablesInRequest));
            tablesCount -= AppConsts.CountOfTablesInRequest;
            int startPos = AppConsts.CountOfTablesInRequest;
            int endPos;

            while (tablesCount > 0)
            {
                tablesCount -= AppConsts.CountOfTablesInRequest;

                if (tablesCount < 0)
                    endPos = startPos + AppConsts.CountOfTablesInRequest + tablesCount;
                else
                    endPos = startPos + AppConsts.CountOfTablesInRequest;

                pairs.Add(new Tuple<int, int>(startPos, endPos));
                startPos = endPos;
            }
            return pairs;
        }

        public static List<string> MakeCompositeRequestsToDB(List<string> tableNames, string databaseType, string schema = null)
        {
            List<Tuple<int, int>> tablesPairs = SplitOnParts(tableNames.Count);
            List<string> compositeRequests = new List<string>();

            foreach (Tuple<int, int> pair in tablesPairs)
            {
                string compositeRequest = "";
                for (int i = pair.Item1; i < pair.Item2; i++)
                {
                    string tableName = tableNames[i];

                    compositeRequest += MakeRequestHeader(tableName) + AppConsts.DBConsts.UnionCommand;
                    compositeRequest += MakeRequestHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[0]) + AppConsts.DBConsts.UnionCommand;

                    if (databaseType == AppConsts.DatabaseType.MySQL)
                        compositeRequest += SQLRequests.ColumnsInfo(tableName, AppConsts.FieldsInfo);
                    else if (databaseType == AppConsts.DatabaseType.PostgreSQL)
                        compositeRequest += PostgreRequests.ColumnsInfo(tableName, AppConsts.FieldsInfo);

                    if (JSONWorker.TableTemplateData.AddIndexesInfo)
                    {
                        compositeRequest += AppConsts.DBConsts.UnionCommand;
                        compositeRequest += MakeRequestHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[1]) + AppConsts.DBConsts.UnionCommand;

                        if (databaseType == AppConsts.DatabaseType.MySQL)
                            compositeRequest += SQLRequests.TableIndexesRequest(tableName);
                        /* else if (databaseType == AppConsts.DatabaseType.PostgreSQL)
                             compositeRequest += PostgreRequests.TableIndexesRequest(tableName);*/
                    }
                    if (JSONWorker.TableTemplateData.AddForeignInfo)
                    {
                        compositeRequest += AppConsts.DBConsts.UnionCommand;
                        compositeRequest += MakeRequestHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[2]) + AppConsts.DBConsts.UnionCommand;

                        if (databaseType == AppConsts.DatabaseType.MySQL)
                            compositeRequest += SQLRequests.SelectForeignKeysInfoRequest(tableName);
                        else if (databaseType == AppConsts.DatabaseType.PostgreSQL)
                            compositeRequest += PostgreRequests.SelectForeignKeysInfoRequest(tableName, schema);
                    }
                    if (tableNames[pair.Item2 - 1] != tableName)
                        compositeRequest += AppConsts.DBConsts.UnionCommand;
                }
                compositeRequests.Add(compositeRequest);
            }

            return compositeRequests;
        }

        public static bool CheckSupportFormat(string filePath)
        {
            if (filePath == AppConsts.FileDialogSupportedFormats.UnsupportFormatStatus)
            {
                MessageBox.Show("Unsupported format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (filePath == null)
            {
                MessageBox.Show("File selecting cancelled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }
    }
}
