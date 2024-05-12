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
using System.Reflection;
using DBObjectsViewer.Properties;
using System.Runtime;

namespace DBObjectsViewer.Forms
{
    public partial class TableTemplateForm : Form
    {
        public TableTemplateForm()
        {
            InitializeComponent();
            PageLoading = true;
            checkBox1.Checked = JSONWorker.TableTemplateData.AddIndexesInfo;
            checkBox2.Checked = JSONWorker.TableTemplateData.AddForeignInfo;
            checkBox3.Checked = JSONWorker.TableTemplateData.AllAboutDataType;

            SettingsCopy.NotSelectedColumns = new Dictionary<string, string>(JSONWorker.TableTemplateData.NotSelectedColumns);
            SettingsCopy.SelectedColumns = new Dictionary<string, string>(JSONWorker.TableTemplateData.SelectedColumns);
            SettingsCopy.TableTitle = new List<Tuple<string, string>>(JSONWorker.TableTemplateData.TableTitle);
            SettingsCopy.IndexParamsColumnsNum = new Dictionary<string, int>(JSONWorker.TableTemplateData.IndexParamsColumnsNum);

            foreach (string key in JSONWorker.TableTemplateData.NotSelectedColumns.Keys)
            {
                listBox1.Items.Add(JSONWorker.TableTemplateData.NotSelectedColumns[key]);
            }

            foreach (string key in JSONWorker.TableTemplateData.SelectedColumns.Keys)
            {
                listBox2.Items.Add(JSONWorker.TableTemplateData.SelectedColumns[key]);
            }

            foreach (string key in AppConsts.Types.Keys)
            {
                comboBox3.Items.Add(AppConsts.Types[key]);
            }
            comboBox3.Items.Add(AppConsts.NoneType);

            SavePressed = false;
            CancelPressed = false;

            LoadComboBox();
            LoadTableTemplate();
            PageLoading = false;
        }

        public static Deserializers.TableTemplate SettingsCopy { get; set; } = new Deserializers.TableTemplate();

        private static bool SavePressed { get; set; }
        private static bool CancelPressed { get; set; }
        private static bool PageLoading { get; set; }
        private static Dictionary<int, string> TableColumns { get; set;} = new Dictionary<int, string>();

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

        private Deserializers.TableTemplate CopySettings(Deserializers.TableTemplate settings)
        {
            Deserializers.TableTemplate settingsCopy = new Deserializers.TableTemplate();

            settingsCopy.AddIndexesInfo = settings.AddIndexesInfo;
            settingsCopy.AddForeignInfo = settings.AddForeignInfo;
            settingsCopy.AllAboutDataType = settings.AllAboutDataType;
            settingsCopy.SelectedColumns = new Dictionary<string, string>(settings.SelectedColumns);
            settingsCopy.NotSelectedColumns = new Dictionary<string, string>(settings.NotSelectedColumns);
            settingsCopy.TableTitle = new List<Tuple<string, string>>(settings.TableTitle);
            settingsCopy.IndexParamsColumnsNum = new Dictionary<string, int>(settings.IndexParamsColumnsNum);

            return settingsCopy;
        }

        private void LoadComboBox(int selectedIndex = -1)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            foreach (Tuple<string, string> value in SettingsCopy.TableTitle)
            {
                comboBox1.Items.Add(value.Item1);
                comboBox2.Items.Add(value.Item1);
            }

            if (selectedIndex != -1)
                comboBox1.SelectedIndex = selectedIndex;
        }

        private void LoadTableTemplate()
        {
            dataGridView1.Rows.Clear();

            if (SettingsCopy.TableTitle.Count < 6)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            else
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            List<Tuple<string, string>> list = SettingsCopy.TableTitle;
            Dictionary<string, Deserializers.TestTableFields> testData = JSONWorker.SQLTestData;
            Dictionary<string, Deserializers.TestIndexes> testDataIndexes = JSONWorker.SQLTestIndexes;
            Dictionary<string, Deserializers.TestForeigns> testDataForeigns = JSONWorker.SQLTestForeigns;
            List<string> keys = new List<string>();
            List<string> indexKeys = new List<string>();
            List<string> foreignKeys = new List<string>();
            foreach (string key in testData.Keys)
            {
                keys.Add(key);
            }

            foreach (string key in testDataIndexes.Keys)
            {
                indexKeys.Add(key);
            }

            foreach (string key in testDataForeigns.Keys)
            {
                foreignKeys.Add(key);
            }

            dataGridView1.ColumnCount = list.Count;

            TableColumns.Clear();
            for (int d = 0; d < list.Count; d++)
            {
                dataGridView1.Columns[d].HeaderText = list[d].Item1;
                TableColumns[d] = list[d].Item2;
            }
            
            // Выгрузка инфы по полям
            for (int i = 0; i < testData.Count; ++i)
            {
                Deserializers.TestTableFields fieldData = testData[keys[i]];

                dataGridView1.RowCount++;
                foreach (int key in TableColumns.Keys)
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[key].Value = GetValueForField(TableColumns[key], fieldData);
            }

            // Выгрузка инфы по индексам
            if (SettingsCopy.AddIndexesInfo)
            {
                dataGridView1.RowCount++;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = "Индексы";

                for (int i = 0; i < testDataIndexes.Count; ++i)
                {
                    Deserializers.TestIndexes indexData = testDataIndexes[indexKeys[i]];

                    dataGridView1.RowCount++;
                    foreach (int key in TableColumns.Keys)
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[key].Value = GetValueForIndex(key, indexData);
                }
            }
            
            // Выгрузка инфы по вторичным ключам
            if (SettingsCopy.AddForeignInfo)
            {
                dataGridView1.RowCount++;
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = "Внешние ключи";

                for (int i = 0; i < testDataForeigns.Count; ++i)
                {
                    Deserializers.TestForeigns foreignData = testDataForeigns[foreignKeys[i]];

                    dataGridView1.RowCount++;
                    foreach (int key in TableColumns.Keys)
                        dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[key].Value = GetValueForForeign(key, foreignData);
                }
            }
        }

