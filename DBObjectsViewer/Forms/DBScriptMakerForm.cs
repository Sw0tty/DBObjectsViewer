using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBObjectsViewer.Forms
{
    public partial class DBScriptMakerForm : Form
    {
        protected string DataBaseType;

        public DBScriptMakerForm(string dataBaseType)
        {
            InitializeComponent();
            DataBaseType = dataBaseType;

            if (DataBaseType == AppConsts.DatabaseType.MySQL)
            {
                groupBox2.Location = new Point(13, 13);
                groupBox2.Size = new Size(321, 356);
                groupBox1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> tables = new List<string>();

            foreach (string tableName in textBox3.Text.Split('\n'))
                tables.Add(tableName.Trim().Replace("\"", ""));

            if (textBox3.Text.Trim(' ') == "" || tables.Count == 0)
                MessageBox.Show("Enter tables in column separated with new row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (groupBox1.Visible == true && textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Enter used database schema.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                textBox2.Clear();
                textBox2.Enabled = true;
                foreach (string request in AppUsedFunctions.MakeCompositeRequestsToDB(tables, DataBaseType, textBox1.Text.Trim()))
                    textBox2.Text += request + "\n";
            }
        }
    }
}
