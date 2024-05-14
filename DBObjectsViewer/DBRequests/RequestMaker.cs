using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBObjectsViewer.DBRequests
{
    internal class RequestMaker
    {
        private static string UnionCommand { get; } = "\nUNION ALL\n";

        private static string MakeHeader(string header)
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
                {
                    endPos = startPos + AppConsts.CountOfTablesInRequest + tablesCount;
                }
                else
                {
                    endPos = startPos + AppConsts.CountOfTablesInRequest;
                }

                pairs.Add(new Tuple<int, int>(startPos, endPos));
                startPos = endPos;
            }
            return pairs;
        }

        public static List<string> CompositeRequestToDB(List<string> tableNames, string databaseType, string schema = null)
        {
            List<Tuple<int, int>> tablesPairs = SplitOnParts(tableNames.Count);
            List<string> compositeRequests = new List<string>();

            foreach (Tuple<int, int> pair in tablesPairs)
            {
                string compositeRequest = "";
                for (int i = pair.Item1; i < pair.Item2; i++)
                {
                    string tableName = tableNames[i];

                    compositeRequest += MakeHeader(tableName) + UnionCommand;
                    compositeRequest += MakeHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[0]) + UnionCommand;

                    if (databaseType == AppConsts.DatabaseType.MYSQL)
                        compositeRequest += SQLRequests.ColumnsInfo(tableName, AppConsts.FieldsInfo);
                    else if (databaseType == AppConsts.DatabaseType.PostgreSQL)
                        compositeRequest += PostgreRequests.ColumnsInfo(tableName, AppConsts.FieldsInfo);

                    if (JSONWorker.TableTemplateData.AddIndexesInfo)
                    {
                        compositeRequest += UnionCommand;
                        compositeRequest += MakeHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[1]) + UnionCommand;

                        if (databaseType == AppConsts.DatabaseType.MYSQL)
                            compositeRequest += SQLRequests.TableIndexesRequest(tableName);
                       /* else if (databaseType == AppConsts.DatabaseType.PostgreSQL)
                            compositeRequest += PostgreRequests.TableIndexesRequest(tableName);*/
                    }
                    if (JSONWorker.TableTemplateData.AddForeignInfo)
                    {
                        compositeRequest += UnionCommand;
                        compositeRequest += MakeHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[2]) + UnionCommand;

                        if (databaseType == AppConsts.DatabaseType.MYSQL)
                            compositeRequest += SQLRequests.SelectForeignKeysInfoRequest(tableName);
                        else if (databaseType == AppConsts.DatabaseType.PostgreSQL)
                            compositeRequest += PostgreRequests.SelectForeignKeysInfoRequest(tableName, schema);
                    }
                    if (tableNames[pair.Item2 - 1] != tableName)
                        compositeRequest += UnionCommand;
                }
                compositeRequests.Add(compositeRequest);
            }

            return compositeRequests;
        }
    }
}
