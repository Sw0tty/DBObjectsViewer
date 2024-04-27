﻿using System.Collections.Generic;


namespace DBObjectsViewer
{
    public static class JSONFilesDefaultData
    {
        public static Dictionary<string, string> DefaultData  = new Dictionary<string, string>()
        {
            { AppConsts.JSONConsts.TableTemplateFileName, "{\r\n    \"AddIndexesInfo\": false,\r\n    \"AddForeignInfo\": true,\r\n    \"AllAboutDataType\": true,\r\n    \"NotSelectedColumns\": {\r\n        \"data_type\": \"\\u0422\\u0438\\u043F \\u0434\\u0430\\u043D\\u043D\\u044B\\u0445\"\r\n    },\r\n    \"SelectedColumns\": {\r\n        \"name\": \"\\u041D\\u0430\\u0438\\u043C\\u0435\\u043D\\u043E\\u0432\\u0430\\u043D\\u0438\\u0435 \\u0430\\u0442\\u0440\\u0438\\u0431\\u0443\\u0442\\u0430\",\r\n        \"required\": \"\\u041E\\u0431\\u044F\\u0437\\u0430\\u0442\\u0435\\u043B\\u044C\\u043D\\u043E\\u0435 \\u043F\\u043E\\u043B\\u0435\"\r\n    },\r\n    \"TableTitle\": [\r\n        \"Обяз. поле\",\r\n        \"Атрибут\",\r\n        \"Тип данных\",\r\n        \"Описание\"\r\n    ]\r\n}" },
            { AppConsts.JSONConsts.SQLTestDataFileName, "{\r\n  \"field1\": {\r\n    \"AtributeName\": \"ID\",\r\n    \"DataType\": \"uniqueidentifier\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": true\r\n  },\r\n  \"field2\": {\r\n    \"AtributeName\": \"CreationDateTime\",\r\n    \"DataType\": \"smalldatetime\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": true\r\n  },\r\n  \"field3\": {\r\n    \"AtributeName\": \"Type\",\r\n    \"DataType\": \"int\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": false\r\n  },\r\n  \"field4\": {\r\n    \"AtributeName\": \"Name\",\r\n    \"DataType\": \"varchar\",\r\n    \"MaxLength\": 50,\r\n    \"Required\": false\r\n  },\r\n  \"field5\": {\r\n    \"AtributeName\": \"FullName\",\r\n    \"DataType\": \"varchar\",\r\n    \"MaxLength\": -1,\r\n    \"Required\": false\r\n  },\r\n  \"field6\": {\r\n    \"AtributeName\": \"SmallValue\",\r\n    \"DataType\": \"bit\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": true\r\n  },\r\n  \"field7\": {\r\n    \"AtributeName\": \"NameOf\",\r\n    \"DataType\": \"nvarchar\",\r\n    \"MaxLength\": 255,\r\n    \"Required\": true\r\n  },\r\n  \"field8\": {\r\n    \"AtributeName\": \"DateTimeInvite\",\r\n    \"DataType\": \"datetime\",\r\n    \"MaxLength\": 0,\r\n    \"Required\": true\r\n  },\r\n  \"field9\": {\r\n    \"AtributeName\": \"ClassLittera\",\r\n    \"DataType\": \"char\",\r\n    \"MaxLenght\": 1,\r\n    \"Required\": true\r\n  }\r\n}" },
            { AppConsts.JSONConsts.SQLTestForeignsFileName, "{\r\n  \"foreign1\": {\r\n    \"Name\": \"FK_TestTable_NameOfTest\",\r\n    \"Description\": \"FK\",\r\n    \"Column\": \"NameOf\",\r\n    \"RefTable\": \"NameOfTest\",\r\n    \"RefTableColumn\": \"TestName\"\r\n  }\r\n}" },
            { AppConsts.JSONConsts.SQLTestIndexesFileName, "{\r\n  \"index1\": {\r\n    \"Name\": \"PK_TestTable\",\r\n    \"IndexedColumn\": \"ID\",\r\n    \"Info\": [\"Clustered\"]\r\n  },\r\n  \"index2\": {\r\n    \"Name\": \"IX_TestTable\",\r\n    \"IndexedColumn\": \"Name\",\r\n    \"Info\": [\"Unique\", \"Non-Clustered\"]\r\n  }\r\n}" },
        };
    }
}
