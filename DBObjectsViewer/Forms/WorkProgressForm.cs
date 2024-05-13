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
        private SQLDBConnector MYSQLConnection { get; set; }
        private PostgreDBConnector PostgreSQLConnection { get; set; }

        public WorkProgressForm(dynamic connection)
        {
            InitializeComponent();

            if (connection is SQLDBConnector)
                MYSQLConnection = connection;
            else if (connection is PostgreDBConnector)
                PostgreSQLConnection = connection;
        }

        private void CancelValues()
        {
            MYSQLConnection = null;
            PostgreSQLConnection = null;
        }

        private void ScanDatabaseBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (MYSQLConnection != null)
            {
                worker.ReportProgress(100, "Open connection...");
                MYSQLConnection.OpenConnection();
                List<string> tables = MYSQLConnection.ReturnListTables();

                Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = MYSQLConnection.ReturnTablesInfo(tables);
                JSONWorker.SaveJson(DBInfo, JSONWorker.MakeUniqueFileName(MYSQLConnection.ReturnCatalogName(), AppConsts.DatabaseType.MYSQL), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

                worker.ReportProgress(100, "Close connection...");
                MYSQLConnection.CloseConnection();
            }
            else if (PostgreSQLConnection != null)
            {
                worker.ReportProgress(100, "Open connection...");
                PostgreSQLConnection.OpenConnection();


                worker.ReportProgress(100, "Close connection...");
                PostgreSQLConnection.CloseConnection();
            }
        }

        private void WorkProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelValues();
        }

        private void ScanDatabaseBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
    }
}
