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

namespace DBObjectsViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            SQLDBConnector sqlConnector = new SQLDBConnector(@"(local)\SQLEXPRESS2022", "5009_d", "sa", "123");
            sqlConnector.OpenConnection();
            List<Dictionary<string, string>> tableColumnsInfo = sqlConnector.ReturnTableColumnsInfo(tableName, columns);
            sqlConnector.CloseConnection();


            Word.Application wordApp = new Word.Application();
            Word.Document doc = wordApp.Documents.Add();

            List<string> title = new List<string>() { "Признак обяз. поля", "Атрибут", "Тип данных", "Описание строки", "Содержит FK ключ" };

            Word.Table table = doc.Tables.Add(doc.Range(0, 0), 2, title.Count);
            Object emptyRow = System.Reflection.Missing.Value;


            // Заполнение таблицы данными
            for (int rowTitle = 1; rowTitle <= title.Count; rowTitle++)
            {
                table.Cell(1, rowTitle).Range.Text = title[rowTitle - 1];
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
            string filePath = @"C:\Users\Swotty\Desktop\test.docx";
            doc.SaveAs2(filePath);

            // Закрытие документа и Word
            doc.Close();
            wordApp.Quit();

            Console.WriteLine("Документ успешно сохранен по пути: " + filePath);

        }
    }
}
