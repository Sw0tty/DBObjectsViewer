using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBObjectsViewer
{
    internal class Node
    {
            /* SQLDBConnector sqlConnector = new SQLDBConnector("(local)\\SQL2022", "211", "sa", "123");
     sqlConnector.OpenConnection();
     MessageBox.Show(sqlConnector.tableColumnsInfo().ToString());
     sqlConnector.CloseConnection();*//*

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = true;

    Word.Document doc = wordApp.Documents.Add();
            Object template = Type.Missing;
            Object newTemplate = false;
            Object documentType = Word.WdNewDocumentType.wdNewBlankDocument;
            Object visible = true;
            //Создаем документ 1
            wordApp.Documents.Add(ref template, ref newTemplate, ref documentType, ref visible);
    //Меняем шаблон
    template = @"C:\Users\Егор\Desktop\test.doc";
    //Создаем документ 2 worddocument в данном случае создаваемый объект 
    wordApp.Documents.Add(ref template, ref newTemplate, ref documentType, ref visible);

    //Таблицу вставляем в начало документа
    Object start = 0;
            Object end = 0;
            Word.Range wordrange = doc.Range(ref start, ref end);
            Object defaultTableBehavior =
               Word.WdDefaultTableBehavior.wdWord9TableBehavior;
            Object autoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitWindow;
            //Добавляем таблицу и получаем объект wordtable 
            Word.Table myTable = doc.Tables.Add(wordrange, 5, 5,
                              ref defaultTableBehavior, ref autoFitBehavior);



            //Word.Table myTable = app.ActiveDocument.Tables.Add(doc.Range(), 10, 3);

            myTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
    myTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
    // объединяем все ячейки первой строки. Внимание, в офисе нумерация начинается с 1, а не с  0, как в C#
    myTable.Rows[1].Cells.Merge();
    myTable.Rows[1].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
    myTable.Rows[1].Cells[1].Range.Text = "Первый блок";
    myTable.Rows[2].Cells[1].Range.Text = "1";
    myTable.Rows[2].Cells[2].Range.Text = "444";
    myTable.Rows[2].Cells[3].Range.Text = "123132";
    myTable.Rows[5].Cells.Merge();
    myTable.Rows[5].Cells[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
    myTable.Rows[5].Cells[1].Range.Text = "Второй блок";*/
    }
}
