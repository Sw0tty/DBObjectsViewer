using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
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
using Excel = Microsoft.Office.Interop.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using WordProcessing = DocumentFormat.OpenXml.Wordprocessing;
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
                /*WorkProgressForm progressForm = new WorkProgressForm(conForm.ReturnStableConnection());
                DialogResult progressResult = progressForm.ShowDialog();

                if (progressResult == DialogResult.OK)
                    MessageBox.Show("Database data successfully save.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
*/

                // -- interop excel
                /*Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = new Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>();

                Excel.Application excel = new Excel.Application();
                Excel.Workbook workbook = excel.Workbooks.Open($@"{filePath}");
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets["Лист1"];

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
                excel.Quit();*/
                // -- interop excel

                // xml
                Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = new Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>();

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
                    {
                        WorkbookPart workbookPart = doc.WorkbookPart;
                        SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        SharedStringTable sst = sstpart.SharedStringTable;

                        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                        DocumentFormat.OpenXml.Spreadsheet.Worksheet sheet = worksheetPart.Worksheet;

                        var cells = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Cell>();
                        var rows = sheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();

                        string tableName = null;
                        string tableInfoKey = null;

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
                    }
                }
                // xml



                JSONWorker.SaveJson(DBInfo, JSONWorker.MakeUniqueFileName("ExcelScan", DataBaseType), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = AppUsedFunctions.SelectFileOnPC(AppDomain.CurrentDomain.BaseDirectory, AppConsts.FileDialogSupportedFormats.JsonFormats);

            if (AppUsedFunctions.CheckSupportFormat(filePath))
            {
                // Converting in Word
                JSONWorker.LoadJson("ExcelScan_MYSQL_052524001525.json", AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

                

                // read the data from the json file
                var data = JSONWorker.MySQLDatabaseInfo;

                if (data != null)
                {
                    using (var document = WordprocessingDocument.Open(@"C:\Users\Swotty\Desktop\TestExp.docx", true))
                    {
                        if (document.MainDocumentPart is null || document.MainDocumentPart.Document.Body is null)
                        {
                            throw new ArgumentNullException("MainDocumentPart and/or Body is null.");
                        }

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
                    }
                }
            }
        }
    }
}
