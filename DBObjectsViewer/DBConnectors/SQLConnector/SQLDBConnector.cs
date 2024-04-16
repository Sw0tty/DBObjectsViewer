using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DBObjectsViewer
{
    public class SQLDBConnector : BaseSQLDBConnector
    {
        public SQLDBConnector(string source, string catalog, string login, string password) : base(source, catalog, login, password) { }
    }
}
