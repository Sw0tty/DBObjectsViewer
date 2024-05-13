using System;
using System.Collections.Generic;


namespace DBObjectsViewer
{
    public static class AppConsts
    {
        public const string NoneType = "Пустая";
        public static Dictionary<string, string> Types = new Dictionary<string, string>() { { "data_type", "Тип данных" }, { "required", "Обязательное поле" }, { "name", "Наименование атрибута" } };
        public static List<string> FieldsInfo = new List<string>() { "COLUMN_NAME", "DATA_TYPE", "IS_NULLABLE", "COLUMN_DEFAULT", "CHARACTER_MAXIMUM_LENGTH" };
        public static int CountOfTablesInRequest = 100;

        public static class DataBaseDataDeserializerConsts
        {
            public static List<string> TableInfoKeys = new List<string>() { "FieldsInfo", "Indexes", "Foreigns" };
            public static List<string> ColumnsHeaders = new List<string>() { "Attribute", "DataType", "Info", "DefaultValue", "MaxLength" };
        }

        public static class DatabaseType
        {
            public const string MYSQL = "MYSQL";
            public const string PostgreSQL = "PostgreSQL";
            //public const string Oracle = "Oracle";
        }

        public static class DirsConsts
        {
            public const string DirectoryOfTestDataFiles = @"TestData\";
            public const string DirectoryOfDatabaseDataFiles = @"DatabaseData\";
        }

        public static class FileNamesConsts
        {
            public const string TableTemplateFileName = "table_template.json";
            public const string SQLTestDataFileName = "sql_test_data.json";
            public const string SQLTestForeignsFileName = "sql_test_data_foreigns.json";
            public const string SQLTestIndexesFileName = "sql_test_data_indexes.json";
        }
    }
}
