using System;
using System.Collections.Generic;


namespace DBObjectsViewer
{
    public static class AppConsts
    {
        public const string NoneType = "Без типа";
        public static Dictionary<string, string> Types = new Dictionary<string, string>() { { "data_type", "Тип данных" }, { "required", "Обязательное поле" }, { "name", "Наименование атрибута" } };

        public static class JSONConsts
        {
            public const string DirectoryOfTestDataFiles = @"TestData\";
            public const string TableTemplateFileName = "table_template.json";
            public const string SQLTestDataFileName = "sql_test_data.json";
            public const string SQLTestForeignsFileName = "sql_test_data_foreigns.json";
            public const string SQLTestIndexesFileName = "sql_test_data_indexes.json";
        }
    }
}
