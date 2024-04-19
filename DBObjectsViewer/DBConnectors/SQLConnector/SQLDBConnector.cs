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

        public List<Dictionary<string, string>> ReturnTables()
        {
            string request = SQLRequests.TablesRequest();
            return SelectAdapter(request, allowsNull: true, ReturnConnection(), ReturnTransaction());
        }

        public List<Dictionary<string, string>> ReturnTableColumnsInfo(string tableName, List<string> infoColumns)
        {
            string request = SQLRequests.ColumnsInfo(tableName, infoColumns);
            return SelectAdapter(request, allowsNull: true, ReturnConnection(), ReturnTransaction());
        }

        public List<Dictionary<string, string>> ReturnIndexesInfo(string tableName)
        {
            string request = SQLRequests.TableIndexesRequest(tableName);
            return SelectAdapter(request, allowsNull: true, ReturnConnection(), ReturnTransaction());
        }

        public List<Dictionary<string, string>> ReturnForeignsInfo(string tableName)
        {
            string request = SQLRequests.SelectForeignKeysInfoRequest(tableName);
            return SelectAdapter(request, allowsNull: true, ReturnConnection(), ReturnTransaction());
        }

        public List<Dictionary<string, string>> ReturnPrimaryInfo(string tableName)
        {
            string request = SQLRequests.PrimaryKeyRequest(tableName);
            return SelectAdapter(request, allowsNull: true, ReturnConnection(), ReturnTransaction());
        }
    }
}
