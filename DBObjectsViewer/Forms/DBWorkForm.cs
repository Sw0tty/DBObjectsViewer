using System;
using System.Windows.Forms;


namespace DBObjectsViewer.Forms
{
    public partial class DBWorkForm : Form
    {
        protected string DataBaseType;

        public DBWorkForm(string databaseType)
        {
            InitializeComponent();
            DataBaseType = databaseType;
            label1.Text = $"Now working with {DataBaseType}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionForm conForm = new ConnectionForm(DataBaseType);

            this.Visible = false;
            DialogResult conResult = conForm.ShowDialog();
            this.Visible = true;

            if (conResult == DialogResult.OK)
            {
                WorkProgressForm progressForm = new WorkProgressForm(connection: conForm.ReturnStableConnection());
                DialogResult progressResult = progressForm.ShowDialog();

                if (progressResult == DialogResult.OK)
                    MessageBox.Show("Database data successfully save.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DBScriptMakerForm scriprtMaker = new DBScriptMakerForm(DataBaseType);
            scriprtMaker.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filePath = FilesManager.SelectFileOnPC(@"C:\", "Selecting an Excel file to scan", supportedFormats: AppConsts.FileDialogSupportedFormats.ExcelFormats);

            if (AppUsedFunctions.CheckSupportFormat(filePath))
            {
                WorkProgressForm progressForm = new WorkProgressForm(fileInfo: new Tuple<string, string>(filePath, DataBaseType));
                DialogResult progressResult = progressForm.ShowDialog();

                if (progressResult == DialogResult.OK)
                    MessageBox.Show($"Excel file successfully scan. File saved on path: {progressForm.ReturnSavePath()}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = FilesManager.SelectFileOnPC(AppDomain.CurrentDomain.BaseDirectory, "Selecting an JSON file to convert in Word", supportedFormats: AppConsts.FileDialogSupportedFormats.JsonFormats);

            if (!AppUsedFunctions.CheckSelectingFile(filePath, DataBaseType))
            {}
            else if (AppUsedFunctions.CheckSupportFormat(filePath))
            {
                Tuple<string, string> pathParts = AppUsedFunctions.SplitPath(filePath);

                dynamic data = JSONWorker.LoadAndReturnJSON(pathParts.Item1, pathToFile: pathParts.Item2, defAppPath: false);

                FilesManager.CheckPath(AppConsts.DirsConsts.DirectoryOfWordFormatDatabaseDataFiles);

                string uName = FilesManager.MakeUniqueFileName("WordReport", DataBaseType);
                string workPath = AppDomain.CurrentDomain.BaseDirectory + AppConsts.DirsConsts.DirectoryOfWordFormatDatabaseDataFiles + uName;

                WorkProgressForm progressForm = new WorkProgressForm(jsonInfo: new Tuple<string, dynamic>(workPath, data));
                progressForm.ShowDialog();

                MessageBox.Show("Json converted!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
