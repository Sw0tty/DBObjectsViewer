using System;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using WordProcessing = DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Presentation;


namespace DBObjectsViewer.Forms
{
    public partial class DBWorkForm : Form
    {
        protected string DataBaseType;

        public DBWorkForm(string databaseType)
        {
            InitializeComponent();
            DataBaseType = databaseType;
            label1.Text = $"Now working with {DataBaseType}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionForm conForm = new ConnectionForm(DataBaseType);

            this.Visible = false;
            DialogResult conResult = conForm.ShowDialog();
            this.Visible = true;

            if (conResult == DialogResult.OK)
            {
                WorkProgressForm progressForm = new WorkProgressForm(connection: conForm.ReturnStableConnection());
                DialogResult progressResult = progressForm.ShowDialog();

                if (progressResult == DialogResult.OK)
                    MessageBox.Show("Database data successfully save.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DBScriptMakerForm scriprtMaker = new DBScriptMakerForm(DataBaseType);
            scriprtMaker.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filePath = FilesManager.SelectFileOnPC(@"C:\", "Selecting an Excel file to scan", supportedFormats: AppConsts.FileDialogSupportedFormats.ExcelFormats);

            if (AppUsedFunctions.CheckSupportFormat(filePath))
            {
                WorkProgressForm progressForm = new WorkProgressForm(fileInfo: new Tuple<string, string>(filePath, DataBaseType));
                DialogResult progressResult = progressForm.ShowDialog();

                if (progressResult == DialogResult.OK)
                    MessageBox.Show($"Excel file successfully scan. File saved on path: {progressForm.ReturnSavePath()}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = FilesManager.SelectFileOnPC(AppDomain.CurrentDomain.BaseDirectory, "Selecting an JSON file to convert in Word", supportedFormats: AppConsts.FileDialogSupportedFormats.JsonFormats);

            if (!AppUsedFunctions.CheckSelectingFile(filePath, DataBaseType))
            {}
            else if (AppUsedFunctions.CheckSupportFormat(filePath))
            {
                Tuple<string, string> pathParts = AppUsedFunctions.SplitPath(filePath);

                dynamic data = JSONWorker.LoadAndReturnJSON(pathParts.Item1, pathToFile: pathParts.Item2, defAppPath: false);

                FilesManager.CheckPath(AppConsts.DirsConsts.DirectoryOfWordFormatDatabaseDataFiles);

                string uName = FilesManager.MakeUniqueFileName("WordReport", DataBaseType);
                string workPath = AppDomain.CurrentDomain.BaseDirectory + AppConsts.DirsConsts.DirectoryOfWordFormatDatabaseDataFiles + uName;
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
                                tableData[dIndex] = new List<string>() { "", obj["Attribute"], obj["Info"], obj["DataType"] };
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
                                    MergeRow(tCell);
                                }
                                else if (tableData[i][0] == AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[2])
                                {
                                    tCell.Append(AppWordProps.HorizontalCenterCellValue());
                                    tCell.Append(new Paragraph(AppWordProps.TableHeaderSpacing(), new WordProcessing.Run(new WordProcessing.Text(j < tableData[i].Count ? JSONWorker.AppSettings.ForeignsHeader : "")) { RunProperties = AppWordProps.BoldText() }));
                                    MergeRow(tCell);
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
            }
        }

        private void MergeRow(TableCell tCell)
        {
            TableCellProperties cellOneProperties = new TableCellProperties();
            cellOneProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Restart
            });

            TableCellProperties cellTwoProperties = new TableCellProperties();
            cellTwoProperties.Append(new HorizontalMerge()
            {
                Val = MergedCellValues.Continue
            });

            tCell.Append(cellOneProperties);
            tCell.Append(cellTwoProperties);
        }
    }
}
