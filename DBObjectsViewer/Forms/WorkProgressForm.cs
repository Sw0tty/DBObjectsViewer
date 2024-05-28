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
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;


namespace DBObjectsViewer.Forms
{
    public partial class WorkProgressForm : Form
    {
        private SQLDBConnector MySQLConnection { get; set; }
        private PostgreDBConnector PostgreSQLConnection { get; set; }

        public WorkProgressForm(dynamic connection = null, string filePath = null)
        {
            InitializeComponent();

            if (connection is SQLDBConnector)
                MySQLConnection = connection;
            else if (connection is PostgreDBConnector)
                PostgreSQLConnection = connection;

            if (connection != null)
                ScanDatabaseBGWorker.RunWorkerAsync();

            if (filePath != null)
                ConvertExcelToJsonDBWorker.RunWorkerAsync(filePath);
        }

        private void CancelValues()
        {
            MySQLConnection = null;
            PostgreSQLConnection = null;
        }

        private void WorkProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelValues();
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
                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.OpenStatus);
                MySQLConnection.OpenConnection();

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.CollectTablesStatus);
                List<string> tables = MySQLConnection.ReturnListTables();

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.CollectInfoStatus);

                foreach (string compositeRequest in AppUsedFunctions.MakeCompositeRequestsToDB(tables, AppConsts.DatabaseType.MySQL))
                {
                    Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> middleInfo = AppUsedFunctions.DeepClone(MySQLConnection.ReturnTablesInfo(compositeRequest, tables));
                    
                    foreach (string key in middleInfo.Keys)
                        DBInfo.Add(key, AppUsedFunctions.DeepClone(middleInfo[key]));
                }

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.SaveDataStatus);
                JSONWorker.SaveJson(DBInfo, JSONWorker.MakeUniqueFileName(MySQLConnection.ReturnCatalogName(), AppConsts.DatabaseType.MySQL), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.CloseStatus);
                MySQLConnection.CloseConnection();

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.DoneStatus);
            }
            else if (PostgreSQLConnection != null)
            {
                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.OpenStatus);
                PostgreSQLConnection.OpenConnection();

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.CollectTablesStatus);
                List<string> tables = PostgreSQLConnection.ReturnListTables(PostgreSQLConnection.ReturnSchemaName());

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.CollectInfoStatus);

                foreach (string compositeRequest in AppUsedFunctions.MakeCompositeRequestsToDB(tables, AppConsts.DatabaseType.PostgreSQL, schema: PostgreSQLConnection.ReturnSchemaName()))
                {
                    Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> middleInfo = AppUsedFunctions.DeepClone(PostgreSQLConnection.ReturnTablesInfo(compositeRequest, tables));

                    foreach (string key in middleInfo.Keys)
                        DBInfo.Add(key, AppUsedFunctions.DeepClone(middleInfo[key]));
                }

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.SaveDataStatus);
                JSONWorker.SaveJson(DBInfo, JSONWorker.MakeUniqueFileName(PostgreSQLConnection.ReturnDatabaseName(), AppConsts.DatabaseType.PostgreSQL), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.CloseStatus);
                PostgreSQLConnection.CloseConnection();

                MakeWorkerReport(worker, AppConsts.ProgressConsts.ScanConsts.DoneStatus);
            }

            this.DialogResult = DialogResult.OK;
        }

        private void ScanDatabaseBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            groupBox1.Text = e.UserState.ToString();
            progressBar1.Value = e.ProgressPercentage;
        }

        private void ConvertExcelToJsonDBWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = new Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>();

            MakeWorkerReport(worker, AppConsts.ProgressConsts.ConvertETJConsts.OpenStatus);
            Excel.Application excel = new Excel.Application();
            Workbook workbook = excel.Workbooks.Open($@"{e.Argument}");
            Worksheet worksheet = (Worksheet)workbook.Worksheets["Лист1"];

            string tableName = null;
            string tableInfoKey = null;

            for (int row = 1; row <= worksheet.UsedRange.Rows.Count; row++)
            {
                Dictionary<string, string> rowInfo = new Dictionary<string, string>();

                if (!AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys.Contains(worksheet.Cells[row, 1].Value2.ToString()) && worksheet.Cells[row, 2].Value2.ToString().ToLower() == "null")
                {
                    tableName = worksheet.Cells[row, 1].Value2.ToString();
                    DBInfo.Add(tableName, new Dictionary<string, List<Dictionary<string, string>>>());
                    continue;
                }
                if (AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys.Contains(worksheet.Cells[row, 1].Value2.ToString()))
                {
                    tableInfoKey = worksheet.Cells[row, 1].Value2.ToString();
                    DBInfo[tableName].Add(tableInfoKey, new List<Dictionary<string, string>>());
                    continue;
                }

                for (int column = 1; column <= worksheet.UsedRange.Columns.Count; column++)
                    rowInfo.Add(AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[column - 1], worksheet.Cells[row, column].Value2.ToString());

                DBInfo[tableName][tableInfoKey].Add(new Dictionary<string, string>(rowInfo));
            }

            workbook.Close();
            excel.Quit();

            //JSONWorker.SaveJson(DBInfo, JSONWorker.MakeUniqueFileName("ExcelScan", DataBaseType), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

            MakeWorkerReport(worker, AppConsts.ProgressConsts.ConvertETJConsts.DoneStatus);

            this.DialogResult = DialogResult.OK;
        }

        private void ConvertExcelToJsonDBWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            groupBox1.Text = e.UserState.ToString();
            progressBar1.Value = e.ProgressPercentage;
        }
    }
}
