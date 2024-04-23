﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBObjectsViewer
{
    public class Deserializers
    {
        public class TableTemplate
        {
            public bool AddIndexesInfo { get; set; }
            public bool AddForeignInfo { get; set; }
            public bool AllAboutDataType { get; set; }
            public Dictionary<string, string> SelectedColumns { get; set; }
            public Dictionary<string, string> NotSelectedColumns { get; set; }
            public List<string> TableTitle { get; set; }
        }

        public class TestTableFields
        {
            public string AtributeName { get; set; }
            public string DataType { get; set; }
            public int MaxLength { get; set; }
            public bool Required { get; set; }
        }

        public class TestIndexes
        {
            public string Name { get; set; }
            public string IndexedColumn { get; set; }
            public List<string> Info { get; set; }
        }

        public class TestForeigns
        {
            public string Name { get; set; }
            public string IndexedColumn { get; set; }
            public List<string> Info { get; set; }
        }
    }
}
