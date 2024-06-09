using System;
using System.Windows.Forms;
using DBObjectsViewer.Forms;


namespace DBObjectsViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            JSONWorker.LoadJson(AppConsts.FileNamesConsts.AppSettingsFileName);
            JSONWorker.LoadJson(AppConsts.FileNamesConsts.SQLTestDataFileName, pathToFile: AppConsts.DirsConsts.DirectoryOfTestDataFiles);
            JSONWorker.LoadJson(AppConsts.FileNamesConsts.SQLTestIndexesFileName, pathToFile: AppConsts.DirsConsts.DirectoryOfTestDataFiles);
            JSONWorker.LoadJson(AppConsts.FileNamesConsts.SQLTestForeignsFileName, pathToFile: AppConsts.DirsConsts.DirectoryOfTestDataFiles);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TableTemplateForm settingsForm = new TableTemplateForm();
            DialogResult formResult = settingsForm.ShowDialog();

            if (formResult == DialogResult.Yes)
                JSONWorker.SaveJson(JSONWorker.AppSettings, AppConsts.FileNamesConsts.AppSettingsFileName);
        }

        private void button4_Click(object sender, EventArgs e)
        {
           /* SQLDBConnector sqlConnector = new SQLDBConnector(@"(local)\SQLEXPRESS2022", "54", "sa", "123"); // Home string

            sqlConnector.OpenConnection();

            List<string> tables2 = sqlConnector.ReturnListTables();
            textBox1.Text = SQLRequests.CompositeRequestToDB(tables2);

            Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>> DBInfo = sqlConnector.ReturnTablesInfo(tables2);

            //MessageBox.Show(DBInfo["tblFUND"]["FieldsInfo"][0]["attribute"]);

            JSONWorker.SaveJson(DBInfo, $"{sqlConnector.ReturnCatalogName()}_{AppConsts.DatabaseType.MYSQL}_" + DateTime.Now.ToString("MMddyyHHmmss") + ".json", AppConsts.DirsConsts.DirectoryOfDatabaseDataFiles);

            sqlConnector.CloseConnection();*/
        }

        private void OpenWorkForm(string databaseType)
        {
            this.Visible = false;
            DBWorkForm workForm = new DBWorkForm(databaseType);
            workForm.ShowDialog();
            this.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenWorkForm(AppConsts.DatabaseType.MySQL);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenWorkForm(AppConsts.DatabaseType.PostgreSQL);
        }
    }
}
