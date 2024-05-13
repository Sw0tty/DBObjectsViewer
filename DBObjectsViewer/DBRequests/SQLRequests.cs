using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBObjectsViewer
{
    public static class SQLRequests
    {
        private static string UnionCommand { get; } = "\nUNION ALL\n";

        private static string MakeHeader(string header)
        {
            //NULL AS data_type, NULL AS info, NULL AS default_value, NULL AS max_length
            string headerRow = $"SELECT '{header}' AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[0]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[1]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[2]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[3]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[4]}";
            /*for (int i = 1; i < AppConsts.FieldsInfo.Count; i++)
                headerRow += (i == AppConsts.FieldsInfo.Count - 1) ? "NULL" : "NULL, ";*/
            return headerRow;
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

        public static string CompositeRequestToDB(List<string> tableNames/*, List<string> fieldsInfo*/)
        {
            List<Tuple<int, int>> tablesPairs = SplitOnParts(tableNames.Count);

            string compositeRequest = "";

            foreach (string tableName in tableNames)
            {
                compositeRequest += MakeHeader(tableName) + UnionCommand;
                // Взаимодействие с настройками и проверка на необходимые данные.
                compositeRequest += MakeHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[0]) + UnionCommand;
                compositeRequest += ColumnsInfo(tableName, AppConsts.FieldsInfo);

                if (JSONWorker.TableTemplateData.AddIndexesInfo)
                {
                    compositeRequest += UnionCommand;
                    compositeRequest += MakeHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[1]) + UnionCommand;
                    compositeRequest += TableIndexesRequest(tableName);
                }
                if (JSONWorker.TableTemplateData.AddForeignInfo)
                {
                    compositeRequest += UnionCommand;
                    compositeRequest += MakeHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[2]) + UnionCommand;
                    compositeRequest += SelectForeignKeysInfoRequest(tableName);
                }
                if (tableNames[tableNames.Count - 1] != tableName)
                    compositeRequest += UnionCommand;
            }

            return compositeRequest;
        }

        public static string ColumnsInfo(string tableName, List<string> columns)
        {
            // COLUMN_NAME, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
            // CASE WHEN IS_NULLABLE = 'YES' THEN 'Required' ELSE 'Non-required' END
            return $"SELECT {string.Join(", ", columns)} FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
        }

        public static string PrimaryKeyRequest(string tableName)
        {
            return "SELECT Col.Column_Name from " +
                   "INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, " +
                   "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col WHERE Col.Constraint_Name = Tab.Constraint_Name " +
                   $"AND Col.Table_Name = '{tableName}' AND Tab.Constraint_Type = 'PRIMARY KEY'";
        }

        public static string TablesRequest()
        {
            return "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
        }

        public static string TableIndexesRequest(string tableName)
        {
            /*return "select i.[name] as index_name, " +
                   "case when i.[type] = 1 then 'Clustered' when i.[type] = 2 then 'Non-Clustered' when i.[type] = 3 then 'XML index' when i.[type] = 4 then 'Spatial index' when i.[type] = 5 then 'Clustered columnstore index' when i.[type] = 6 then 'Nonclustered columnstore index' when i.[type] = 7 then 'Nonclustered hash index' end as index_type, case when i.is_unique = 1 then 'Unique' else 'Not unique' end as [unique], " +
                   "substring(column_names, 1, len(column_names)-1) as [columns] " +
                   "from sys.objects t inner join sys.indexes i on t.object_id = i.object_id cross apply (select col.[name] + ', ' from sys.index_columns ic inner join sys.columns col on ic.object_id = col.object_id and ic.column_id = col.column_id where ic.object_id = t.object_id and ic.index_id = i.index_id order by key_ordinal for xml path ('') ) D (column_names) " +
                   $"where t.is_ms_shipped <> 1 and index_id > 0 and t.[name] = {tableName}";*/
            return "select i.[name] as index_name, " +
                   "'ON ' + substring(column_names, 1, len(column_names)-1) as [on_column], " +
                   "case when i.[type] = 1 then 'Clustered' when i.[type] = 2 then 'Non-Clustered' when i.[type] = 3 then 'XML index' when i.[type] = 4 then 'Spatial index' when i.[type] = 5 then 'Clustered columnstore index' when i.[type] = 6 then 'Nonclustered columnstore index' when i.[type] = 7 then 'Nonclustered hash index' end + ', ' + case when i.is_unique = 1 then 'Unique' else 'Not unique' end as [info], " +
                   "NULL, " +
                   "NULL " +
                   "from sys.objects t inner join sys.indexes i on t.object_id = i.object_id cross apply (select col.[name] + ', ' from sys.index_columns ic inner join sys.columns col on ic.object_id = col.object_id and ic.column_id = col.column_id where ic.object_id = t.object_id and ic.index_id = i.index_id order by key_ordinal for xml path ('') ) D (column_names) " +
                   $"where t.is_ms_shipped <> 1 and index_id > 0 and t.[name] = '{tableName}'";
        }

        public static string SelectForeignKeysInfoRequest(string tableName)
        {
            // COL_NAME(fk_c.parent_object_id, fk_c.parent_column_id) через какую колонку используется
            // name наименование ключа
            // OBJECT_NAME (fk.referenced_object_id) на какую ттаблицу ссылается
            /*return "SELECT name as key_name, COL_NAME(fk_c.parent_object_id, fk_c.parent_column_id) as column_name, OBJECT_NAME (fk.referenced_object_id) as table_name, " +
                   "COL_NAME(fk.referenced_object_id, fk_c.referenced_column_id) AS referenced_column_name " +
                   "FROM sys.foreign_keys AS fk INNER JOIN sys.foreign_key_columns AS fk_c ON fk.object_id = fk_c.constraint_object_id " +
                   $"WHERE OBJECT_NAME(fk.parent_object_id) = {tableName}";*/
            return "SELECT name, " +
                   "'FK', " +
                   "'(' + COL_NAME(fk_c.parent_object_id, fk_c.parent_column_id) + ') ref ' + OBJECT_NAME (fk.referenced_object_id) + ' (' + COL_NAME(fk.referenced_object_id, fk_c.referenced_column_id) + ')', " +
                   "NULL, " +
                   "NULL " +
                   "FROM sys.foreign_keys AS fk INNER JOIN sys.foreign_key_columns AS fk_c ON fk.object_id = fk_c.constraint_object_id " +
                   $"WHERE OBJECT_NAME(fk.parent_object_id) = '{tableName}'";
        }
    }
}
