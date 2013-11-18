namespace CouchbaseToSSIS
{
    partial class DataMappingForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvOutputs = new System.Windows.Forms.DataGridView();
            this.btnGenerateModel = new System.Windows.Forms.Button();
            this.txtTableNameXPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.comboModelTables = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRootXPath = new System.Windows.Forms.TextBox();
            this.ofdModelSampleFile = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.txtModelFile = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbRepresentative = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudDocumentsToSample = new System.Windows.Forms.NumericUpDown();
            this.cbSample = new System.Windows.Forms.CheckBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClearModel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputs)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDocumentsToSample)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOutputs
            // 
            this.dgvOutputs.AllowUserToAddRows = false;
            this.dgvOutputs.AllowUserToDeleteRows = false;
            this.dgvOutputs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOutputs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOutputs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOutputs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOutputs.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvOutputs.Location = new System.Drawing.Point(12, 196);
            this.dgvOutputs.Name = "dgvOutputs";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOutputs.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvOutputs.Size = new System.Drawing.Size(503, 215);
            this.dgvOutputs.TabIndex = 6;
            this.dgvOutputs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvOutputs_CellFormatting);
            this.dgvOutputs.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOutputs_CellValueChanged);
            // 
            // btnGenerateModel
            // 
            this.btnGenerateModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateModel.Location = new System.Drawing.Point(411, 140);
            this.btnGenerateModel.Name = "btnGenerateModel";
            this.btnGenerateModel.Size = new System.Drawing.Size(104, 23);
            this.btnGenerateModel.TabIndex = 4;
            this.btnGenerateModel.Text = "Generate Model";
            this.btnGenerateModel.UseVisualStyleBackColor = true;
            this.btnGenerateModel.Click += new System.EventHandler(this.btnGenerateModel_Click);
            // 
            // txtTableNameXPath
            // 
            this.txtTableNameXPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTableNameXPath.Location = new System.Drawing.Point(114, 93);
            this.txtTableNameXPath.Name = "txtTableNameXPath";
            this.txtTableNameXPath.Size = new System.Drawing.Size(383, 20);
            this.txtTableNameXPath.TabIndex = 3;
            this.txtTableNameXPath.Text = "meta/name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Table Name XPath";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(440, 538);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(359, 538);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // comboModelTables
            // 
            this.comboModelTables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboModelTables.FormattingEnabled = true;
            this.comboModelTables.Location = new System.Drawing.Point(89, 169);
            this.comboModelTables.Name = "comboModelTables";
            this.comboModelTables.Size = new System.Drawing.Size(426, 21);
            this.comboModelTables.TabIndex = 5;
            this.comboModelTables.SelectedIndexChanged += new System.EventHandler(this.comboModelTables_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Model Tables";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Root XPath";
            // 
            // txtRootXPath
            // 
            this.txtRootXPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRootXPath.Location = new System.Drawing.Point(111, 67);
            this.txtRootXPath.Name = "txtRootXPath";
            this.txtRootXPath.Size = new System.Drawing.Size(386, 20);
            this.txtRootXPath.TabIndex = 2;
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseFile.Enabled = false;
            this.btnBrowseFile.Location = new System.Drawing.Point(422, 15);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFile.TabIndex = 1;
            this.btnBrowseFile.Text = "Browse...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // txtModelFile
            // 
            this.txtModelFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModelFile.Enabled = false;
            this.txtModelFile.Location = new System.Drawing.Point(186, 17);
            this.txtModelFile.Name = "txtModelFile";
            this.txtModelFile.Size = new System.Drawing.Size(230, 20);
            this.txtModelFile.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbRepresentative);
            this.groupBox1.Controls.Add(this.btnBrowseFile);
            this.groupBox1.Controls.Add(this.txtModelFile);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTableNameXPath);
            this.groupBox1.Controls.Add(this.nudDocumentsToSample);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtRootXPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbSample);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(503, 122);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Model Inference Strategy";
            // 
            // cbRepresentative
            // 
            this.cbRepresentative.AutoSize = true;
            this.cbRepresentative.Location = new System.Drawing.Point(6, 19);
            this.cbRepresentative.Name = "cbRepresentative";
            this.cbRepresentative.Size = new System.Drawing.Size(174, 17);
            this.cbRepresentative.TabIndex = 3;
            this.cbRepresentative.Text = "Use a representative document";
            this.cbRepresentative.UseVisualStyleBackColor = true;
            this.cbRepresentative.CheckedChanged += new System.EventHandler(this.cbRepresentative_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "documents returned from the view";
            // 
            // nudDocumentsToSample
            // 
            this.nudDocumentsToSample.Enabled = false;
            this.nudDocumentsToSample.Location = new System.Drawing.Point(73, 41);
            this.nudDocumentsToSample.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudDocumentsToSample.Name = "nudDocumentsToSample";
            this.nudDocumentsToSample.Size = new System.Drawing.Size(80, 20);
            this.nudDocumentsToSample.TabIndex = 1;
            this.nudDocumentsToSample.ValueChanged += new System.EventHandler(this.nudDocumentsToSample_ValueChanged);
            this.nudDocumentsToSample.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nudDocumentsToSample_KeyPress);
            // 
            // cbSample
            // 
            this.cbSample.AutoSize = true;
            this.cbSample.Location = new System.Drawing.Point(6, 42);
            this.cbSample.Name = "cbSample";
            this.cbSample.Size = new System.Drawing.Size(61, 17);
            this.cbSample.TabIndex = 0;
            this.cbSample.Text = "Sample";
            this.cbSample.UseVisualStyleBackColor = true;
            this.cbSample.CheckedChanged += new System.EventHandler(this.cbSample_CheckedChanged);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 434);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(503, 98);
            this.txtLog.TabIndex = 15;
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 418);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Log Messages";
            // 
            // btnClearModel
            // 
            this.btnClearModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearModel.Location = new System.Drawing.Point(291, 140);
            this.btnClearModel.Name = "btnClearModel";
            this.btnClearModel.Size = new System.Drawing.Size(114, 23);
            this.btnClearModel.TabIndex = 17;
            this.btnClearModel.Text = "Clear Model";
            this.btnClearModel.UseVisualStyleBackColor = true;
            this.btnClearModel.Click += new System.EventHandler(this.btnClearModel_Click);
            // 
            // DataMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 573);
            this.Controls.Add(this.btnClearModel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboModelTables);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnGenerateModel);
            this.Controls.Add(this.dgvOutputs);
            this.MinimumSize = new System.Drawing.Size(535, 600);
            this.Name = "DataMappingForm";
            this.Text = "Data Mapping";
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputs)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDocumentsToSample)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOutputs;
        private System.Windows.Forms.Button btnGenerateModel;
        private System.Windows.Forms.TextBox txtTableNameXPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox comboModelTables;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRootXPath;
        private System.Windows.Forms.OpenFileDialog ofdModelSampleFile;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.TextBox txtModelFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbRepresentative;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudDocumentsToSample;
        private System.Windows.Forms.CheckBox cbSample;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnClearModel;
    }
}