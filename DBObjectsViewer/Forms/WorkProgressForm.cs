using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using WordProcessing = DocumentFormat.OpenXml.Wordprocessing;


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
        public WorkProgressForm(dynamic connection = null, Tuple<string, string> fileInfo = null, Tuple<string, dynamic> jsonInfo = null)
        {
            InitializeComponent();

            if (connection is SQLDBConnector)
                MySQLConnection = connection;
            else if (connection is PostgreDBConnector)
                PostgreSQLConnection = connection;

            if (connection != null)
                ScanDatabaseBGWorker.RunWorkerAsync();
            else if (fileInfo != null)
                ConvertExcelToJsonDBWorker.RunWorkerAsync(fileInfo);
            else if (jsonInfo != null)
                ConvertJsonToWordBGWorker.RunWorkerAsync(jsonInfo);
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
            Tuple<string, dynamic> args = (Tuple<string, dynamic>)e.Argument;
            string workPath = args.Item1;
            dynamic data = args.Item2;

            // Create a document by supplying the filepath. 
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(workPath + ".docx", WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                WordProcessing.Run run = para.AppendChild(new WordProcessing.Run());

                foreach (string key in data.Keys)
                {
                    Dictionary<int, List<string>> tableData = new Dictionary<int, List<string>>();
                    int dIndex = 0;

                    if (JSONWorker.AppSettings.AddTableHeader)
                    {
                        List<string> tableHeader = new List<string>();
                        foreach (Tuple<string, string> headerCol in JSONWorker.AppSettings.TableTitle)
                            tableHeader.Add(headerCol.Item1);
                        tableData[dIndex] = tableHeader;
                        dIndex++;
                    }

                    foreach (Dictionary<string, string> obj in data[key].FieldsInfo)
                    {
                        List<string> fieldData = new List<string>();
                        foreach (Tuple<string, string> headerCol in JSONWorker.AppSettings.TableTitle)
                        {
                            if (headerCol.Item2 == null)
                                fieldData.Add("");
                            else
                            {
                                if (headerCol.Item2 == "Info")
                                    fieldData.Add(obj[headerCol.Item2] == AppConsts.DBConsts.RequiredInfo ? "*" : "");
                                else if (headerCol.Item2 == "DataType" && JSONWorker.AppSettings.AllAboutDataType && obj["MaxLength"].ToUpper() != "NULL")
                                {
                                    fieldData.Add(obj[headerCol.Item2] + $"({obj["MaxLength"]})");
                                }
                                else
                                    fieldData.Add(obj[headerCol.Item2]);
                            }
                        }
                        tableData[dIndex] = new List<string>(fieldData);
                        //tableData[dIndex] = new List<string>() { obj["Info"] == AppConsts.DBConsts.RequiredInfo ? "*" : "", obj["Attribute"], obj["DataType"], "" };
                        dIndex++;
                    }
                    if (JSONWorker.AppSettings.AddForeignsInfo && data[key].Foreigns != null)
                    {
                        tableData[dIndex] = new List<string>() { AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[2] };
                        dIndex++;
                        foreach (Dictionary<string, string> obj in data[key].Foreigns)
                        {
                            tableData[dIndex] = new List<string>() { "", obj["Attribute"], obj["DataType"], obj["Info"] };
                            dIndex++;
                        }
                    }
                    if (JSONWorker.AppSettings.AddIndexesInfo && data[key].Indexes != null)
                    {
                        tableData[dIndex] = new List<string>() { AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[1] };
                        dIndex++;
                        foreach (Dictionary<string, string> obj in data[key].Indexes)
                        {
                            tableData[dIndex] = new List<string>() { "", obj["Attribute"], obj["Info"], obj["DataType"] };
                            dIndex++;
                        }
                    }

                    /*                    Style style = new Style()
                                        {
                                            Type = StyleValues.Paragraph,
                                            StyleId = styleid,
                                            CustomStyle = true
                                        };
                                        StyleName styleName1 = new StyleName() { Val = "Heading1" };*/

/*                    run.AppendChild(new Paragraph());
                    run.Append(new WordProcessing.Text("Executive Summary"));
                    run.Append( new ParagraphProperties(new ParagraphStyleId() { Val = "Heading1" }));*/

                    
                    run.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Text(key))));
                    run.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Text(""))));
                    run.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Text("Краткое описание:"))));
                    run.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Text(""))));
                    WordProcessing.Table table = new WordProcessing.Table();

                    table.AppendChild<WordProcessing.TableProperties>(AppWordProps.StandartTableProps());

                    int rowsCount = tableData.Count;

                    // Количество строк
                    for (var i = 0; i < rowsCount; i++)
                    {
                        var tRow = new TableRow();

                        // Количество столбцов
                        for (var j = 0; j < JSONWorker.AppSettings.TableTitle.Count; j++)
                        {
                            var tCell = new TableCell();


                            if (tableData[i][0] == AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[1])
                            {
                                tCell.Append(AppWordProps.HorizontalCenterCellValue());
                                tCell.Append(new Paragraph(AppWordProps.TableHeaderSpacing(), new WordProcessing.Run(new WordProcessing.Text(j < tableData[i].Count ? JSONWorker.AppSettings.IndexesHeader : "")) { RunProperties = AppWordProps.BoldText() }));
                                AppWordProps.MergeRow(tCell);
                            }
                            else if (tableData[i][0] == AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[2])
                            {
                                tCell.Append(AppWordProps.HorizontalCenterCellValue());
                                tCell.Append(new Paragraph(AppWordProps.TableHeaderSpacing(), new WordProcessing.Run(new WordProcessing.Text(j < tableData[i].Count ? JSONWorker.AppSettings.ForeignsHeader : "")) { RunProperties = AppWordProps.BoldText() }));
                                AppWordProps.MergeRow(tCell);
                            }
                            else if (i == 0 && JSONWorker.AppSettings.AddTableHeader)
                            {
                                tCell.Append(AppWordProps.HorizontalCenterCellValue());
                                tCell.Append(new Paragraph(AppWordProps.TableHeaderSpacing(), new WordProcessing.Run(new WordProcessing.Text(j < tableData[i].Count ? tableData[i][j] : "")) { RunProperties = AppWordProps.BoldText() }));
                            }
                            else
                                tCell.Append(new Paragraph(AppWordProps.TableDataSpacing(), new WordProcessing.Run(new WordProcessing.Text(j < tableData[i].Count ? tableData[i][j] : ""/*j < objData.Count ? objData[j] : ""*/))));

                            tCell.Append(AppWordProps.VerticalCenterCellValue());
                            tCell.Append(AppWordProps.TableWidthOnPage());
                            tRow.Append(tCell);
                        }
                        table.Append(tRow);
                    }
                    run.Append(table);

                    run.Append(AppWordProps.PageBreak());
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void ConvertJsonToWordBGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //
        }
    }
}
