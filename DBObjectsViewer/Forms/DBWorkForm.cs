using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using WordProcessing = DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
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
            string filePath = AppUsedFunctions.SelectFileOnPC(@"C:\", AppConsts.FileDialogSupportedFormats.ExcelFormats);

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
            BaseJsonWorker.BaseWorker.CheckPath(AppConsts.DirsConsts.DirectoryOfWordFormatDatabaseDataFiles);

            string uName = JSONWorker.MakeUniqueFileName("WordReport", DataBaseType);

            // Create a document by supplying the filepath. 
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(@"C:\Users\Swotty\Desktop\TestExp" + ".docx", WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                WordProcessing.Run run = para.AppendChild(new WordProcessing.Run());
                run.AppendChild(new WordProcessing.Text("Create text in body - CreateWordprocessingDocument"));

                run.AppendChild(new Paragraph(new WordProcessing.Run(new WordProcessing.Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit. "))));

                run.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Break() { Type = BreakValues.Page })));

                run.Append(new Paragraph(new WordProcessing.Run(new WordProcessing.Text("Lorem ipsum dolor sit amet, consectetur adipiscing elit. "))));

            }

            //string filePath = AppUsedFunctions.SelectFileOnPC(AppDomain.CurrentDomain.BaseDirectory, AppConsts.FileDialogSupportedFormats.JsonFormats);

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
