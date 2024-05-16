using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBObjectsViewer.DBConnectors.OracleConnector
{
    public class OracleDBConnector : BaseOracleDBConnector
    {
        public OracleDBConnector(string server, string port, string database, string login, string password) : base(server, port, database, login, password) { }

/*        public List<string> ReturnListTables(string schema)
        {
            string request = PostgreRequests.TablesRequest(schema);
            return SelectListAdapter(request, returnsNull: true, removeEscapes: true, ReturnConnection(), ReturnTransaction());
        }

        public Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> ReturnTablesInfo(string request, List<string> tables)
        {
            return SelectCompositeDictAdapter(request, tables, returnsNull: true, removeEscapes: true, ReturnConnection(), ReturnTransaction());
        }*/
    }
}
