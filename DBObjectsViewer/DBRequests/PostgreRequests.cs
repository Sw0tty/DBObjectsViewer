using System.Collections.Generic;


namespace DBObjectsViewer
{
    public static class PostgreRequests
    {
        public static string SchemasRequest()
        {
            return "SELECT schema_name FROM information_schema.schemata";
        }

        public static string ColumnsInfo(string tableName, List<string> columns)
        {
            // COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT, CHARACTER_MAXIMUM_LENGTH
            return $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT, CAST(CHARACTER_MAXIMUM_LENGTH as text) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
        }

        public static string TablesRequest(string schema)
        {
            return $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{schema}' ORDER BY TABLE_NAME";
        }

        public static string SelectForeignKeysInfoRequest(string tableName, string schema)
        {
            return "SELECT " +
                   "tc.constraint_name, " +
                   "'FK', " +
                   "'(' || kcu.column_name || ') ref ' || ccu.table_name || ' (' || ccu.column_name || ')', " +
                   "NULL, " +
                   "NULL " +
                   "FROM information_schema.table_constraints AS tc JOIN information_schema.key_column_usage AS kcu ON tc.constraint_name = kcu.constraint_name AND tc.table_schema = kcu.table_schema JOIN information_schema.constraint_column_usage AS ccu ON ccu.constraint_name = tc.constraint_name " +
                   $"WHERE tc.constraint_type = 'FOREIGN KEY' AND tc.table_schema = '{schema}' AND tc.table_name = '{tableName}'";
        }
    }
}
