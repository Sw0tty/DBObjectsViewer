using Microsoft.Office.Interop.Word;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Windows.Forms;

namespace DBObjectsViewer
{
    internal class Node
    {
        /*private static string UnionCommand { get; } = "\nUNION ALL\n";

        private static string MakeHeader(string header)
        {
            //NULL AS data_type, NULL AS info, NULL AS default_value, NULL AS max_length
            string headerRow = $"SELECT '{header}' AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[0]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[1]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[2]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[3]}, NULL AS {AppConsts.DataBaseDataDeserializerConsts.ColumnsHeaders[4]}";
            *//*for (int i = 1; i < AppConsts.FieldsInfo.Count; i++)
                headerRow += (i == AppConsts.FieldsInfo.Count - 1) ? "NULL" : "NULL, ";*//*
            return headerRow;
        }

        public static List<Tuple<int, int>> SplitOnParts(int tablesCount)
        {
            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();

            if (tablesCount < AppConsts.CountOfTablesInRequest)
            {
                pairs.Add(new Tuple<int, int>(0, tablesCount));
                return pairs;
            }
            else if (tablesCount == AppConsts.CountOfTablesInRequest)
            {
                pairs.Add(new Tuple<int, int>(0, AppConsts.CountOfTablesInRequest));
                return pairs;
            }

            pairs.Add(new Tuple<int, int>(0, AppConsts.CountOfTablesInRequest));
            tablesCount -= AppConsts.CountOfTablesInRequest;
            int startPos = AppConsts.CountOfTablesInRequest;
            int endPos;

            while (tablesCount > 0)
            {
                tablesCount -= AppConsts.CountOfTablesInRequest;

                if (tablesCount < 0)
                {
                    endPos = startPos + AppConsts.CountOfTablesInRequest + tablesCount;
                }
                else
                {
                    endPos = startPos + AppConsts.CountOfTablesInRequest;
                }

                pairs.Add(new Tuple<int, int>(startPos, endPos));
                startPos = endPos;
            }
            return pairs;
        }

        public static List<string> CompositeRequestToDB(List<string> tableNames*//*, List<string> fieldsInfo*//*)
        {
            List<Tuple<int, int>> tablesPairs = SplitOnParts(tableNames.Count);
            List<string> compositeRequests = new List<string>();

            foreach (Tuple<int, int> pair in tablesPairs)
            {
                string compositeRequest = "";
                for (int i = pair.Item1; i < pair.Item2; i++)
                {
                    string tableName = tableNames[i];

                    compositeRequest += MakeHeader(tableName) + UnionCommand;
                    compositeRequest += MakeHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[0]) + UnionCommand;
                    compositeRequest += ColumnsInfo(tableName, AppConsts.FieldsInfo);

                    if (JSONWorker.TableTemplateData.AddIndexesInfo)
                    {
                        compositeRequest += UnionCommand;
                        compositeRequest += MakeHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[1]) + UnionCommand;
                        compositeRequest += TableIndexesRequest(tableName);
                    }
                    if (JSONWorker.TableTemplateData.AddForeignInfo)
                    {
                        compositeRequest += UnionCommand;
                        compositeRequest += MakeHeader(AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys[2]) + UnionCommand;
                        compositeRequest += SelectForeignKeysInfoRequest(tableName);
                    }
                    if (tableNames[pair.Item2 - 1] != tableName)
                        compositeRequest += UnionCommand;
                }
                compositeRequests.Add(compositeRequest);
            }

            return compositeRequests;
        }*/





        // for one table
        /*       use main;

               SELECT 'ColumnsInfo' as attribute, '' as data_type, '' as info, '' as default_value, '' as max_length
               UNION ALL
               SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tblFUND'
       UNION ALL

       select 'Foreigns', '', '', '', NULL
       UNION ALL

       SELECT name as key_name, 'FK',
       '(' + COL_NAME(fk_c.parent_object_id, fk_c.parent_column_id) + ') ref ' + OBJECT_NAME(fk.referenced_object_id) + ' (' + COL_NAME(fk.referenced_object_id, fk_c.referenced_column_id) + ')' AS referenced_column_name,
       NULL, NULL
       FROM sys.foreign_keys AS fk INNER JOIN sys.foreign_key_columns AS fk_c ON fk.object_id = fk_c.constraint_object_id
       WHERE OBJECT_NAME(fk.parent_object_id) = 'tblFUND'

       UNION ALL
       select 'Indexes', '', '', '', ''
       UNION ALL

       select i.[name] as index_name,
       'ON ' + substring(column_names, 1, len(column_names)-1) as [on_column],
       case when i.[type] = 1 then 'Clustered' when i.[type] = 2 then 'Non-Clustered' when i.[type] = 3 then 'XML index' when i.[type] = 4 then 'Spatial index' when i.[type] = 5 then 'Clustered columnstore index' when i.[type] = 6 then 'Nonclustered columnstore index' when i.[type] = 7 then 'Nonclustered hash index' end + ', ' + case when i.is_unique = 1 then 'Unique' else 'Not unique' end as [info],
               NULL, NULL
       from sys.objects t inner join sys.indexes i on t.object_id = i.object_id cross apply (select col.[name] + ', ' from sys.index_columns ic inner join sys.columns col on ic.object_id = col.object_id and ic.column_id = col.column_id where ic.object_id = t.object_id and ic.index_id = i.index_id order by key_ordinal for xml path ('') ) D(column_names)
       where t.is_ms_shipped<> 1 and index_id > 0 and t.[name] = 'tblFUND'*/




        // -1 == MAX
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
