namespace CouchbaseToSSIS
{
    partial class CouchbaseSourceAdapterUIForm
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
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblURL = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.lblBucket = new System.Windows.Forms.Label();
            this.txtBucket = new System.Windows.Forms.TextBox();
            this.lblDesignDoc = new System.Windows.Forms.Label();
            this.txtDesignDoc = new System.Windows.Forms.TextBox();
            this.lblView = new System.Windows.Forms.Label();
            this.txtView = new System.Windows.Forms.TextBox();
            this.cbForceReindex = new System.Windows.Forms.CheckBox();
            this.lblStartKey = new System.Windows.Forms.Label();
            this.txtStartKey = new System.Windows.Forms.TextBox();
            this.cbDescending = new System.Windows.Forms.CheckBox();
            this.lblEndKey = new System.Windows.Forms.Label();
            this.txtEndKey = new System.Windows.Forms.TextBox();
            this.txtTestResult = new System.Windows.Forms.TextBox();
            this.lblTestResult = new System.Windows.Forms.Label();
            this.btnMapping = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtURL
            // 
            this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURL.Location = new System.Drawing.Point(78, 12);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(247, 20);
            this.txtURL.TabIndex = 0;
            this.txtURL.Text = "http://hostname.com/";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(169, 313);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(250, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(12, 15);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(55, 13);
            this.lblURL.TabIndex = 3;
            this.lblURL.Text = "View URL";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(12, 67);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(78, 64);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(247, 20);
            this.txtPassword.TabIndex = 2;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestConnection.Location = new System.Drawing.Point(169, 217);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(156, 23);
            this.btnTestConnection.TabIndex = 9;
            this.btnTestConnection.Text = "Test and Use Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // lblBucket
            // 
            this.lblBucket.AutoSize = true;
            this.lblBucket.Location = new System.Drawing.Point(12, 41);
            this.lblBucket.Name = "lblBucket";
            this.lblBucket.Size = new System.Drawing.Size(41, 13);
            this.lblBucket.TabIndex = 12;
            this.lblBucket.Text = "Bucket";
            // 
            // txtBucket
            // 
            this.txtBucket.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBucket.Location = new System.Drawing.Point(78, 38);
            this.txtBucket.Name = "txtBucket";
            this.txtBucket.Size = new System.Drawing.Size(247, 20);
            this.txtBucket.TabIndex = 1;
            // 
            // lblDesignDoc
            // 
            this.lblDesignDoc.AutoSize = true;
            this.lblDesignDoc.Location = new System.Drawing.Point(12, 93);
            this.lblDesignDoc.Name = "lblDesignDoc";
            this.lblDesignDoc.Size = new System.Drawing.Size(60, 13);
            this.lblDesignDoc.TabIndex = 14;
            this.lblDesignDoc.Text = "DesignDoc";
            // 
            // txtDesignDoc
            // 
            this.txtDesignDoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDesignDoc.Location = new System.Drawing.Point(78, 90);
            this.txtDesignDoc.Name = "txtDesignDoc";
            this.txtDesignDoc.Size = new System.Drawing.Size(247, 20);
            this.txtDesignDoc.TabIndex = 3;
            // 
            // lblView
            // 
            this.lblView.AutoSize = true;
            this.lblView.Location = new System.Drawing.Point(12, 119);
            this.lblView.Name = "lblView";
            this.lblView.Size = new System.Drawing.Size(30, 13);
            this.lblView.TabIndex = 16;
            this.lblView.Text = "View";
            // 
            // txtView
            // 
            this.txtView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtView.Location = new System.Drawing.Point(78, 116);
            this.txtView.Name = "txtView";
            this.txtView.Size = new System.Drawing.Size(247, 20);
            this.txtView.TabIndex = 4;
            // 
            // cbForceReindex
            // 
            this.cbForceReindex.AutoSize = true;
            this.cbForceReindex.Checked = true;
            this.cbForceReindex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbForceReindex.Location = new System.Drawing.Point(78, 142);
            this.cbForceReindex.Name = "cbForceReindex";
            this.cbForceReindex.Size = new System.Drawing.Size(129, 17);
            this.cbForceReindex.TabIndex = 5;
            this.cbForceReindex.Text = "Force view reindexing";
            this.cbForceReindex.UseVisualStyleBackColor = true;
            // 
            // lblStartKey
            // 
            this.lblStartKey.AutoSize = true;
            this.lblStartKey.Location = new System.Drawing.Point(12, 168);
            this.lblStartKey.Name = "lblStartKey";
            this.lblStartKey.Size = new System.Drawing.Size(50, 13);
            this.lblStartKey.TabIndex = 20;
            this.lblStartKey.Text = "Start Key";
            // 
            // txtStartKey
            // 
            this.txtStartKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStartKey.Location = new System.Drawing.Point(78, 165);
            this.txtStartKey.Name = "txtStartKey";
            this.txtStartKey.Size = new System.Drawing.Size(247, 20);
            this.txtStartKey.TabIndex = 7;
            // 
            // cbDescending
            // 
            this.cbDescending.AutoSize = true;
            this.cbDescending.Location = new System.Drawing.Point(213, 142);
            this.cbDescending.Name = "cbDescending";
            this.cbDescending.Size = new System.Drawing.Size(110, 17);
            this.cbDescending.TabIndex = 6;
            this.cbDescending.Text = "Descending order";
            this.cbDescending.UseVisualStyleBackColor = true;
            // 
            // lblEndKey
            // 
            this.lblEndKey.AutoSize = true;
            this.lblEndKey.Location = new System.Drawing.Point(12, 194);
            this.lblEndKey.Name = "lblEndKey";
            this.lblEndKey.Size = new System.Drawing.Size(47, 13);
            this.lblEndKey.TabIndex = 23;
            this.lblEndKey.Text = "End Key";
            // 
            // txtEndKey
            // 
            this.txtEndKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEndKey.Location = new System.Drawing.Point(78, 191);
            this.txtEndKey.Name = "txtEndKey";
            this.txtEndKey.Size = new System.Drawing.Size(247, 20);
            this.txtEndKey.TabIndex = 8;
            // 
            // txtTestResult
            // 
            this.txtTestResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTestResult.Location = new System.Drawing.Point(15, 246);
            this.txtTestResult.Multiline = true;
            this.txtTestResult.Name = "txtTestResult";
            this.txtTestResult.ReadOnly = true;
            this.txtTestResult.Size = new System.Drawing.Size(310, 61);
            this.txtTestResult.TabIndex = 24;
            // 
            // lblTestResult
            // 
            this.lblTestResult.AutoSize = true;
            this.lblTestResult.Location = new System.Drawing.Point(12, 230);
            this.lblTestResult.Name = "lblTestResult";
            this.lblTestResult.Size = new System.Drawing.Size(61, 13);
            this.lblTestResult.TabIndex = 25;
            this.lblTestResult.Text = "Test Result";
            // 
            // btnMapping
            // 
            this.btnMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMapping.Enabled = false;
            this.btnMapping.Location = new System.Drawing.Point(12, 313);
            this.btnMapping.Name = "btnMapping";
            this.btnMapping.Size = new System.Drawing.Size(151, 23);
            this.btnMapping.TabIndex = 10;
            this.btnMapping.Text = "Edit Data Mapping";
            this.btnMapping.UseVisualStyleBackColor = true;
            this.btnMapping.Click += new System.EventHandler(this.btnMapping_Click);
            // 
            // CouchbaseSourceAdapterUIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 348);
            this.Controls.Add(this.btnMapping);
            this.Controls.Add(this.lblTestResult);
            this.Controls.Add(this.txtTestResult);
            this.Controls.Add(this.lblEndKey);
            this.Controls.Add(this.txtEndKey);
            this.Controls.Add(this.cbDescending);
            this.Controls.Add(this.lblStartKey);
            this.Controls.Add(this.txtStartKey);
            this.Controls.Add(this.cbForceReindex);
            this.Controls.Add(this.lblView);
            this.Controls.Add(this.txtView);
            this.Controls.Add(this.lblDesignDoc);
            this.Controls.Add(this.txtDesignDoc);
            this.Controls.Add(this.lblBucket);
            this.Controls.Add(this.txtBucket);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtURL);
            this.MinimumSize = new System.Drawing.Size(345, 375);
            this.Name = "CouchbaseSourceAdapterUIForm";
            this.Text = "CouchbaseSourceAdapterUIForm";
            this.Load += new System.EventHandler(this.CouchbaseSourceAdapterUIForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Label lblBucket;
        private System.Windows.Forms.TextBox txtBucket;
        private System.Windows.Forms.Label lblDesignDoc;
        private System.Windows.Forms.TextBox txtDesignDoc;
        private System.Windows.Forms.Label lblView;
        private System.Windows.Forms.TextBox txtView;
        private System.Windows.Forms.CheckBox cbForceReindex;
        private System.Windows.Forms.Label lblStartKey;
        private System.Windows.Forms.TextBox txtStartKey;
        private System.Windows.Forms.CheckBox cbDescending;
        private System.Windows.Forms.Label lblEndKey;
        private System.Windows.Forms.TextBox txtEndKey;
        private System.Windows.Forms.TextBox txtTestResult;
        private System.Windows.Forms.Label lblTestResult;
        private System.Windows.Forms.Button btnMapping;

        
    }
}