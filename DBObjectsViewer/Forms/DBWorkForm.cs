using System;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using WordProcessing = DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;


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
                //JSONWorker.LoadJson(pathParts.Item1, pathToFile: pathParts.Item2, defAppPath: false);
                dynamic data = JSONWorker.LoadAndReturnJSON(pathParts.Item1, pathToFile: pathParts.Item2, defAppPath: false);

                /*if (pathParts.Item1.Contains(AppConsts.DatabaseType.MySQL))
                    data = JSONWorker.MySQLDatabaseInfo;
                else if (pathParts.Item1.Contains(AppConsts.DatabaseType.PostgreSQL))
                    data = JSONWorker.PostgreSQLDatabaseInfo;*/

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

                    // 1 - header
                    bool addHeader = true;

                    foreach (string key in data.Keys)
                    {
                        Dictionary<int, List<string>> tableData = new Dictionary<int, List<string>>();
                        int dIndex = addHeader ? 1 : 0;

                        foreach (Dictionary<string, string> obj in data[key].FieldsInfo)
                        {
                            tableData[dIndex] = new List<string>() { obj["Info"] == "NO" ? "*" : "", obj["Attribute"], obj["DataType"] };
                            dIndex++;
                        }
                        if (JSONWorker.AppSettings.AddForeignsInfo && data[key].Foreigns != null)
                        {
                            tableData[dIndex] = new List<string>() { AppConsts.FieldsInfo[2] };
                            dIndex++;
                            foreach (Dictionary<string, string> obj in data[key].Foreigns)
                            {
                                tableData[dIndex] = new List<string>() { "", obj["Attribute"], obj["Info"], obj["DataType"] };
                                dIndex++;
                            }
                        }
                        if (JSONWorker.AppSettings.AddIndexesInfo && data[key].Indexes != null)
                        {
                            tableData[dIndex] = new List<string>() { AppConsts.FieldsInfo[1] };
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
                        UInt32Value tBorderSize = 1;
                        TableProperties props = new TableProperties(
                             new TableBorders(
                                 new WordProcessing.TopBorder // верх таблицы (строки, если одна)
                                 {
                                     Val = new EnumValue<BorderValues>(BorderValues.Single),
                                     Size = tBorderSize
                                 },
                                 new WordProcessing.BottomBorder
                                 {
                                     Val = new EnumValue<BorderValues>(BorderValues.Single),
                                     Size = tBorderSize
                                 },
                                 new WordProcessing.LeftBorder
                                 {
                                     Val = new EnumValue<BorderValues>(BorderValues.Single),
                                     Size = tBorderSize
                                 },
                                 new WordProcessing.RightBorder
                                 {
                                     Val = new EnumValue<BorderValues>(BorderValues.Single),
                                     Size = tBorderSize
                                 },
                                 new InsideHorizontalBorder
                                 {
                                     Val = new EnumValue<BorderValues>(BorderValues.Single),
                                     Size = tBorderSize
                                 },
                                 new InsideVerticalBorder
                                 {
                                     Val = new EnumValue<BorderValues>(BorderValues.Single),
                                     Size = tBorderSize
                                 }
                             ),
/*                             new WordProcessing.GrowAutofit { Val },*/
                             new TableLayout { Type = TableLayoutValues.Autofit },
                             new TableWidth { Type = TableWidthUnitValues.Auto }
                             );

                        table.AppendChild<WordProcessing.TableProperties>(props);

                        // Add header


                        List<Tuple<string, string>> tableHeader = JSONWorker.AppSettings.TableTitle;

                        int rowsForFieldsInfo = data[key].FieldsInfo.Count;
                        int rowsForForeigns = JSONWorker.AppSettings.AddForeignsInfo && data[key].Foreigns != null ? 1 + data[key].Foreigns.Count : 0;
                        int rowsForIndexes = JSONWorker.AppSettings.AddIndexesInfo && data[key].Indexes != null ? 1 + data[key].Indexes.Count : 0;
                        int rowsCount = (addHeader ? 1 : 0) + rowsForFieldsInfo + rowsForIndexes + rowsForForeigns;


                        // Количество строк
                        for (var i = 0; i < rowsCount; i++)
                        {
                            var tRow = new WordProcessing.TableRow();
/*                            List<string> objData = new List<string>();

                            if (addHeader && i == 0)
                            {

                            }
                            else
                            {


                                if (i - (addHeader ? 1 : 0) < rowsForFieldsInfo)
                                {
                                    objData.Add(data[key].FieldsInfo[i - (addHeader ? 1 : 0)]["Info"] == "NO" ? "*" : "");
                                    objData.Add(data[key].FieldsInfo[i - (addHeader ? 1 : 0)]["Attribute"]);
                                    objData.Add(data[key].FieldsInfo[i - (addHeader ? 1 : 0)]["DataType"]);
                                }
*//*                                else if (rowsForForeigns != 0 && i - (addHeader ? 1 : 0) < rowsForFieldsInfo + rowsForForeigns)
                                {
                                    objData.Add(data[key].Foreigns[i - (addHeader ? 1 : 0)]["Info"]);
                                    objData.Add(data[key].Foreigns[i - (addHeader ? 1 : 0)]["Attribute"]);
                                    objData.Add(data[key].Foreigns[i - (addHeader ? 1 : 0)]["DataType"]);
                                    objData.Add(data[key].Foreigns[i - (addHeader ? 1 : 0)]["Info"]);
                                }*//*

                            }*/
                                

                            // Количество столбцов
                            for (var j = 0; j < tableHeader.Count; j++)
                            {
                                var tCell = new TableCell();
                            
                                if (addHeader && i == 0)
                                    tCell.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Text(tableHeader[j].Item1))));
                                else // Добалвение данных в ячейку
                                {
                                    //Paragraph pp = new Paragraph(new ParagraphProperties(new Justification() { Val = JustificationValues.Center}), new WordProcessing.Run(new WordProcessing.Text(j < tableData[i].Count ? tableData[i][j] : "")));
                                    ParagraphProperties centerP = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });

                                    if (tableData[i][j] == AppConsts.FieldsInfo[1])
                                    {
                                        tCell.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Text(j < tableData[i].Count ? tableData[i][j] : ""))));
                                    }
                                    else if (tableData[i][j] == AppConsts.FieldsInfo[2])
                                    {

                                    }
                                    tCell.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Text(j < tableData[i].Count ? tableData[i][j] : ""/*j < objData.Count ? objData[j] : ""*/))));
                                    /*if (objData.Count > 0)
                                        tc.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new WordProcessing.Run(new WordProcessing.Text(tableData[i][j]*//*j < objData.Count ? objData[j] : ""*//*))));
                                    else
                                        tc.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new WordProcessing.Run(new WordProcessing.Text("TEST"))));*/
                                }


                                // Assume you want columns that are automatically sized.
                                /*tCell.Append(new TableCellProperties(
                                    new TableCellWidth { Type = TableWidthUnitValues.Auto }));*/
                                tCell.Append(new TableCellProperties(
                                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "9999" }));

                                tRow.Append(tCell);
                            }
                            table.Append(tRow);
                        }
                        run.Append(table);

                        run.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Break() { Type = BreakValues.Page })));
                    }


                    /*run.AppendChild(new WordProcessing.Text("Create text in body - CreateWordprocessingDocument"));

                    run.AppendChild(new Paragraph(new WordProcessing.Run(new WordProcessing.Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit. "))));

                    run.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Break() { Type = BreakValues.Page })));

                    run.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit. "))));
*/
                }
            }

            

            

            /*if (AppUsedFunctions.CheckSupportFormat(filePath))
            {
                // Converting in Word
                Tuple<string, string> pathParts = AppUsedFunctions.SplitPath(filePath);

                JSONWorker.LoadJson(pathParts.Item1, pathToFile: pathParts.Item2, defAppPath: false);


                // read the data from the json file
                var data = JSONWorker.MySQLDatabaseInfo;

                if (data != null)
                {
                    // Create Document
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(@"C:\Users\Swotty\Desktop\TestExp.docx", WordprocessingDocumentType.Document, true))
                    {
                        // Add a main document part. 
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        // Create the document structure and add some text.
                        mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                        DocumentFormat.OpenXml.Wordprocessing.Body docBody = new DocumentFormat.OpenXml.Wordprocessing.Body();
                        // Add your docx content here

                        docBody.Append(new WordProcessing.Paragraph(new Run(new WordProcessing.Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent quam augue, tempus id metus in, laoreet viverra quam. Sed vulputate risus lacus, et dapibus orci porttitor non."))));




                        // Документ для записи
                        //var doc = document.MainDocumentPart.Document;

                        // Таблица с данными
                        *//* DocumentFormat.OpenXml.Wordprocessing.Table table = new DocumentFormat.OpenXml.Wordprocessing.Table();

                         WordProcessing.TableProperties props = new WordProcessing.TableProperties(
                             new WordProcessing.TableBorders(
                             new WordProcessing.TopBorder
                             {
                                 Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                 Size = 12
                             },
                             new WordProcessing.BottomBorder
                             {
                                 Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                 Size = 12
                             },
                             new WordProcessing.LeftBorder
                             {
                                 Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                 Size = 12
                             },
                             new WordProcessing.RightBorder
                             {
                                 Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                 Size = 12
                             },
                             new WordProcessing.InsideHorizontalBorder
                             {
                                 Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                 Size = 12
                             },
                             new WordProcessing.InsideVerticalBorder
                             {
                                 Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                 Size = 12
                             }));

                         table.AppendChild<WordProcessing.TableProperties>(props);

                         for (var i = 0; i < 4; i++)
                         {
                             var tr = new WordProcessing.TableRow();

                             // Количество ячеек в строке
                             for (var j = 0; j < 4; j++)
                             {
                                 var tc = new WordProcessing.TableCell();

                                 // Добалвение данных в ячейку
                                 tc.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("TEST"))));

                                 // Assume you want columns that are automatically sized.
                                 tc.Append(new WordProcessing.TableCellProperties(
                                     new WordProcessing.TableCellWidth { Type = WordProcessing.TableWidthUnitValues.Auto }));

                                 tr.Append(tc);
                             }
                             table.Append(tr);
                         }
                         docBody.Append(table);
                         mainPart.Document.Save();*//*
                        mainPart.Document.MainDocumentPart.Document.Save();
                    }


                    *//*using (var document = WordprocessingDocument.Open(@"C:\Users\Swotty\Desktop\TestExp.docx", true))
                    {
                        *//*if (document.MainDocumentPart is null || document.MainDocumentPart.Document.Body is null)
                        {
                            throw new ArgumentNullException("MainDocumentPart and/or Body is null.");
                        }*//*

                        // Документ для записи
                        var doc = document.MainDocumentPart.Document;

                        // Таблица с данными
                        DocumentFormat.OpenXml.Wordprocessing.Table table = new DocumentFormat.OpenXml.Wordprocessing.Table();

                        WordProcessing.TableProperties props = new WordProcessing.TableProperties(
                            new WordProcessing.TableBorders(
                            new WordProcessing.TopBorder
                            {
                                Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                Size = 12
                            },
                            new WordProcessing.BottomBorder
                            {
                                Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                Size = 12
                            },
                            new WordProcessing.LeftBorder
                            {
                                Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                Size = 12
                            },
                            new WordProcessing.RightBorder
                            {
                                Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                Size = 12
                            },
                            new WordProcessing.InsideHorizontalBorder
                            {
                                Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                Size = 12
                            },
                            new WordProcessing.InsideVerticalBorder
                            {
                                Val = new EnumValue<WordProcessing.BorderValues>(WordProcessing.BorderValues.Single),
                                Size = 12
                            }));

                        table.AppendChild<WordProcessing.TableProperties>(props);

                        for (var i = 0; i < 4; i++)
                        {
                            var tr = new WordProcessing.TableRow();

                            // Количество ячеек в строке
                            for (var j = 0; j < 4; j++)
                            {
                                var tc = new WordProcessing.TableCell();

                                // Добалвение данных в ячейку
                                tc.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("TEST"))));

                                // Assume you want columns that are automatically sized.
                                tc.Append(new WordProcessing.TableCellProperties(
                                    new WordProcessing.TableCellWidth { Type = WordProcessing.TableWidthUnitValues.Auto }));

                                tr.Append(tc);
                            }
                            table.Append(tr);
                        }
                        doc.Body.Append(table);
                        doc.Save();
                    }*//*
                }
            }*/
        }
    }
}
