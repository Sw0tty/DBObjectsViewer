using System;
using Npgsql;
using System.Data;
using System.Collections.Generic;


namespace DBObjectsViewer
{
    public abstract class PostgreAdapters
    {
        /// <summary>
        /// Makes SELECT requests
        /// </summary>
        /// <returns>List of Dictionarys Selected data</returns>
        protected List<Dictionary<string, string>> SelectAdapter(string request, bool allowsNull, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            DataSet dataSet = new DataSet();
            adapter.SelectCommand = new NpgsqlCommand(request, connection);
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
                            if (allowsNull)
                            {
                                valueFromTable[columns[columnNum]] = "'null'";
                            }
                            else
                            {
                                valueFromTable[columns[columnNum]] = "''";
                            }
                        }
                        else
                        {
                            valueFromTable[columns[columnNum]] = "'" + cell.ToString() + "'";
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
        /// Makes SELECT COUNT requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int SelectCountAdapter(string request, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = new NpgsqlCommand(request, connection);
            adapter.SelectCommand.Transaction = transaction;
            NpgsqlDataReader reader = adapter.SelectCommand.ExecuteReader();
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
        protected string SelectSingleValueAdapter(string request, bool likeValue, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            NpgsqlCommand command = new NpgsqlCommand(request, connection);
            command.Transaction = transaction;
            NpgsqlDataReader reader = command.ExecuteReader();
            string valueFromDB = null;

            while (reader.Read())
            {
                if (likeValue)
                {
                    valueFromDB = "'" + reader.GetValue(0).ToString() + "'";
                }
                else
                {
                    valueFromDB = reader.GetValue(0).ToString();
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
        protected int AlterAdapter(string request, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            NpgsqlCommand command = new NpgsqlCommand(request, connection);
            command.Transaction = transaction;
            int rowsAffected = command.ExecuteNonQuery();
            command.Dispose();
            return rowsAffected;
        }

        protected int BackUpAdapter(string request, NpgsqlConnection connection)
        {
            NpgsqlCommand command = new NpgsqlCommand(request, connection);
            int rowsAffected = command.ExecuteNonQuery();
            command.Dispose();
            return rowsAffected;
        }

        /// <summary>
        /// Makes INSERT requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int InsertAdapter(string request, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            adapter.InsertCommand = new NpgsqlCommand(request, connection);
            adapter.InsertCommand.Transaction = transaction;
            int rowsAffected = adapter.InsertCommand.ExecuteNonQuery();
            adapter.Dispose();
            return rowsAffected;
        }

        /// <summary>
        /// Makes UPDATE requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int UpdateAdapter(string request, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            adapter.UpdateCommand = new NpgsqlCommand(request, connection);
            adapter.UpdateCommand.Transaction = transaction;
            int rowsAffected = adapter.UpdateCommand.ExecuteNonQuery();
            adapter.Dispose();
            return rowsAffected;
        }

        /// <summary>
        /// Makes DELTE requests
        /// </summary>
        /// <returns>Count of affected rows</returns>
        protected int DeleteAdapter(string request, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            adapter.DeleteCommand = new NpgsqlCommand(request, connection);
            adapter.DeleteCommand.Transaction = transaction;
            int rowsAffected = adapter.DeleteCommand.ExecuteNonQuery();
            adapter.Dispose();
            return rowsAffected;
        }
    }
}
