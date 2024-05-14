using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DBObjectsViewer
{
    public class PostgreDBConnector : BasePostgreDBConnector
    {
        public PostgreDBConnector(string server, string port,string catalog, string login, string password, string schema) : base(server, port, catalog, login, password, schema) { }

        public List<string> ReturnListTables(string schema)
        {
            string request = PostgreRequests.TablesRequest(schema);
            return SelectListAdapter(request, returnsNull: true, removeEscapes: true, ReturnConnection(), ReturnTransaction());
        }

        public Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> ReturnTablesInfo(string request, List<string> tables)
        {
            return SelectCompositeDictAdapter(request, tables, returnsNull: true, removeEscapes: true, ReturnConnection(), ReturnTransaction());
        }
    }
}
