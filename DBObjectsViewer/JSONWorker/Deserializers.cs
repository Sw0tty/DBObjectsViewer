using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBObjectsViewer
{
    public class Deserializers
    {
        public class DataBaseInfo
        {
            public List<Dictionary<string, string>> FieldsInfo { get; set; }
            public List<Dictionary<string, string>> Indexes { get; set; }
            public List<Dictionary<string, string>> Foreigns { get; set; }
        }

/*        public class ParamInfoFields
        {
            //{ "Attribute", "DataType", "Info", "DefaultValue", "MaxLength" };
            public string Attribute { get; set; }
            public string DataType { get; set; }
            public string Info { get; set; }
            public string DefaultValue { get; set; }
            public string MaxLength { get; set; }
        }*/

        public class TableTemplate
        {
            public bool AddIndexesInfo { get; set; }
            public bool AddForeignInfo { get; set; }
            public bool AllAboutDataType { get; set; }
            public Dictionary<string, int> IndexParamsColumnsNum { get; set; }
            public Dictionary<string, string> SelectedColumns { get; set; }
            public Dictionary<string, string> NotSelectedColumns { get; set; }
            //public List<string> TableTitle { get; set; }
            public List<Tuple<string, string>> TableTitle { get; set; }
        }

        public class TestTableFields
        {
            public string AtributeName { get; set; }
            public string DataType { get; set; }
            public int MaxLength { get; set; }
            public bool Required { get; set; }
        }

        public class TestForeigns
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Column { get; set; }
            public string RefTable { get; set; }
            public string RefTableColumn { get; set; }
        }

        public class TestIndexes
        {
            public string Name { get; set; }
            public string IndexedColumn { get; set; }
            public List<string> Info { get; set; }
        }
    }
}
