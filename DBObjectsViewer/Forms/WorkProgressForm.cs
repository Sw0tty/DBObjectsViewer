using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBObjectsViewer.Forms
{
    public partial class WorkProgressForm : Form
    {
        private SQLDBConnector SQLConnection { get; }

        public WorkProgressForm(dynamic connection)
        {
            InitializeComponent();

            SQLConnection = null;
            SQLConnection = connection;
        }

        private void ScanDatabaseBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SQLConnection.OpenConnection();
            List<string> tables = SQLConnection.ReturnListTables();

            Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = SQLConnection.ReturnTablesInfo(tables);
            JSONWorker.SaveJson(DBInfo, $"{SQLConnection.ReturnCatalogName()}_{AppConsts.DatabaseType.MYSQL}_" + DateTime.Now.ToString("MMddyyHHmmss") + ".json", pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

            SQLConnection.CloseConnection();
        }
    }
}
