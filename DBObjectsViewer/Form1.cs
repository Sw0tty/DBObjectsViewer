using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System.Diagnostics.Tracing;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DBObjectsViewer.Forms;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections;

namespace DBObjectsViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            JSONWorker.LoadJson();
            JSONWorker.LoadTestData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostgreDBConnector postgreConnector = new PostgreDBConnector("localhost", "5432", "postgres", "postgres", "Admin_1234");
            postgreConnector.OpenConnection();
            MessageBox.Show(postgreConnector.GetVersion());
            postgreConnector.CloseConnection();
        }

        private void MakeCellBorder(Table table, int row, int column)
        {
            table.Cell(row, column).Range.Borders[WdBorderType.wdBorderLeft].LineStyle = WdLineStyle.wdLineStyleSingle;
            table.Cell(row, column).Range.Borders[WdBorderType.wdBorderRight].LineStyle = WdLineStyle.wdLineStyleSingle;
            table.Cell(row, column).Range.Borders[WdBorderType.wdBorderTop].LineStyle = WdLineStyle.wdLineStyleSingle;
            table.Cell(row, column).Range.Borders[WdBorderType.wdBorderBottom].LineStyle = WdLineStyle.wdLineStyleSingle;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();

            
            /*
                        string tableName = "tblFUND";
                        List<string> columns = new List<string>() { "IS_NULLABLE", "COLUMN_NAME", "DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH" };

                        int startCol = 1;
                        int endCol = 3;
                        //SQLDBConnector sqlConnector = new SQLDBConnector(@"(local)\SQLEXPRESS2022", "5009_d", "sa", "123"); // Home string
                        SQLDBConnector sqlConnector = new SQLDBConnector(@"(local)\SQL2022", "220", "sa", "123"); // Home string
                        sqlConnector.OpenConnection();
                        List<Dictionary<string, string>> tableColumnsInfo = sqlConnector.ReturnTableColumnsInfo(tableName, columns);
                        sqlConnector.CloseConnection();


                        Word.Application wordApp = new Word.Application();

            *//*            // Устанавливаем текущую программу как программу по умолчанию для открытия файлов Word
                        RegistryKey regKey = Registry.ClassesRoot.OpenSubKey("Word.Document.12\\shell\\Open\\command", true);
                        if (regKey != null)
                        {
                            regKey.SetValue("", "\"" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + "\" \"%1\"");
                            regKey.Close();
                        }*//*

                        //wordApp.Visible = true;
                        Word.Document doc = wordApp.Documents.Add();
                        Word.Table table = null;
                        wordApp.Visible = true;

                        List<string> title = new List<string>() { "Признак обяз. поля", "Атрибут", "Тип данных", "Описание строки", "Содержит FK ключ" };

                        try
                        {
                            table = doc.Tables.Add(doc.Range(0, 0), 2, title.Count);
                        }
                        catch (Exception)
                        {
                            wordApp.Visible = true;
                            MessageBox.Show("Укажите 'не показывать' и попробуйте снова", "Ошибка всплывающего окна", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        if (table != null)
                        {
                            Object emptyRow = System.Reflection.Missing.Value;


                            // Заполнение таблицы данными
                            for (int rowTitle = 1; rowTitle <= title.Count; rowTitle++)
                            {
                                table.Cell(1, rowTitle).Range.Text = title[rowTitle - 1];
                                table.Cell(1, rowTitle).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; // Aligment in cell
                                table.Cell(1, rowTitle).Range.ParagraphFormat.SpaceAfter = 0.0F; // 0 space
                                table.Cell(1, rowTitle).Range.Font.Bold = 1;
                                table.Cell(1, rowTitle).Range.Font.Name = "Times New Roman";
                                table.Cell(1, rowTitle).Range.Font.Size = 12;
                                MakeCellBorder(table, 1, rowTitle);
                            }

                            List<string> keys = new List<string>();
                            foreach (string key in tableColumnsInfo[0].Keys)
                            {
                                keys.Add(key);
                            }

                            for (int row = 2; row <= tableColumnsInfo.Count + 1; row++)
                            {

                                Dictionary<string, string> columnData = tableColumnsInfo[row - 2];
                                for (int col = 1; col <= title.Count; col++)
                                {
                                    if (col >= startCol && col <= endCol)
                                    {
                                        string cellData = columnData[keys[col - 1]].Replace("\'", string.Empty);
                                        if (cellData == "NO" || cellData == "YES")
                                        {
                                            cellData = cellData == "NO" ? "*" : "";
                                        }
                                        if (col == endCol)
                                        {
                                            string maxLen = columnData[keys[col]].Replace("\'", string.Empty);
                                            if (maxLen == "-1")
                                            {
                                                cellData = $"{cellData}(MAX)";
                                            }
                                            else if (maxLen == "null")
                                            {
                                                cellData = $"{cellData}";
                                            }
                                            else if (maxLen == "")
                                            {
                                                cellData = $"{cellData}";
                                            }
                                            else
                                            {
                                                cellData = $"{cellData}({maxLen})";
                                            }

                                        }
                                        table.Cell(row, col).Range.Text = cellData;
                                    }
                                    MakeCellBorder(table, row, col);
                                }
                                if (row != title.Count)
                                    table.Rows.Add(ref emptyRow);
                            }


                            // Сохранение документа
                            //string filePath = @"C:\Users\Swotty\Desktop\test.docx"; // Home path
                            string filePath = @"C:\Users\Егор\Desktop\test.docx"; // Work path
                            doc.SaveAs2(filePath);

                            // Закрытие документа и Word
                            doc.Close();
                            wordApp.Quit();

                            //Console.WriteLine("Документ успешно сохранен по пути: " + filePath);
                        }*/



        }

        private void button3_Click(object sender, EventArgs e)
        {
            
/*            Dictionary<string, dynamic> sdf = JSONWorker.TableTemplateData;
            Dictionary<string, dynamic> newsdf = new Dictionary<string, dynamic>();
            foreach (string key in sdf.Keys)
            {
                try
                {
                    newsdf[key] = JsonSerializer.Deserialize<bool>(sdf[key].GetRawText());
                }
                catch
                {
                    newsdf[key] = JsonSerializer.Deserialize<Dictionary<string, string>>(sdf[key].GetRawText());
                }
            }*/

            TableTemplateForm settingsForm = new TableTemplateForm(JSONWorker.TableTemplateData);
            DialogResult formResult = settingsForm.ShowDialog();

            if (formResult == DialogResult.Yes)
            {
                JSONWorker.TableTemplateData = settingsForm.GetSettings();
                JSONWorker.SaveTableTemplate();
            }
            else if (formResult == DialogResult.No)
            {
                //
            }
            else if (formResult == DialogResult.Cancel)
            {
                //
            }
            /*            Dictionary<string, object> tableTemplateParams = JSONWorker.LoadJson();

                        Dictionary<string, bool> checkBoxes = new Dictionary<string, bool>();
                        Dictionary<string, Dictionary<string, string>> columnsTable = new Dictionary<string, Dictionary<string, string>>();

                        foreach (dynamic obj in tableTemplateParams)
                        {
                            if (obj.Value.GetType() == typeof(bool))
                            {
                                checkBoxes[obj.Key] = obj.Value;
                            }
                            else if (obj.Value.GetType() == typeof(Dictionary<string, string>))
                            {
                                columnsTable[obj.Key] = obj.Value;
                            }
                        }

                        JSONWorker.SaveTableTemplate(checkBoxes, columnsTable);*/

            /*if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                string newCategory = settingsForm.NewCategory();
                List<string> valuesInNewCategory = JSONWorker.ReturnCategoryValues(newCategory);
                List<string> problemValues = new List<string>();

                foreach (string value in listBox1.SelectedItems)
                {
                    if (valuesInNewCategory.Contains(value))
                        problemValues.Add(value);
                }

                if (problemValues.Count > 0)
                {
                    MessageBox.Show($"В конечной категории уже присутствуют значения: {string.Join(", ", problemValues)}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    List<string> selectedItems = new List<string>(listBox1.SelectedItems.Cast<string>().ToList());
                    foreach (string value in selectedItems)
                    {
                        JSONWorker.AddValue(newCategory, value);
                        JSONWorker.DeleteValue(comboBox1.SelectedItem.ToString(), value);
                        listBox1.Items.Remove(value);
                    }
                    button3.Enabled = false;
                    button10.Enabled = false;
                    CheckCountList();
                }
            }



            Dictionary<string, dynamic> tableParams = JSONWorker.LoadJson();
            MessageBox.Show(tableParams["ADD_INDEXES_INFO"].ToString());*/
        }

        private void MakeCellsMerge(Table table, List<int> cellsRows, int titles)
        {
            foreach (int row in cellsRows)
                table.Cell(row, 1).Merge(table.Cell(row, titles));
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string tableName = "eqCOND";
            int lastRowInserted = 0;
            
            List<string> columns = new List<string>() { "IS_NULLABLE", "COLUMN_NAME", "DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH" };
            List<string> title = new List<string>() { "Признак обяз. поля", "Атрибут", "Тип данных", "Описание строки", "Содержит FK ключ" };

            int startCol = 1;
            int endCol = 3;
            int startIndexCol = 2;
            int endIndexCol = 4;
            int startForeignCol = 2;
            int endForeignCol = 4;
            //SQLDBConnector sqlConnector = new SQLDBConnector(@"(local)\SQLEXPRESS2022", "5009_d", "sa", "123"); // Home string
            SQLDBConnector sqlConnector = new SQLDBConnector(@"(local)\SQL2022", "220", "sa", "123"); // Home string
            
            
            sqlConnector.OpenConnection();

            List<Dictionary<string, string>> tables = sqlConnector.ReturnTables();
            

            Word.Application wordApp = new Word.Application();
            Word.Document doc = wordApp.Documents.Add();
            Word.Table table = null;
            wordApp.Visible = true;

            

            foreach (Dictionary<string, string> selectedTable in tables)
            {
                List<int> cellsRows = new List<int>();
                List<Dictionary<string, string>> tableColumnsInfo = sqlConnector.ReturnTableColumnsInfo(selectedTable["TABLE_NAME"], columns);
                List<Dictionary<string, string>> tableIndexes = sqlConnector.ReturnIndexesInfo(selectedTable["TABLE_NAME"]);
                List<Dictionary<string, string>> tableForeigns = sqlConnector.ReturnForeignsInfo(selectedTable["TABLE_NAME"]);
                List<Dictionary<string, string>> tablePrimary = sqlConnector.ReturnPrimaryInfo(selectedTable["TABLE_NAME"]);
                List<string> tblPrimarys = new List<string>();

                foreach (Dictionary<string, string> primaryKey in tablePrimary)
                {
                    tblPrimarys.Add(primaryKey["Column_Name"]);
                }

                try
                {
                    Object begin1 = 0;
                    Object end1 = 0;
                    Word.Range wordrange1 = doc.Range(ref begin1, ref end1);

                    wordrange1.Text = selectedTable["TABLE_NAME"].Replace("\'", string.Empty);
                    wordrange1.FormattedText.Font.Name = "Times New Roman";
                    wordrange1.FormattedText.Font.Size = 12;
                    wordrange1.ParagraphFormat.SpaceAfter = 2.0F;
                    begin1 = 1;
                    end1 = 60;
                    table = doc.Tables.Add(doc.Range(selectedTable["TABLE_NAME"].Replace("\'", string.Empty).Length, selectedTable["TABLE_NAME"].Replace("\'", string.Empty).Length), 1, title.Count);
                    lastRowInserted += 1;
                }
                catch (Exception)
                {
                    MessageBox.Show("Укажите 'не показывать' и попробуйте снова", "Ошибка всплывающего окна", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (table != null)
                {
                    Object emptyRow = System.Reflection.Missing.Value;



                    // Заполнение таблицы данными

                    for (int rowTitle = 1; rowTitle <= title.Count; rowTitle++)
                    {
                        table.Cell(1, rowTitle).Range.Text = title[rowTitle - 1];
                        table.Cell(1, rowTitle).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; // Aligment in cell
                        table.Cell(1, rowTitle).Range.ParagraphFormat.SpaceAfter = 0.0F; // 0 space
                        table.Cell(1, rowTitle).Range.Font.Bold = 1;
                        table.Cell(1, rowTitle).Range.Font.Name = "Times New Roman";
                        table.Cell(1, rowTitle).Range.Font.Size = 12;
                        MakeCellBorder(table, 1, rowTitle);
                    }

                    List<string> keys = new List<string>();
                    foreach (string key in tableColumnsInfo[0].Keys)
                    {
                        keys.Add(key);
                    }

                    for (int row = 2; row < tableColumnsInfo.Count + 2; row++)
                    {
                        table.Rows.Add(ref emptyRow);
                        lastRowInserted++;
                        Dictionary<string, string> columnData = tableColumnsInfo[row - 2];
                        for (int col = 1; col <= title.Count; col++)
                        {
                            if (col >= startCol && col <= endCol)
                            {
                                string cellData = columnData[keys[col - 1]].Replace("\'", string.Empty);
                                if (cellData == "NO" || cellData == "YES")
                                {
                                    cellData = cellData == "NO" ? "*" : "";
                                }
                                if (col == endCol)
                                {
                                    string maxLen = columnData[keys[col]].Replace("\'", string.Empty);
                                    if (maxLen == "-1")
                                    {
                                        cellData = $"{cellData}(MAX)";
                                    }
                                    else if (maxLen == "null")
                                    {
                                        cellData = $"{cellData}";
                                    }
                                    else if (maxLen == "")
                                    {
                                        cellData = $"{cellData}";
                                    }
                                    else if (cellData == "ntext")
                                    {
                                        cellData = $"{cellData}";
                                    }
                                    else
                                    {
                                        cellData = $"{cellData}({maxLen})";
                                    }
                                }
                                table.Cell(row, col).Range.Text = cellData;
                            }
                            

                            table.Cell(row, col).Range.ParagraphFormat.SpaceAfter = 0.0F;
                            table.Cell(row, col).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; // Aligment in cell
                            table.Cell(row, col).Range.Font.Bold = 0;
                            table.Cell(row, col).Range.Font.Name = "Calibri";
                            table.Cell(row, col).Range.Font.Size = 11;
                            if (col == title.Count - 1 && tblPrimarys.Contains(columnData["COLUMN_NAME"]))
                            {
                                table.Cell(row, col).Range.Text = "PK";
                                table.Cell(row, col).Range.ParagraphFormat.SpaceAfter = 0.0F;
                                table.Cell(row, col).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; // Aligment in cell
                                table.Cell(row, col).Range.Font.Bold = 1;
                                table.Cell(row, col).Range.Font.Italic = 1;
                                table.Cell(row, col).Range.Font.Name = "Calibri";
                                table.Cell(row, col).Range.Font.Size = 11;
                            }
                            MakeCellBorder(table, row, col);
                        }
                        

                    }

                    // Indexes
                    table.Rows.Add(System.Reflection.Missing.Value);
                    lastRowInserted++;
                    table.Cell(lastRowInserted, 1).Range.Text = "Индексы";
                    cellsRows.Add(lastRowInserted);
                    //table.Cell(lastRowInserted, 1).Merge(table.Cell(lastRowInserted, title.Count));
                    table.Cell(lastRowInserted, 1).Range.ParagraphFormat.SpaceAfter = 0.0F; // 0 space
                    table.Cell(lastRowInserted, 1).Range.Font.Bold = 1;
                    table.Cell(lastRowInserted, 1).Range.Font.Italic = 0;
                    table.Cell(lastRowInserted, 1).Range.Font.Name = "Times New Roman";
                    table.Cell(lastRowInserted, 1).Range.Font.Size = 12;
                    table.Cell(lastRowInserted, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter; // Aligment in cell
                    MakeCellBorder(table, lastRowInserted, 1);

                    if (tableIndexes.Count > 0)
                    {
                        lastRowInserted++;
                        List<string> indexesKeys = new List<string>();
                        foreach (string key in tableIndexes[0].Keys)
                        {
                            indexesKeys.Add(key);
                        }
                        int rowLast = lastRowInserted;
                        for (int row = rowLast; row < rowLast + tableIndexes.Count; row++)
                        {
                            table.Rows.Add(ref emptyRow);
                            lastRowInserted++;

                            Dictionary<string, string> indexInfo = tableIndexes[row - rowLast];
                            for (int col = 1; col <= title.Count; col++)
                            {
                                if (col >= startIndexCol && col <= endIndexCol)
                                {
                                    string cellData = indexInfo[indexesKeys[col - 2]].Replace("\'", string.Empty);
                                    if (indexesKeys[col - 2] == "unique")
                                    {
                                        cellData = $"{string.Join(", ", cellData.Split())}";
                                    }
                                    else if (indexesKeys[col - 2] == "columns")
                                    {
                                        cellData = $"ON {string.Join(", ", cellData.Split())}";
                                    }

                                    table.Cell(row, col).Range.Text = cellData;
                                }
                                table.Cell(row, col).Range.ParagraphFormat.SpaceAfter = 0.0F;
                                MakeCellBorder(table, row, col);
                            }


                        }
                    }
                    else
                    {
                        table.Rows.Add(ref emptyRow);
                    }




                    // Foreigns
                    table.Rows.Add(ref emptyRow);
                    lastRowInserted++;
                    table.Cell(lastRowInserted, 1).Range.Text = "Вторичные ключи";
                    //table.Cell(lastRowInserted, 1).Merge(table.Cell(lastRowInserted, title.Count));
                    cellsRows.Add(lastRowInserted);
                    table.Cell(lastRowInserted, 1).Range.ParagraphFormat.SpaceAfter = 0.0F; // 0 space
                    table.Cell(lastRowInserted, 1).Range.Font.Bold = 1;
                    table.Cell(lastRowInserted, 1).Range.FormattedText.Italic = 0;
                    table.Cell(lastRowInserted, 1).Range.Font.Name = "Times New Roman";
                    table.Cell(lastRowInserted, 1).Range.Font.Size = 12;
                    table.Cell(lastRowInserted, 1).Range.Rows.Alignment = Word.WdRowAlignment.wdAlignRowCenter; // Aligment in cell
                    MakeCellBorder(table, lastRowInserted, 1);

                    if (tableForeigns.Count > 0)
                    {
                        lastRowInserted++;
                        List<string> foreignsKeys = new List<string>();
                        foreach (string key in tableForeigns[0].Keys)
                        {
                            foreignsKeys.Add(key);
                        }
                        int rowLast = lastRowInserted;
                        for (int row = rowLast; row < rowLast + tableForeigns.Count; row++)
                        {
                            table.Rows.Add(ref emptyRow);
                            lastRowInserted++;

                            Dictionary<string, string> foreignInfo = tableForeigns[row - rowLast];
                            for (int col = 1; col <= title.Count; col++)
                            {
                                if (col >= startForeignCol && col <= endForeignCol)
                                {
                                    string cellData = foreignInfo[foreignsKeys[col - 2]].Replace("\'", string.Empty);
                                    if (col == endForeignCol)
                                    {
                                        
                                        cellData = $"({foreignInfo["column_name"].Replace("\'", string.Empty)}) ref {foreignInfo["table_name"].Replace("\'", string.Empty)} ({foreignInfo["referenced_column_name"].Replace("\'", string.Empty)})";

                                    }
                                    else if (col == endForeignCol - 1)
                                    {
                                        cellData = "FK";
                                    }
                                    
                                    table.Cell(row, col).Range.Text = cellData;
                                    if (cellData == "FK")
                                    {
                                        table.Cell(row, col).Range.FormattedText.Bold = 1;
                                        table.Cell(row, col).Range.FormattedText.Italic = 1;
                                    }

                                }
                                table.Cell(row, col).Range.ParagraphFormat.SpaceAfter = 0.0F;
                                MakeCellBorder(table, row, col);
                            }
                        }
                    }
                    else
                    {
                        table.Rows.Add(ref emptyRow);
                    }
                    //MakeCellsMerge(table, cellsRows, title.Count);

                    Object begin = 0;
                    Object end = 0;
                    Word.Range wordrange = doc.Range(ref begin, ref end);

                    wordrange.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                }

                //Console.WriteLine("Документ успешно сохранен по пути: " + filePath);
            }
            sqlConnector.CloseConnection();

            // Сохранение документа
            //string filePath = @"C:\Users\Swotty\Desktop\test.docx"; // Home path
            string filePath = @"C:\Users\Егор\Desktop\test.docx"; // Work path
            doc.SaveAs2(filePath);

            // Закрытие документа и Word
            doc.Close();
            wordApp.Quit();
        }
    }
}
