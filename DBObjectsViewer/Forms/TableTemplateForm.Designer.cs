namespace DBObjectsViewer.Forms
{
    partial class TableTemplateForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableTemplateForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.gridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ScanIndexesCheckBox = new System.Windows.Forms.CheckBox();
            this.ScanForeignsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.FullDTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.SaveSettingsButton = new System.Windows.Forms.Button();
            this.CloseWithoutSaveButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ScanViewsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.AddViewsCheckBox = new System.Windows.Forms.CheckBox();
            this.AddIndexesCheckBox = new System.Windows.Forms.CheckBox();
            this.AddTHeaderCheckBox = new System.Windows.Forms.CheckBox();
            this.AddForeingsCheckBox = new System.Windows.Forms.CheckBox();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.gridContextMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.gridContextMenu;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(587, 367);
            this.dataGridView1.TabIndex = 0;
            // 
            // gridContextMenu
            // 
            this.gridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.gridContextMenu.Name = "gridContextMenu";
            this.gridContextMenu.Size = new System.Drawing.Size(181, 92);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "Foreigns header";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // ScanIndexesCheckBox
            // 
            this.ScanIndexesCheckBox.AutoSize = true;
            this.ScanIndexesCheckBox.Location = new System.Drawing.Point(6, 42);
            this.ScanIndexesCheckBox.Name = "ScanIndexesCheckBox";
            this.ScanIndexesCheckBox.Size = new System.Drawing.Size(207, 17);
            this.ScanIndexesCheckBox.TabIndex = 1;
            this.ScanIndexesCheckBox.Text = "Получить информацию об индексах";
            this.ScanIndexesCheckBox.UseVisualStyleBackColor = true;
            this.ScanIndexesCheckBox.CheckedChanged += new System.EventHandler(this.ScanIndexesCheckBox_CheckedChanged);
            // 
            // ScanForeignsCheckBox
            // 
            this.ScanForeignsCheckBox.AutoSize = true;
            this.ScanForeignsCheckBox.Location = new System.Drawing.Point(6, 19);
            this.ScanForeignsCheckBox.Name = "ScanForeignsCheckBox";
            this.ScanForeignsCheckBox.Size = new System.Drawing.Size(246, 17);
            this.ScanForeignsCheckBox.TabIndex = 2;
            this.ScanForeignsCheckBox.Text = "Получить информацию о вторичных ключах";
            this.ScanForeignsCheckBox.UseVisualStyleBackColor = true;
            this.ScanForeignsCheckBox.CheckedChanged += new System.EventHandler(this.ScanForeignsCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(593, 386);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Шаблон таблицы в Word";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(15, 480);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(286, 39);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(62, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(162, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(230, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = ">";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(6, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "<";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(293, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Добавить колонку";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(293, 48);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "Удалить колонку";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(159, 20);
            this.textBox1.TabIndex = 7;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(7, 49);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(279, 21);
            this.comboBox2.TabIndex = 3;
            // 
            // FullDTypeCheckBox
            // 
            this.FullDTypeCheckBox.AutoSize = true;
            this.FullDTypeCheckBox.Location = new System.Drawing.Point(6, 42);
            this.FullDTypeCheckBox.Name = "FullDTypeCheckBox";
            this.FullDTypeCheckBox.Size = new System.Drawing.Size(204, 17);
            this.FullDTypeCheckBox.TabIndex = 8;
            this.FullDTypeCheckBox.Text = "Точная информация о типе данных";
            this.FullDTypeCheckBox.UseVisualStyleBackColor = true;
            this.FullDTypeCheckBox.CheckedChanged += new System.EventHandler(this.FullDTypeCheckBox_CheckedChanged);
            // 
            // SaveSettingsButton
            // 
            this.SaveSettingsButton.Location = new System.Drawing.Point(599, 487);
            this.SaveSettingsButton.Name = "SaveSettingsButton";
            this.SaveSettingsButton.Size = new System.Drawing.Size(140, 32);
            this.SaveSettingsButton.TabIndex = 9;
            this.SaveSettingsButton.Text = "Сохранить";
            this.SaveSettingsButton.UseVisualStyleBackColor = true;
            this.SaveSettingsButton.Click += new System.EventHandler(this.SaveSettingsButton_Click);
            // 
            // CloseWithoutSaveButton
            // 
            this.CloseWithoutSaveButton.Location = new System.Drawing.Point(751, 487);
            this.CloseWithoutSaveButton.Name = "CloseWithoutSaveButton";
            this.CloseWithoutSaveButton.Size = new System.Drawing.Size(140, 32);
            this.CloseWithoutSaveButton.TabIndex = 10;
            this.CloseWithoutSaveButton.Text = "Отменить изменения";
            this.CloseWithoutSaveButton.UseVisualStyleBackColor = true;
            this.CloseWithoutSaveButton.Click += new System.EventHandler(this.button6_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(9, 32);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(104, 82);
            this.listBox1.TabIndex = 11;
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // listBox2
            // 
            this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(160, 32);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(114, 82);
            this.listBox2.TabIndex = 12;
            this.listBox2.SelectedValueChanged += new System.EventHandler(this.listBox2_SelectedValueChanged);
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(122, 38);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(32, 32);
            this.button7.TabIndex = 13;
            this.button7.Text = ">";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button8.Enabled = false;
            this.button8.Location = new System.Drawing.Point(122, 76);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(32, 32);
            this.button8.TabIndex = 14;
            this.button8.Text = "<";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Не выбранные";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Выбранные";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.listBox1);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.listBox2);
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Location = new System.Drawing.Point(611, 267);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(280, 131);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Информация о колонках";
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(172, 21);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(114, 21);
            this.comboBox3.TabIndex = 18;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.comboBox3);
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.Controls.Add(this.comboBox2);
            this.groupBox4.Location = new System.Drawing.Point(15, 404);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(424, 76);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // ScanViewsCheckBox
            // 
            this.ScanViewsCheckBox.AutoSize = true;
            this.ScanViewsCheckBox.Enabled = false;
            this.ScanViewsCheckBox.Location = new System.Drawing.Point(6, 65);
            this.ScanViewsCheckBox.Name = "ScanViewsCheckBox";
            this.ScanViewsCheckBox.Size = new System.Drawing.Size(236, 17);
            this.ScanViewsCheckBox.TabIndex = 20;
            this.ScanViewsCheckBox.Text = "Получить информацию о представлениях";
            this.ScanViewsCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ScanIndexesCheckBox);
            this.groupBox5.Controls.Add(this.ScanForeignsCheckBox);
            this.groupBox5.Controls.Add(this.ScanViewsCheckBox);
            this.groupBox5.Location = new System.Drawing.Point(611, 152);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(280, 100);
            this.groupBox5.TabIndex = 21;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Настройки сканера:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.AddViewsCheckBox);
            this.groupBox6.Controls.Add(this.AddIndexesCheckBox);
            this.groupBox6.Controls.Add(this.AddTHeaderCheckBox);
            this.groupBox6.Controls.Add(this.AddForeingsCheckBox);
            this.groupBox6.Controls.Add(this.FullDTypeCheckBox);
            this.groupBox6.Location = new System.Drawing.Point(611, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(280, 134);
            this.groupBox6.TabIndex = 22;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Настройки Word:";
            // 
            // AddViewsCheckBox
            // 
            this.AddViewsCheckBox.AutoSize = true;
            this.AddViewsCheckBox.Enabled = false;
            this.AddViewsCheckBox.Location = new System.Drawing.Point(6, 111);
            this.AddViewsCheckBox.Name = "AddViewsCheckBox";
            this.AddViewsCheckBox.Size = new System.Drawing.Size(239, 17);
            this.AddViewsCheckBox.TabIndex = 21;
            this.AddViewsCheckBox.Text = "Добавить информацию о представлениях";
            this.AddViewsCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddIndexesCheckBox
            // 
            this.AddIndexesCheckBox.AutoSize = true;
            this.AddIndexesCheckBox.Location = new System.Drawing.Point(6, 88);
            this.AddIndexesCheckBox.Name = "AddIndexesCheckBox";
            this.AddIndexesCheckBox.Size = new System.Drawing.Size(210, 17);
            this.AddIndexesCheckBox.TabIndex = 21;
            this.AddIndexesCheckBox.Text = "Добавить информацию об индексах";
            this.AddIndexesCheckBox.UseVisualStyleBackColor = true;
            this.AddIndexesCheckBox.CheckedChanged += new System.EventHandler(this.AddIndexesInfo_CheckedChanged);
            // 
            // AddTHeaderCheckBox
            // 
            this.AddTHeaderCheckBox.AutoSize = true;
            this.AddTHeaderCheckBox.Enabled = false;
            this.AddTHeaderCheckBox.Location = new System.Drawing.Point(6, 19);
            this.AddTHeaderCheckBox.Name = "AddTHeaderCheckBox";
            this.AddTHeaderCheckBox.Size = new System.Drawing.Size(198, 17);
            this.AddTHeaderCheckBox.TabIndex = 23;
            this.AddTHeaderCheckBox.Text = "Добавить наименования колонок";
            this.AddTHeaderCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddForeingsCheckBox
            // 
            this.AddForeingsCheckBox.AutoSize = true;
            this.AddForeingsCheckBox.Location = new System.Drawing.Point(6, 65);
            this.AddForeingsCheckBox.Name = "AddForeingsCheckBox";
            this.AddForeingsCheckBox.Size = new System.Drawing.Size(249, 17);
            this.AddForeingsCheckBox.TabIndex = 22;
            this.AddForeingsCheckBox.Text = "Добавить информацию о вторичных ключах";
            this.AddForeingsCheckBox.UseVisualStyleBackColor = true;
            this.AddForeingsCheckBox.CheckedChanged += new System.EventHandler(this.AddForeignsInfo_CheckedChanged);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem2.Text = "Indexes header";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem3.Text = "Table header";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // TableTemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 529);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.CloseWithoutSaveButton);
            this.Controls.Add(this.SaveSettingsButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TableTemplateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Шаблон информации об объекте";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TableTemplateForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.gridContextMenu.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox ScanIndexesCheckBox;
        private System.Windows.Forms.CheckBox ScanForeignsCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox FullDTypeCheckBox;
        private System.Windows.Forms.Button SaveSettingsButton;
        private System.Windows.Forms.Button CloseWithoutSaveButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox ScanViewsCheckBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox AddTHeaderCheckBox;
        private System.Windows.Forms.CheckBox AddIndexesCheckBox;
        private System.Windows.Forms.CheckBox AddForeingsCheckBox;
        private System.Windows.Forms.CheckBox AddViewsCheckBox;
        private System.Windows.Forms.ContextMenuStrip gridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
    }
}