        private dynamic GetValueForField(string typeOfInfo, Deserializers.TestTableFields fieldData)
        {
            switch (typeOfInfo)
            {
                case "required":
                    return fieldData.Required;
                case "name":
                    return fieldData.AtributeName;
                case "data_type":
                    if (checkBox3.Checked)
                    {
                        if (fieldData.MaxLength == -1)
                            return fieldData.DataType + "(MAX)";
                        if (fieldData.MaxLength == 0)
                            return fieldData.DataType;
                        return fieldData.DataType + $"({fieldData.MaxLength})";
                    }   
                    return fieldData.DataType;
            }
            return "";
        }

        private dynamic GetValueForIndex(int column, Deserializers.TestIndexes indexData)
        {
            switch (column)
            {
                case 1:
                    return indexData.Name;
                case 2:
                    return "ON " + indexData.IndexedColumn;
                case 3:
                    return string.Join(", ", indexData.Info);
            }
            return "";
        }

        private dynamic GetValueForForeign(int column, Deserializers.TestForeigns foreignData)
        {
            switch (column)
            {
                case 1:
                    return foreignData.Name;
                case 2:
                    return foreignData.Description;
                case 3:
                    return $"({foreignData.Column}) ref {foreignData.RefTable} ({foreignData.RefTableColumn})";
            }
            return "";
        }

