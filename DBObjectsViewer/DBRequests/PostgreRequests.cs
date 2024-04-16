using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBObjectsViewer
{
    public static class PostgreRequests
    {
        public static string ColumnsInfo(string tableName, List<string> columns)
        {
            // COLUMN_NAME, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
            return $"SELECT {string.Join(", ", columns)} FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
        }

        public static string VersionRequest()
        {
            return "SELECT version()";
        }
    }
}
