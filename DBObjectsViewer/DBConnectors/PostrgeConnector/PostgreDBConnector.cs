using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DBObjectsViewer
{
    public class PostgreDBConnector : BasePostgreDBConnector
    {
        public PostgreDBConnector(string server, string port,string catalog, string login, string password) : base(server, port, catalog, login, password) { }

        public string GetVersion()
        {
            string request = PostgreRequests.VersionRequest();
            return SelectSingleValueAdapter(request, likeValue: false, ReturnConnection(), ReturnTransaction());
        }
    }
}
