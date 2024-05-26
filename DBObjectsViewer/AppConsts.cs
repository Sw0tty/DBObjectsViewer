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

        public static class DBConsts
        {
            public const string Schema = "public";
            public const string UnionCommand = "\nUNION ALL\n";
        }

        public static class FileDialogSupportedFormats
        {
            public const string UnsupportFormatStatus = "$UnsupporT$";
            public static Dictionary<string, string> ExcelFormats = new Dictionary<string, string>() { { "xlsx", "Excel files" } };
            public static Dictionary<string, string> JsonFormats = new Dictionary<string, string>() { { "json", "Json files" } };
        }

        public static class DatabaseType
        {
            public const string MySQL = "MySQL";
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

        public static class ScanProgressConsts
        {
            public static Tuple<int, string> OpenStatus = new Tuple<int, string>(10, "Open connection...");
            public static Tuple<int, string> CollectTablesStatus = new Tuple<int, string>(20, "Сollect tables...");
            public static Tuple<int, string> CollectInfoStatus = new Tuple<int, string>(50, "Collect info about database...");
            public static Tuple<int, string> SaveDataStatus = new Tuple<int, string>(80, "Save data in file...");
            public static Tuple<int, string> CloseStatus = new Tuple<int, string>(90, "Close connection...");
            public static Tuple<int, string> DoneStatus = new Tuple<int, string>(100, "Done");
        }
    }
}
