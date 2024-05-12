using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace DBObjectsViewer
{
    public abstract class SQLAdapters
    {

        /// <summary>
        /// Makes SELECT requests
        /// 1. returnsNull - if true convert NULL in 'null'<br/>
        /// 2. removeEscapes - if true convert "'value'" in "value" <br/>
        /// </summary>
        /// <returns>List of Dictionarys Selected data</returns>
        protected List<Dictionary<string, string>> SelectAdapter(string request, bool returnsNull, bool removeEscapes, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            adapter.SelectCommand = new SqlCommand(request, connection);
            adapter.SelectCommand.Transaction = transaction;
            List<Dictionary<string, string>> selectedData = new List<Dictionary<string, string>>();
            adapter.Fill(dataSet);

            foreach (DataTable table in dataSet.Tables)
            {
                List<string> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                }

                foreach (DataRow row in table.Rows)
                {
                    var cells = row.ItemArray;
                    Dictionary<string, string> valueFromTable = new Dictionary<string, string>();
                    int columnNum = 0;

                    foreach (object cell in cells)
                    {
                        if (cell.ToString().Trim(' ') == "")
                        {
                            if (returnsNull && removeEscapes)
                            {
                                valueFromTable[columns[columnNum]] = "null";
                            }
                            else if (returnsNull && !removeEscapes)
                            {
                                valueFromTable[columns[columnNum]] = "'null'";
                            }
                            else if (!returnsNull && !removeEscapes)
                            {
                                valueFromTable[columns[columnNum]] = "''";
                            }
                            else
                            {
                                valueFromTable[columns[columnNum]] = "";
                            }
                        }
                        else
                        {
                            if (removeEscapes)
                            {
                                valueFromTable[columns[columnNum]] = cell.ToString();
                            }
                            else
                            {
                                valueFromTable[columns[columnNum]] = "'" + cell.ToString() + "'";
                            }
                        }
                        columnNum++;
                    }
                    selectedData.Add(new Dictionary<string, string>(valueFromTable));
                }
            }
            adapter.Dispose();
            return selectedData;
        }

        /// <summary>
        /// Makes SELECT requests
        /// 1. returnsNull - if true convert NULL in 'null'<br/>
        /// 2. removeEscapes - if true convert "'value'" in "value" <br/>
        /// </summary>
        /// <returns>List of Strings Selected data</returns>
        protected List<string> SelectListAdapter(string request, bool returnsNull, bool removeEscapes, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(request, connection);
            adapter.SelectCommand.Transaction = transaction;
            SqlDataReader reader = adapter.SelectCommand.ExecuteReader();
            List<string> selectedData = new List<string>();

            while (reader.Read())
            {
                string value = reader.GetSqlValue(0).ToString().Trim(' ');
                
                if (value == "")
                {
                    if (returnsNull && removeEscapes)
                    {
                        selectedData.Add("null");
                    }
                    else if (returnsNull && !removeEscapes)
                    {
                        selectedData.Add("'null'");
                    }
                    else if (!returnsNull && removeEscapes)
                    {
                        selectedData.Add("");
                    }
                    else if (!returnsNull && !removeEscapes)
                    {
                        selectedData.Add("''");
                    }
                }
                else
                {
                    if (removeEscapes)
                    {
                        selectedData.Add(value);
                    }
                    else
                        selectedData.Add("'" + value + "'");
                }
            }

            reader.Close();
            adapter.Dispose();
            return selectedData;
        }

        /// <summary>
        /// Makes SELECT COUNT requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int SelectCountAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(request, connection);
            adapter.SelectCommand.Transaction = transaction;
            SqlDataReader reader = adapter.SelectCommand.ExecuteReader();
            reader.Read();
            int count = Convert.ToInt32(reader.GetValue(0));
            reader.Close();
            adapter.Dispose();
            return count;
        }

        /// <summary>
        /// Makes SELECT SINGLE VALUE requests
        /// </summary>
        /// <param name="likeValue">Escapes the string if true</param>
        /// <returns>String value</returns>
        protected string SelectSingleValueAdapter(string request, bool likeValue, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            SqlDataReader reader = command.ExecuteReader();
            string valueFromDB = null;

            while (reader.Read())
            {
                if (likeValue)
                {
                    valueFromDB = "'" + reader.GetSqlValue(0).ToString() + "'";
                }
                else
                {
                    valueFromDB = reader.GetSqlValue(0).ToString();
                }
            }
            reader.Close();
            command.Dispose();
            return valueFromDB;
        }

        /// <summary>
        /// Makes MODIFICATION/MANIPULATION catalog requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int AlterAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand(request, connection);
            command.Transaction = transaction;
            int rowsAffected = command.ExecuteNonQuery();
            command.Dispose();
            return rowsAffected;
        }

        protected int BackUpAdapter(string request, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(request, connection);
            int rowsAffected = command.ExecuteNonQuery();
            command.Dispose();
            return rowsAffected;
        }

        /// <summary>
        /// Makes INSERT requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int InsertAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = new SqlCommand(request, connection);
            adapter.InsertCommand.Transaction = transaction;
            int rowsAffected = adapter.InsertCommand.ExecuteNonQuery();
            adapter.Dispose();
            return rowsAffected;
        }

        /// <summary>
        /// Makes UPDATE requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int UpdateAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = new SqlCommand(request, connection);
            adapter.UpdateCommand.Transaction = transaction;
            int rowsAffected = adapter.UpdateCommand.ExecuteNonQuery();
            adapter.Dispose();
            return rowsAffected;
        }

        /// <summary>
        /// Makes DELTE requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int DeleteAdapter(string request, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.DeleteCommand = new SqlCommand(request, connection);
            adapter.DeleteCommand.Transaction = transaction;
            int rowsAffected = adapter.DeleteCommand.ExecuteNonQuery();
            adapter.Dispose();
            return rowsAffected;
        }

        protected Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> SelectCompositeDictAdapter(string request, List<string> tables, bool returnsNull, bool removeEscapes, SqlConnection connection, SqlTransaction transaction)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            adapter.SelectCommand = new SqlCommand(request, connection);
            adapter.SelectCommand.Transaction = transaction;
            Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> selectedData = new Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>();
            adapter.Fill(dataSet);

            foreach (DataTable table in dataSet.Tables)
            {
                List<string> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                }

                string tableName = null;
                string tableInfoKey = null;
                foreach (DataRow row in table.Rows)
                {
                    var cells = row.ItemArray;
                    //Dictionary<string, string> valueFromTable = new Dictionary<string, string>();
                    int columnNum = 0;

                    if (tables.Contains(cells[0].ToString()))
                    {
                        tableName = cells[0].ToString();
                        selectedData.Add(tableName, new Dictionary<string, List<Dictionary<string, string>>>());
                        continue;
                    }
                    if (AppConsts.DataBaseDataDeserializerConsts.TableInfoKeys.Contains(cells[0].ToString()))
                    {
                        tableInfoKey = cells[0].ToString();
                        selectedData[tableName].Add(tableInfoKey, new List<Dictionary<string, string>>());
                        continue;
                    }

                    Dictionary<string, string> rowInfo = new Dictionary<string, string>();
                    foreach (object cell in cells)
                    {
                        
                        if (cell.ToString().Trim(' ') == "")
                        {
                            if (returnsNull && removeEscapes)
                            {
                                rowInfo.Add(columns[columnNum], "null");
                            }
                            else if (returnsNull && !removeEscapes)
                            {
                                rowInfo.Add(columns[columnNum], "'null'");
                            }
                            else if (!returnsNull && removeEscapes)
                            {
                                rowInfo.Add(columns[columnNum], "");
                            }
                            else if (!returnsNull && !removeEscapes)
                            {
                                rowInfo.Add(columns[columnNum], "''");
                            }
                        }
                        else
                        {
                            if (removeEscapes)
                                rowInfo.Add(columns[columnNum], cell.ToString().Trim(' '));
                            else
                                rowInfo.Add(columns[columnNum], "'" + cell.ToString().Trim(' ') + "'");
                        }
                        columnNum++;
                    }
                    selectedData[tableName][tableInfoKey].Add(new Dictionary<string, string>(rowInfo));
                }
            }
            adapter.Dispose();

            return selectedData;
        }
    }
}
