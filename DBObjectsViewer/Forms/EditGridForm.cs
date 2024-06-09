using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Forms;


namespace DBObjectsViewer.Forms
{
    public partial class EditGridForm : Form
    {
        private static List<Tuple<string, string>> TableHeader = new List<Tuple<string, string>>();
        private static string EditMode;

        public EditGridForm(dynamic data, string editType)
        {
            EditMode = null;
            TableHeader = new List<Tuple<string, string>>();
            InitializeComponent();

            EditMode = editType;
            if (editType == AppConsts.EditTypeConsts.TableHeaderEdit)
            {
                textBox1.Visible = false;
                this.Size = new System.Drawing.Size(486, 200);

                TableHeader = data;
                dataGridView1.ColumnCount = data.Count;
                dataGridView1.RowCount = 1;

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    dataGridView1.Rows[0].Cells[i].Value = TableHeader[i].Item1;
                }
                if (data.Count < 6)
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                else
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                dataGridView1.AllowUserToAddRows = false;
            }
            else if (editType == AppConsts.EditTypeConsts.RowHeaderEdit)
            {
                textBox1.Text = data;
                dataGridView1.Visible = false;
                this.Size = new System.Drawing.Size(486, 153);
            }
        }

        public List<Tuple<string, string>> ReturnHeader()
        {
            List<Tuple<string, string>> newHeader = new List<Tuple<string, string>>();
            int col = 0;
            foreach (var item in TableHeader)
            {
                newHeader.Add(new Tuple<string, string>(dataGridView1.Rows[0].Cells[col].Value.ToString(), item.Item2));
                col++;
            }
            return newHeader;
        }

        public string ReturnRowHeader()
        {
            return textBox1.Text.Trim();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
