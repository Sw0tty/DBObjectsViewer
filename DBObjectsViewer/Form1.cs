using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBObjectsViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PostgreDBConnector postgreConnector = new PostgreDBConnector("localhost", "5432", "postgres", "postgres", "Admin_1234");
            postgreConnector.OpenConnection();
            MessageBox.Show(postgreConnector.GetVersion());
            postgreConnector.CloseConnection();
        }
    }
}
