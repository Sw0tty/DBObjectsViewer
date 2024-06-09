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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace DBObjectsViewer.Forms
{
    public partial class WorkProgressForm : Form
    {
        private SQLDBConnector MySQLConnection { get; set; }
        private PostgreDBConnector PostgreSQLConnection { get; set; }

        /// <summary>
        /// Make work on paramets. Make sure to give only one parameter. <br/>
        /// connection - make work with requests and converting data to json <br/>
        /// fileInfo - scan excel file with data and converting in json. Item1 filePath, Item2 DBType
        /// </summary>
        public WorkProgressForm(dynamic connection = null, Tuple<string, string> fileInfo = null)
        {
            InitializeComponent();

            if (connection is SQLDBConnector)
                MySQLConnection = connection;
            else if (connection is PostgreDBConnector)
                PostgreSQLConnection = connection;

            if (connection != null)
                ScanDatabaseBGWorker.RunWorkerAsync();

            if (fileInfo != null)
                ConvertExcelToJsonDBWorker.RunWorkerAsync(fileInfo);
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
                JSONWorker.SaveJson(DBInfo, FilesManager.MakeUniqueFileName(MySQLConnection.ReturnCatalogName(), AppConsts.DatabaseType.MySQL), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

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
                JSONWorker.SaveJson(DBInfo, FilesManager.MakeUniqueFileName(PostgreSQLConnection.ReturnDatabaseName(), AppConsts.DatabaseType.PostgreSQL), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

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
            Tuple<string, string> fileInfo = (Tuple<string, string>)e.Argument;

            Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = new Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>();

            MakeWorkerReport(worker, AppConsts.ProgressConsts.ConvertETJConsts.OpenStatus);
            using (FileStream fs = new FileStream(fileInfo.Item1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
                {
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    SharedStringTable sst = sstpart.SharedStringTable;

                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    DocumentFormat.OpenXml.Spreadsheet.Worksheet sheet = worksheetPart.Worksheet;

                    MakeWorkerReport(worker, AppConsts.ProgressConsts.ConvertETJConsts.CollectScanField);
                    var cells = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Cell>();
                    var rows = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();

                    string tableName = null;
                    string tableInfoKey = null;

                    MakeWorkerReport(worker, AppConsts.ProgressConsts.ConvertETJConsts.ScanRows);
                    foreach (DocumentFormat.OpenXml.Spreadsheet.Row row in rows)
                    {
                        Dictionary<string, string> rowInfo = new Dictionary<string, string>();
                        List<string> rowValues = new List<string>();

                        foreach (DocumentFormat.OpenXml.Spreadsheet.Cell cell in row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                        {
                            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                            {
                                int ssid = int.Parse(cell.CellValue.Text);
                                string str = sst.ChildElements[ssid].InnerText;
                                rowValues.Add(str);
                            }
                            else if (cell.CellValue != null)
                            {
                                rowValues.Add(cell.CellValue.Text);
                            }
                        }

                        if (rowValues.Count == 1 || rowValues[1].ToLower() == "null")
                        {
                            string value = rowValues[0];

                            if (!AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys.Contains(value) && rowValues[1].ToLower() == "null")
                            {
                                tableName = value;
                                DBInfo.Add(tableName, new Dictionary<string, List<Dictionary<string, string>>>());
                                continue;
                            }
                            if (AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys.Contains(value))
                            {
                                tableInfoKey = value;
                                DBInfo[tableName].Add(tableInfoKey, new List<Dictionary<string, string>>());
                                continue;
                            }
                        }

                        int nIndex = 0;
                        foreach (string value in rowValues)
                        {
                            rowInfo.Add(AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[nIndex], value);
                            nIndex++;
                        }
                        DBInfo[tableName][tableInfoKey].Add(new Dictionary<string, string>(rowInfo));
                    }
                    MakeWorkerReport(worker, AppConsts.ProgressConsts.ConvertETJConsts.CloseFile);
                }
            }

            MakeWorkerReport(worker, AppConsts.ProgressConsts.ConvertETJConsts.SaveFile);
            JSONWorker.SaveJson(DBInfo, FilesManager.MakeUniqueFileName("ExcelScan", fileInfo.Item2), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

            MakeWorkerReport(worker, AppConsts.ProgressConsts.ConvertETJConsts.DoneStatus);

            this.DialogResult = DialogResult.OK;
        }

        public string ReturnSavePath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles;
        }

        private void ConvertExcelToJsonDBWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            groupBox1.Text = e.UserState.ToString();
            progressBar1.Value = e.ProgressPercentage;
        }

        private void ConvertJsonToWordBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //
        }

        private void ConvertJsonToWordBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //
        }
    }
}