        private string GetFieldType(string nameOf)
        {
            foreach (string key in AppConsts.Types.Keys)
            {
                if (nameOf == AppConsts.Types[key])
                    return key;
            }
            return null;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SavePressed = true;

            if (CheckUnsavedSettings())
            {
                JSONWorker.TableTemplateData = CopySettings(SettingsCopy);
                this.DialogResult = DialogResult.Yes;
            }
            else
                this.DialogResult = DialogResult.No;

            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CancelPressed = true;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string newColumn = textBox1.Text.Trim();
            if (newColumn != "" && comboBox3.SelectedItem != null)
            {
                comboBox1.Items.Add(newColumn);
                comboBox2.Items.Add(newColumn);
                SettingsCopy.TableTitle.Add(new Tuple<string, string>(newColumn, comboBox3.SelectedItem.ToString() == AppConsts.NoneType ? null : GetFieldType(comboBox3.SelectedItem.ToString())));
                LoadTableTemplate();
            }
            else
            {
                MessageBox.Show("Пустое значение не допустимо!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            comboBox3.SelectedItem = null;
            textBox1.Text = "";
        }

        private bool CheckDictionary(Dictionary<string, string> firstDict, Dictionary<string, string> secondDict)
        {
            foreach (string key in firstDict.Keys)
                if (!secondDict.ContainsKey(key))
                    return true;
            return false;
        }

        private bool CheckListOfTuples(List<Tuple<string, string>> firstList, List<Tuple<string, string>> secondList)
        {
            if (firstList.Count != secondList.Count)
                return true;

            for (int tupleIndex = 0; tupleIndex < firstList.Count; tupleIndex++)
            {
                if (firstList[tupleIndex].Item1 != secondList[tupleIndex].Item1)
                    return true;
                if (firstList[tupleIndex].Item2 != secondList[tupleIndex].Item2)
                    return true;
            }
            return false;
        }

        private bool CheckUnsavedSettings()
        {
            if (JSONWorker.TableTemplateData.AddIndexesInfo != SettingsCopy.AddIndexesInfo)
                return true;
            else if (JSONWorker.TableTemplateData.AddForeignInfo != SettingsCopy.AddForeignInfo)
                return true;
            else if (JSONWorker.TableTemplateData.AllAboutDataType != SettingsCopy.AllAboutDataType)
                return true;
            else if (CheckDictionary(JSONWorker.TableTemplateData.NotSelectedColumns, SettingsCopy.NotSelectedColumns))
                return true;
            else if (CheckDictionary(JSONWorker.TableTemplateData.SelectedColumns, SettingsCopy.SelectedColumns))
                return true;
            else if (CheckListOfTuples(JSONWorker.TableTemplateData.TableTitle, SettingsCopy.TableTitle))
                return true;
            return false;
        }

        private void TableTemplateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CancelPressed)
            {
                if (!SavePressed)
                {
                    if (CheckUnsavedSettings())
                    {
                        if (MessageBox.Show("Настройки не были сохранены. Сохранить?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            JSONWorker.TableTemplateData = CopySettings(SettingsCopy);
                            this.DialogResult = DialogResult.Yes;
                        }
                        else
                        {
                            this.DialogResult = DialogResult.No;
                        }
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SettingsCopy.AddIndexesInfo = checkBox1.Checked;
            if (!PageLoading)
                LoadTableTemplate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SettingsCopy.AddForeignInfo = checkBox2.Checked;
            if (!PageLoading)
                LoadTableTemplate();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SettingsCopy.AllAboutDataType = checkBox3.Checked;
            if (!PageLoading)
                LoadTableTemplate();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(listBox1.SelectedItem);

            string keyOfSwitchValue = null;

            foreach (string key in SettingsCopy.NotSelectedColumns.Keys)
            {
                if (listBox1.SelectedItem.ToString() == SettingsCopy.NotSelectedColumns[key])
                {
                    keyOfSwitchValue = key;
                    break;
                }
            }

            SettingsCopy.SelectedColumns.Add(keyOfSwitchValue, SettingsCopy.NotSelectedColumns[keyOfSwitchValue]);
            SettingsCopy.NotSelectedColumns.Remove(keyOfSwitchValue);

            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox2.SelectedItem);

            string keyOfSwitchValue = null;

            foreach (string key in SettingsCopy.SelectedColumns.Keys)
            {
                if (listBox2.SelectedItem.ToString() == SettingsCopy.SelectedColumns[key])
                {
                    keyOfSwitchValue = key;
                    break;
                }
            }

            SettingsCopy.NotSelectedColumns.Add(keyOfSwitchValue, SettingsCopy.SelectedColumns[keyOfSwitchValue]);
            SettingsCopy.SelectedColumns.Remove(keyOfSwitchValue);

            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox2.ClearSelected();
                button7.Enabled = true;
            }
            else if (listBox1.SelectedItem == null)
            {
                listBox2.ClearSelected();
                button7.Enabled = false;
                button8.Enabled = false;
            }
        }

        private void listBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                listBox1.ClearSelected();
                button8.Enabled = true;
            }
            else if (listBox2.SelectedItem == null)
            {
                listBox1.ClearSelected();
                button7.Enabled = false;
                button8.Enabled = false;
            }
            /*if (listBox1.SelectedItem != null)
            {
                listBox1.ClearSelected();
            }

            button8.Enabled = true;
            button7.Enabled = false;*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SettingsCopy.TableTitle.Remove(SettingsCopy.TableTitle[comboBox2.SelectedIndex]);
            LoadComboBox();
            LoadTableTemplate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                button1.Enabled = false;
                button2.Enabled = true;
            }
            else if (comboBox1.SelectedIndex == SettingsCopy.TableTitle.Count - 1)
            {
                button1.Enabled = true;
                button2.Enabled = false;
            }
            else if (comboBox1.SelectedItem == null)
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tuple<string, string> item = new Tuple<string, string>(SettingsCopy.TableTitle[comboBox1.SelectedIndex].Item1, SettingsCopy.TableTitle[comboBox1.SelectedIndex].Item2);
            SettingsCopy.TableTitle.Remove(SettingsCopy.TableTitle[comboBox1.SelectedIndex]);
            SettingsCopy.TableTitle.Insert(comboBox1.SelectedIndex - 1, item);
            LoadComboBox(comboBox1.SelectedIndex - 1);
            LoadTableTemplate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Tuple<string, string> item = new Tuple<string, string>(SettingsCopy.TableTitle[comboBox1.SelectedIndex].Item1, SettingsCopy.TableTitle[comboBox1.SelectedIndex].Item2);
            SettingsCopy.TableTitle.Remove(SettingsCopy.TableTitle[comboBox1.SelectedIndex]);
            SettingsCopy.TableTitle.Insert(comboBox1.SelectedIndex + 1, item);
            LoadComboBox(comboBox1.SelectedIndex + 1);
            LoadTableTemplate();
        }
    }
}
