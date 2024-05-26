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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Office.Interop.Word;


namespace DBObjectsViewer.Forms
{
    public partial class WorkProgressForm : Form
    {
        private SQLDBConnector MySQLConnection { get; set; }
        private PostgreDBConnector PostgreSQLConnection { get; set; }

        public WorkProgressForm(dynamic connection)
        {
            InitializeComponent();

            if (connection is SQLDBConnector)
                MySQLConnection = connection;
            else if (connection is PostgreDBConnector)
                PostgreSQLConnection = connection;

            ScanDatabaseBGWorker.RunWorkerAsync();
        }

        private void CancelValues()
        {
            MySQLConnection = null;
            PostgreSQLConnection = null;
        }

        static void MakeWorkerReport(BackgroundWorker worker, Tuple<int, string> report)
        {
            worker.ReportProgress(report.Item1, report.Item2);
        }

        private void ScanDatabaseBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = new Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>();

            if (MySQLConnection != null)
            {
                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.OpenStatus);
                MySQLConnection.OpenConnection();

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CollectTablesStatus);
                List<string> tables = MySQLConnection.ReturnListTables();

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CollectInfoStatus);

                foreach (string compositeRequest in AppUsedFunctions.MakeCompositeRequestsToDB(tables, AppConsts.DatabaseType.MySQL))
                {
                    Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> middleInfo = AppUsedFunctions.DeepClone(MySQLConnection.ReturnTablesInfo(compositeRequest, tables));
                    
                    foreach (string key in middleInfo.Keys)
                        DBInfo.Add(key, AppUsedFunctions.DeepClone(middleInfo[key]));
                }

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.SaveDataStatus);
                JSONWorker.SaveJson(DBInfo, JSONWorker.MakeUniqueFileName(MySQLConnection.ReturnCatalogName(), AppConsts.DatabaseType.MySQL), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CloseStatus);
                MySQLConnection.CloseConnection();

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.DoneStatus);
            }
            else if (PostgreSQLConnection != null)
            {
                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.OpenStatus);
                PostgreSQLConnection.OpenConnection();

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CollectTablesStatus);
                List<string> tables = PostgreSQLConnection.ReturnListTables(PostgreSQLConnection.ReturnSchemaName());

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CollectInfoStatus);

                foreach (string compositeRequest in AppUsedFunctions.MakeCompositeRequestsToDB(tables, AppConsts.DatabaseType.PostgreSQL, schema: PostgreSQLConnection.ReturnSchemaName()))
                {
                    Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> middleInfo = AppUsedFunctions.DeepClone(PostgreSQLConnection.ReturnTablesInfo(compositeRequest, tables));

                    foreach (string key in middleInfo.Keys)
                        DBInfo.Add(key, AppUsedFunctions.DeepClone(middleInfo[key]));
                }

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.SaveDataStatus);
                JSONWorker.SaveJson(DBInfo, JSONWorker.MakeUniqueFileName(PostgreSQLConnection.ReturnDatabaseName(), AppConsts.DatabaseType.PostgreSQL), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CloseStatus);
                PostgreSQLConnection.CloseConnection();

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.DoneStatus);
            }

            this.DialogResult = DialogResult.OK;
        }

        private void WorkProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelValues();
        }

        private void ScanDatabaseBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            groupBox1.Text = e.UserState.ToString();
            progressBar1.Value = e.ProgressPercentage;
        }
    }
}
