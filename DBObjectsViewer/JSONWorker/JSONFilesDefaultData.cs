using System.Collections.Generic;


namespace DBObjectsViewer
{
    public static class JSONFilesDefaultData
    {
        public static Dictionary<string, string> DefaultData  = new Dictionary<string, string>()
        {
            { AppConsts.JSONConsts.TableTemplateFileName, "{\r\n    \"ADD_INDEXES_INFO\": false,\r\n    \"ADD_FOREIGN_KEYS_INFO\": false,\r\n    \"ALL_ABOUT_DATA_TYPE\": false,\r\n    \"NOT_SELECTED_COLUMNS\": {\"name\": \"Наименование атрибута\", \"data_type\": \"Тип данных\", \"required\": \"Обязательное поле\"},\r\n    \"SELECTED_COLUMNS\": {},\r\n    \"TABLE_TITLE\": [\r\n        \"Обяз. поле\",\r\n        \"Атрибут\",\r\n        \"Тип данных\",\r\n        \"Описание\"\r\n    ]\r\n}" },
            { AppConsts.JSONConsts.SQLTestDataFileName, "{\r\n  \"field1\": {\r\n    \"AtributeName\": \"ID\",\r\n    \"DataType\": \"uniqueidentifier\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": true\r\n  },\r\n  \"field2\": {\r\n    \"AtributeName\": \"CreationDateTime\",\r\n    \"DataType\": \"smalldatetime\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": true\r\n  },\r\n  \"field3\": {\r\n    \"AtributeName\": \"Type\",\r\n    \"DataType\": \"int\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": false\r\n  },\r\n  \"field4\": {\r\n    \"AtributeName\": \"Name\",\r\n    \"DataType\": \"varchar\",\r\n    \"MaxLength\": 50,\r\n    \"Required\": false\r\n  },\r\n  \"field5\": {\r\n    \"AtributeName\": \"FullName\",\r\n    \"DataType\": \"varchar\",\r\n    \"MaxLength\": -1,\r\n    \"Required\": false\r\n  },\r\n  \"field6\": {\r\n    \"AtributeName\": \"SmallValue\",\r\n    \"DataType\": \"bit\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": true\r\n  },\r\n  \"field7\": {\r\n    \"AtributeName\": \"NameOf\",\r\n    \"DataType\": \"nvarchar\",\r\n    \"MaxLength\": 255,\r\n    \"Required\": true\r\n  },\r\n  \"field8\": {\r\n    \"AtributeName\": \"DateTimeInvite\",\r\n    \"DataType\": \"datetime\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": true\r\n  },\r\n  \"field9\": {\r\n    \"AtributeName\": \"ClassLittera\",\r\n    \"DataType\": \"char\",\r\n    \"MaxLenght\": 1,\r\n    \"Required\": true\r\n  }\r\n}" },
            { AppConsts.JSONConsts.SQLTestForeignsFileName, "{\r\n  \"foreign1\": {\r\n    \"Name\": \"FK_TestTable_NameOfTest\",\r\n    \"Description\": \"FK\",\r\n    \"Column\": \"NameOf\",\r\n    \"RefTable\": \"NameOfTest\",\r\n    \"RefTableColumn\": \"TestName\"\r\n  }\r\n}" },
            { AppConsts.JSONConsts.SQLTestIndexesFileName, "{\r\n  \"index1\": {\r\n    \"Name\": \"PK_TestTable\",\r\n    \"IndexedColumn\": \"ID\",\r\n    \"Info\": [\"Clustered\"]\r\n  },\r\n  \"index2\": {\r\n    \"Name\": \"IX_TestTable\",\r\n    \"IndexedColumn\": \"Name\",\r\n    \"Info\": [\"Unique\", \"Non-Clustered\"]\r\n  }\r\n}" },
        };
    }
}
