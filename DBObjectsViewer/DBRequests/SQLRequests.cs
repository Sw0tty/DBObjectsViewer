using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBObjectsViewer
{
    public static class SQLRequests
    {
        public static string ColumnsInfo(string tableName, List<string> columns)
        {
            // COLUMN_NAME, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
            return $"SELECT {string.Join(", ", columns)} FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
        }

        public static string TableIndexesRequest(string tableName)
        {
            return "select i.[name] as index_name, " +
                   "substring(column_names, 1, len(column_names)-1) as [columns], " +
                   "case when i.[type] = 1 then 'Clustered' when i.[type] = 2 then 'Nonclustered unique index' when i.[type] = 3 then 'XML index' when i.[type] = 4 then 'Spatial index' when i.[type] = 5 then 'Clustered columnstore index' when i.[type] = 6 then 'Nonclustered columnstore index' when i.[type] = 7 then 'Nonclustered hash index' end as index_type, case when i.is_unique = 1 then 'Unique' else 'Not unique' end as [unique], " +
                   "t.[name] as table_view " +
                   "from sys.objects t inner join sys.indexes i on t.object_id = i.object_id cross apply (select col.[name] + ', ' from sys.index_columns ic inner join sys.columns col on ic.object_id = col.object_id and ic.column_id = col.column_id where ic.object_id = t.object_id and ic.index_id = i.index_id order by key_ordinal for xml path ('') ) D (column_names) " +
                   $"where t.is_ms_shipped <> 1 and index_id > 0 and t.[name] = '{tableName}' order by i.[name]";
        }

        public static string SelectForeignKeysInfoRequest(string tableName)
        {
            // COL_NAME(fk_c.parent_object_id, fk_c.parent_column_id) через какую колонку используется
            // name наименование ключа
            // OBJECT_NAME (fk.referenced_object_id) на какую ттаблицу ссылается
            return "SELECT COL_NAME(fk_c.parent_object_id, fk_c.parent_column_id), name, OBJECT_NAME (fk.referenced_object_id) " +
                   "FROM sys.foreign_keys AS fk INNER JOIN sys.foreign_key_columns AS fk_c ON fk.object_id = fk_c.constraint_object_id " +
                   $"WHERE OBJECT_NAME(fk.parent_object_id) = '{tableName}';";
        }
    }
}
