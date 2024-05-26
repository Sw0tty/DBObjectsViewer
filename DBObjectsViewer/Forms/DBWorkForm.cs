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
using DocumentFormat.OpenXml.Wordprocessing;


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
                WorkProgressForm progressForm = new WorkProgressForm(conForm.ReturnStableConnection());
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
            string filePath = AppUsedFunctions.SelectFileOnPC(AppConsts.FileDialogSupportedFormats.ExcelFormats);

            if (AppUsedFunctions.CheckSupportFormat(filePath))
            {
                Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = new Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>();

                Excel.Application excel = new Excel.Application();
                Workbook workbook = excel.Workbooks.Open($@"{filePath}");
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

                JSONWorker.SaveJson(DBInfo, JSONWorker.MakeUniqueFileName("ExcelScan", DataBaseType), pathToFile: AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = AppUsedFunctions.SelectFileOnPC(AppConsts.FileDialogSupportedFormats.JsonFormats);

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

                        TableProperties props = new TableProperties(
                            new TableBorders(
                            new TopBorder
                            {
                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                Size = 12
                            },
                            new BottomBorder
                            {
                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                Size = 12
                            },
                            new LeftBorder
                            {
                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                Size = 12
                            },
                            new RightBorder
                            {
                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                Size = 12
                            },
                            new InsideHorizontalBorder
                            {
                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                Size = 12
                            },
                            new InsideVerticalBorder
                            {
                                Val = new EnumValue<BorderValues>(BorderValues.Single),
                                Size = 12
                            }));

                        table.AppendChild<TableProperties>(props);

                        for (var i = 0; i < 4; i++)
                        {
                            var tr = new TableRow();

                            // Количество ячеек в строке
                            for (var j = 0; j < 4; j++)
                            {
                                var tc = new TableCell();

                                // Добалвение данных в ячейку
                                tc.Append(new DocumentFormat.OpenXml.Wordprocessing.Paragraph(new Run(new Text("TEST"))));

                                // Assume you want columns that are automatically sized.
                                tc.Append(new TableCellProperties(
                                    new TableCellWidth { Type = TableWidthUnitValues.Auto }));

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
