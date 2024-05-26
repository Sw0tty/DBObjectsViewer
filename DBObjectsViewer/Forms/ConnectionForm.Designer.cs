namespace DBObjectsViewer.Forms
{
    partial class ConnectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionForm));
            this.button1 = new System.Windows.Forms.Button();
            this.ConServer = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ConDataBaseName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ConLogin = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ConPassword = new System.Windows.Forms.TextBox();
            this.portGroup = new System.Windows.Forms.GroupBox();
            this.ConPort = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.schemaGroup = new System.Windows.Forms.GroupBox();
            this.ConSchema = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.portGroup.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.schemaGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(81, 370);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(193, 49);
            this.button1.TabIndex = 0;
            this.button1.Text = "Выгрузить информацию базы";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ConServer
            // 
            this.ConServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConServer.Location = new System.Drawing.Point(3, 16);
            this.ConServer.Name = "ConServer";
            this.ConServer.Size = new System.Drawing.Size(319, 22);
            this.ConServer.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ConServer);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(325, 42);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сервер:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ConDataBaseName);
            this.groupBox2.Location = new System.Drawing.Point(3, 51);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(325, 42);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Наименование базы:";
            // 
            // ConDataBaseName
            // 
            this.ConDataBaseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConDataBaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConDataBaseName.Location = new System.Drawing.Point(3, 16);
            this.ConDataBaseName.Name = "ConDataBaseName";
            this.ConDataBaseName.Size = new System.Drawing.Size(319, 22);
            this.ConDataBaseName.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ConLogin);
            this.groupBox3.Location = new System.Drawing.Point(3, 147);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(325, 42);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Логин:";
            // 
            // ConLogin
            // 
            this.ConLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConLogin.Location = new System.Drawing.Point(3, 16);
            this.ConLogin.Name = "ConLogin";
            this.ConLogin.Size = new System.Drawing.Size(319, 22);
            this.ConLogin.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ConPassword);
            this.groupBox4.Location = new System.Drawing.Point(3, 195);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(325, 42);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Пароль:";
            // 
            // ConPassword
            // 
            this.ConPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConPassword.Location = new System.Drawing.Point(3, 16);
            this.ConPassword.Name = "ConPassword";
            this.ConPassword.Size = new System.Drawing.Size(319, 22);
            this.ConPassword.TabIndex = 1;
            // 
            // portGroup
            // 
            this.portGroup.Controls.Add(this.ConPort);
            this.portGroup.Location = new System.Drawing.Point(3, 99);
            this.portGroup.Name = "portGroup";
            this.portGroup.Size = new System.Drawing.Size(325, 42);
            this.portGroup.TabIndex = 6;
            this.portGroup.TabStop = false;
            this.portGroup.Text = "Порт:";
            this.portGroup.Visible = false;
            // 
            // ConPort
            // 
            this.ConPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConPort.Location = new System.Drawing.Point(3, 16);
            this.ConPort.Name = "ConPort";
            this.ConPort.Size = new System.Drawing.Size(319, 22);
            this.ConPort.TabIndex = 1;
            this.ConPort.Text = "5432";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.groupBox2);
            this.flowLayoutPanel1.Controls.Add(this.portGroup);
            this.flowLayoutPanel1.Controls.Add(this.groupBox3);
            this.flowLayoutPanel1.Controls.Add(this.groupBox4);
            this.flowLayoutPanel1.Controls.Add(this.schemaGroup);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 63);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(335, 300);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // schemaGroup
            // 
            this.schemaGroup.Controls.Add(this.ConSchema);
            this.schemaGroup.Location = new System.Drawing.Point(3, 243);
            this.schemaGroup.Name = "schemaGroup";
            this.schemaGroup.Size = new System.Drawing.Size(325, 42);
            this.schemaGroup.TabIndex = 9;
            this.schemaGroup.TabStop = false;
            this.schemaGroup.Text = "Схема:";
            this.schemaGroup.Visible = false;
            // 
            // ConSchema
            // 
            this.ConSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConSchema.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ConSchema.Location = new System.Drawing.Point(3, 16);
            this.ConSchema.Name = "ConSchema";
            this.ConSchema.Size = new System.Drawing.Size(319, 22);
            this.ConSchema.TabIndex = 1;
            this.ConSchema.Text = "public";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 45);
            this.label1.TabIndex = 8;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // ConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 431);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConnectionForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConnectionForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.portGroup.ResumeLayout(false);
            this.portGroup.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.schemaGroup.ResumeLayout(false);
            this.schemaGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox ConServer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox ConDataBaseName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox ConLogin;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox ConPassword;
        private System.Windows.Forms.GroupBox portGroup;
        private System.Windows.Forms.TextBox ConPort;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox schemaGroup;
        private System.Windows.Forms.TextBox ConSchema;
    }
}