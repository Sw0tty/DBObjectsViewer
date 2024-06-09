using DocumentFormat.OpenXml.Office2016.Excel;
using System.Collections.Generic;


namespace DBObjectsViewer
{
    public static class SQLRequests
    {
        public static string ColumnsInfo(string tableName, List<string> columns)
        {
            // COLUMN_NAME, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
            // CASE WHEN IS_NULLABLE = 'YES' THEN 'Required' ELSE 'Non-required' END
            string request = $"SELECT {string.Join(", ", columns)} FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
            if (request.Contains("IS_NULLABLE"))
                request = request.Replace("IS_NULLABLE", $"CASE WHEN IS_NULLABLE = 'NO' THEN '{AppConsts.DBConsts.RequiredInfo}' ELSE '{AppConsts.DBConsts.NonRequiredInfo}' END");
            return request;
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
