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
using DBObjectsViewer.DBRequests;


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

            ScanDatabaseBGWorker.RunWorkerAsync();
        }

        private void CancelValues()
        {
            MYSQLConnection = null;
            PostgreSQLConnection = null;
        }

        static T DeepClone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        static void MakeWorkerReport(BackgroundWorker worker, Tuple<int, string> report)
        {
            worker.ReportProgress(report.Item1, report.Item2);
        }

        private void ScanDatabaseBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = new Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>();

            if (MYSQLConnection != null)
            {
                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.OpenStatus);
                MYSQLConnection.OpenConnection();

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CollectTablesStatus);
                List<string> tables = MYSQLConnection.ReturnListTables();

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CollectInfoStatus);

                foreach (string compositeRequest in RequestMaker.CompositeRequestToDB(tables, AppConsts.DatabaseType.MYSQL))
                {
                    Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> middleInfo = DeepClone(MYSQLConnection.ReturnTablesInfo(compositeRequest, tables));
                    
                    foreach (string key in middleInfo.Keys)
                        DBInfo.Add(key, DeepClone(middleInfo[key]));
                }

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.SaveDataStatus);
                JSONWorker.SaveJson(DBInfo, JSONWorker.MakeUniqueFileName(MYSQLConnection.ReturnCatalogName(), AppConsts.DatabaseType.MYSQL), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CloseStatus);
                MYSQLConnection.CloseConnection();

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.DoneStatus);
            }
            else if (PostgreSQLConnection != null)
            {
                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.OpenStatus);
                PostgreSQLConnection.OpenConnection();

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CollectTablesStatus);
                List<string> tables = PostgreSQLConnection.ReturnListTables(PostgreSQLConnection.ReturnSchemaName());

                MakeWorkerReport(worker, AppConsts.ScanProgressConsts.CollectInfoStatus);

                foreach (string compositeRequest in RequestMaker.CompositeRequestToDB(tables, AppConsts.DatabaseType.PostgreSQL, schema: PostgreSQLConnection.ReturnSchemaName()))
                {
                    Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> middleInfo = DeepClone(PostgreSQLConnection.ReturnTablesInfo(compositeRequest, tables));

                    foreach (string key in middleInfo.Keys)
                        DBInfo.Add(key, DeepClone(middleInfo[key]));
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
