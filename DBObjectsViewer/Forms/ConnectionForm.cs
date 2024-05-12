using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DBObjectsViewer.Forms
{
    public partial class ConnectionForm : Form
    {
        private string DBServer { get; set; }
        private bool StableConnection { get; set; }

        public ConnectionForm(string DBServerName)
        {
            InitializeComponent();
            DBServer = DBServerName;
            StableConnection = false;
            label1.Text = $"{DBServer} connection";
            if (DBServer == AppConsts.DatabaseType.MYSQL)
                PortGroup.Visible = false;
        }

        private void TrimFields()
        {
            ConServer.Text = ConServer.Text.Trim();
            ConDataBaseName.Text = ConDataBaseName.Text.Trim();
            ConLogin.Text = ConLogin.Text.Trim();
            ConPassword.Text = ConPassword.Text.Trim();
        }

        private bool TryConnection()
        {
            try
            {
                if (DBServer == AppConsts.DatabaseType.MYSQL)
                {
                    SQLDBConnector con = new SQLDBConnector(ConServer.Text, ConDataBaseName.Text, ConLogin.Text, ConPassword.Text);
                    con.OpenConnection();
                    con.CloseConnection();
                    return true;
                }
                if (DBServer == AppConsts.DatabaseType.PostgreSQL)
                {
                    PostgreDBConnector con = new PostgreDBConnector(ConServer.Text, ConPort.Text, ConDataBaseName.Text, ConLogin.Text, ConPassword.Text);
                    con.OpenConnection();
                    con.CloseConnection();
                    return true;
                }
                return false;
            }
            catch (Exception) {  return false;   }
        }

        public dynamic ReturnStableConnection()
        {
            if (DBServer == AppConsts.DatabaseType.MYSQL)
                return new SQLDBConnector(ConServer.Text, ConDataBaseName.Text, ConLogin.Text, ConPassword.Text);
            if (DBServer == AppConsts.DatabaseType.MYSQL)
                return new PostgreDBConnector(ConServer.Text, ConPort.Text, ConDataBaseName.Text, ConLogin.Text, ConPassword.Text);
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TrimFields();
            backgroundWorker1.RunWorkerAsync();
        }

        private void ConnectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!StableConnection)
                this.DialogResult = DialogResult.Cancel;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke(new Action(() => {
                flowLayoutPanel1.Enabled = false;
                button1.Enabled = false;
            }));

            StableConnection = TryConnection();

            if (StableConnection)
            {
                StableConnection = true;
                
                Invoke(new Action(() => {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }));
            }
            else
            {
                MessageBox.Show("Connection failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Invoke(new Action(() =>
                {
                    flowLayoutPanel1.Enabled = true;
                    button1.Enabled = true;
                }));
            }
        }
    }
}
