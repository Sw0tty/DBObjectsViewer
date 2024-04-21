using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace DBObjectsViewer.Forms
{
    public partial class TableTemplateForm : Form
    {
        public TableTemplateForm(Dictionary<string, dynamic> tableTemplateParams)
        {
            InitializeComponent();
            LoadTableTemplate();

            StartParams = DeepClone(tableTemplateParams);
            MiddleParams = new Dictionary<string, dynamic>(tableTemplateParams);

            checkBox1.Checked = tableTemplateParams["ADD_INDEXES_INFO"];
            checkBox2.Checked = tableTemplateParams["ADD_FOREIGN_KEYS_INFO"];
            checkBox3.Checked = tableTemplateParams["ALL_ABOUT_DATA_TYPE"];

            foreach (string key in tableTemplateParams["NOT_SELECTED_COLUMNS"].Keys)
            {
                listBox1.Items.Add(tableTemplateParams["NOT_SELECTED_COLUMNS"][key]);
            }

            foreach (string key in tableTemplateParams["SELECTED_COLUMNS"].Keys)
            {
                listBox2.Items.Add(tableTemplateParams["SELECTED_COLUMNS"][key]);
            }
        }

        public static Dictionary<string, dynamic> StartParams { get; set; }
        public static Dictionary<string, dynamic> MiddleParams { get; set; }

        public static bool SavePressed = false;

        static T DeepClone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        private void LoadTableTemplate()
        {
            List<string> list = JSONWorker.TableTemplateData["TABLE_TITLE"];
            Dictionary<string, SQLFieldParams> testData = JSONWorker.SQLTestData;
            List<string> keys = new List<string>();

            foreach(string key in testData.Keys)
            {
                keys.Add(key);
            }

            dataGridView1.RowCount = testData.Count;
            dataGridView1.ColumnCount = list.Count;

            
            for (int d = 0; d < list.Count; d++)
                dataGridView1.Columns[d].HeaderText = list[d];

            
            int i, j;
            for (i = 0; i < dataGridView1.RowCount; ++i)
            {
                SQLFieldParams fieldData = testData[keys[i]];
                dataGridView1.Rows[i].Cells[0].Value = fieldData.Required;
                dataGridView1.Rows[i].Cells[1].Value = fieldData.AtributeName;
                dataGridView1.Rows[i].Cells[2].Value = fieldData.DataType + $"({fieldData.MaxLength})";
/*                for (j = 0; j < dataGridView1.ColumnCount; ++j)
                    dataGridView1.Rows[i].Cells[j].Value = 1;*/
            }
                
            
        }

        public Dictionary<string, dynamic> GetSettings()
        {
            return MiddleParams;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(listBox1.SelectedItem);

            string keyOfSwitchValue = null;

            foreach (string key in MiddleParams["NOT_SELECTED_COLUMNS"].Keys)
            {
                if (listBox1.SelectedItem.ToString() == MiddleParams["NOT_SELECTED_COLUMNS"][key])
                {
                    keyOfSwitchValue = key;
                    break;
                }
            }

            MiddleParams["SELECTED_COLUMNS"].Add(keyOfSwitchValue, MiddleParams["NOT_SELECTED_COLUMNS"][keyOfSwitchValue]);
            MiddleParams["NOT_SELECTED_COLUMNS"].Remove(keyOfSwitchValue);

            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SavePressed = true;
            if (!CheckUnsavedSettings())
                this.DialogResult = DialogResult.Yes;
            else
                this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private bool CheckUnsavedSettings()
        {
            foreach (string key in MiddleParams.Keys)
            {
                switch (MiddleParams[key].GetType().Name)
                {
                    case "Boolean":
                        if (MiddleParams[key] != StartParams[key])
                            return false;
                        break;
                    case "Dictionary`2":
                        if (MiddleParams[key].Count != StartParams[key].Count)
                            return false;
                        foreach (string insideKey in MiddleParams[key].Keys)
                        {
                            if (MiddleParams[key][insideKey] != StartParams[key][insideKey])
                                return false;
                        }
                        break;
                    default:
                        MessageBox.Show($"Тип данных '{MiddleParams[key].GetType().Name}' не определен");
                        return false;
                }
            }
            return true;
        }

        private void TableTemplateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SavePressed)
            {
                if (!CheckUnsavedSettings())
                {
                    if (MessageBox.Show("Настройки не были сохранены. Сохранить?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.Yes;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.No;
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MiddleParams["ADD_INDEXES_INFO"] = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            MiddleParams["ADD_FOREIGN_KEYS_INFO"] = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            MiddleParams["ALL_ABOUT_DATA_TYPE"] = checkBox3.Checked;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox2.SelectedItem);
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                listBox2.ClearSelected();
            }
            
            button7.Enabled = true;
            button8.Enabled = false;
        }

        private void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox1.ClearSelected();
            }

            button8.Enabled = true;
            button7.Enabled = false;
        }
    }
}
