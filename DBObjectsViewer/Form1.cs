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
using RandomGame;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DBObjectsViewer.Forms;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DBObjectsViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            JSONWorker.LoadJson();
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
            string tableName = "eqCond";
            List<string> columns = new List<string>() { "IS_NULLABLE", "COLUMN_NAME", "DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH" };

            int startCol = 2;
            int endCol = 4;
            //SQLDBConnector sqlConnector = new SQLDBConnector(@"(local)\SQLEXPRESS2022", "5009_d", "sa", "123"); // Home string
            SQLDBConnector sqlConnector = new SQLDBConnector(@"(local)\SQL2022", "220", "sa", "123"); // Home string
            sqlConnector.OpenConnection();
            List<Dictionary<string, string>> tableColumnsInfo = sqlConnector.ReturnTableColumnsInfo(tableName, columns);
            sqlConnector.CloseConnection();


            Word.Application wordApp = new Word.Application();

/*            // Устанавливаем текущую программу как программу по умолчанию для открытия файлов Word
            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey("Word.Document.12\\shell\\Open\\command", true);
            if (regKey != null)
            {
                regKey.SetValue("", "\"" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + "\" \"%1\"");
                regKey.Close();
            }*/

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
                            table.Cell(row, col).Range.Text = columnData[keys[col - 1]];
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

                Console.WriteLine("Документ успешно сохранен по пути: " + filePath);
            }

            

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
    }
}
