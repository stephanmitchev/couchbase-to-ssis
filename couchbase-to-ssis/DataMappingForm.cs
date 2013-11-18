using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.IO;
using Couchbase;

namespace CouchbaseToSSIS
{
    public partial class DataMappingForm : Form
    {
        private IDTSComponentMetaData100 metaData;
        private CManagedComponentWrapper designTimeInstance;

        private IView<IViewRow> currentView = null;

        public JSONDataModel currentModel = new JSONDataModel();

        private DataTable dtTypesTable = new DataTable();

        public DataMappingForm(IDTSComponentMetaData100 md, CManagedComponentWrapper dti, IView<IViewRow> view)
        {
            InitializeComponent();

            metaData = md;
            designTimeInstance = dti;
            currentView = view;

            this.Text = md.Name + " Data Mapping";

            dtTypesTable = new DataTable();
            dtTypesTable.Columns.Add("datatype");
            dtTypesTable.Columns.Add("description");
            dtTypesTable.Rows.Add(typeof(long).ToString(), "Whole Number");
            dtTypesTable.Rows.Add(typeof(double).ToString(), "Floating Number");
            dtTypesTable.Rows.Add(typeof(bool).ToString(), "Boolean Value");
            dtTypesTable.Rows.Add(typeof(DateTime).ToString(), "Date / Time");
            dtTypesTable.Rows.Add(typeof(string).ToString(), "Text");
            
            string modelXML = metaData.CustomPropertyCollection["modelXML"].Value;
            if (modelXML != null && !modelXML.Trim().Equals(""))
            {
                currentModel.ReadXml(new StringReader(modelXML));
            }

            dgvOutputs.AutoGenerateColumns = false;

            dgvOutputs.Columns.Clear();
            DataGridViewTextBoxColumn col;
            DataGridViewComboBoxColumn cbcol;

            col = new DataGridViewTextBoxColumn();
            col.Name = "column";
            col.HeaderText = "Inferred Column";
            col.DataPropertyName = "column";
            col.ReadOnly = true;
            dgvOutputs.Columns.Add(col);

            cbcol = new DataGridViewComboBoxColumn();
            cbcol.Name = "datatype";
            cbcol.HeaderText = "Data Type";
            cbcol.DataSource = dtTypesTable;
            cbcol.ValueMember = "datatype";
            cbcol.DisplayMember = "description";
            cbcol.DataPropertyName = "datatype";
            cbcol.ReadOnly = false;
            dgvOutputs.Columns.Add(cbcol);

            col = new DataGridViewTextBoxColumn();
            col.Name = "xpath";
            col.HeaderText = "XPath";
            col.DataPropertyName = "xpath";
            col.ReadOnly = true;
            dgvOutputs.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.Name = "shortColumn";
            col.HeaderText = "Database Column";
            col.DataPropertyName = "shortColumn";
            col.ReadOnly = false;
            dgvOutputs.Columns.Add(col);

            // Draw the model
            comboModelTables.Items.Clear();

            foreach (DataTable table in currentModel.Tables)
            {
                comboModelTables.Items.Add(table.TableName);
            }

            if (comboModelTables.Items.Count > 0)
                comboModelTables.SelectedIndex = 0;

        }

        private void btnGenerateModel_Click(object sender, EventArgs e)
        {
            // Start with an empty model
            //currentModel = new JSONDataModel();

            // Clear the logs
            txtLog.Text = "";

            if (cbRepresentative.Checked)
            {
                if (File.Exists(txtModelFile.Text))
                {
                    // Initialize the logs 
                    string logMessages;

                    // Create the model of this document
                    JSONDataModel model = new JSONDataModel(File.ReadAllText(txtModelFile.Text), txtRootXPath.Text, txtTableNameXPath.Text, out logMessages);
                    
                    // Merge it with the current model
                    currentModel = currentModel.mergeWith(model);
                    
                    // Append the logs
                    txtLog.Text += logMessages;
                }
                else
                {
                    MessageBox.Show("File does not exists. Please select a valid representative file.");
                }

            }

            if (cbSample.Checked)
            {
                // Sample documents and union the results
                foreach (var row in currentView.Limit((int)nudDocumentsToSample.Value))
                {
                    // Initialize the logs 
                    string logMessages;

                    // Create the model of this document
                    JSONDataModel model = new JSONDataModel(row.GetItem().ToString(), txtRootXPath.Text, txtTableNameXPath.Text, out logMessages);

                    // Merge it with the current model
                    currentModel = currentModel.mergeWith(model);

                    // Append the logs
                    txtLog.Text += logMessages;
                }

            }

            
            
            // Draw the model
            comboModelTables.Items.Clear();

            foreach (DataTable table in currentModel.Tables)
            {
                comboModelTables.Items.Add(table.TableName);
            }

            if (comboModelTables.Items.Count > 0)
                comboModelTables.SelectedIndex = 0;
        }

        private void comboModelTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvOutputs.DataSource = currentModel;
            dgvOutputs.DataMember = currentModel.Tables[(sender as ComboBox).SelectedItem.ToString()].TableName;

        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            if (ofdModelSampleFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtModelFile.Text = ofdModelSampleFile.FileName;

            }
        }

        private void cbSample_CheckedChanged(object sender, EventArgs e)
        {
            enforceUIRules();
        }

        private void cbRepresentative_CheckedChanged(object sender, EventArgs e)
        {
            enforceUIRules();
        }

        private void nudDocumentsToSample_ValueChanged(object sender, EventArgs e)
        {
            enforceUIRules();
        }

        private void nudDocumentsToSample_KeyPress(object sender, KeyPressEventArgs e)
        {
            enforceUIRules();
        }

        private void dgvOutputs_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


            enforceUIRules();
        }


        private void enforceUIRules()
        {
            nudDocumentsToSample.Enabled = cbSample.Checked;
            txtModelFile.Enabled = cbRepresentative.Checked;
            btnBrowseFile.Enabled = cbRepresentative.Checked;
            nudDocumentsToSample.Value = (long)nudDocumentsToSample.Value;
            
            btnGenerateModel.Enabled = (cbSample.Checked && nudDocumentsToSample.Value > 0) || (cbRepresentative.Checked && !txtModelFile.Text.Equals("") && !ofdModelSampleFile.FileName.Equals(""));

            bool valid = true;

            // Check for column validity
            foreach (DataTable table in currentModel.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    valid = valid && row["shortColumn"].ToString().Length <= 30;
                }

                // Check for column collisions
                valid = valid && (currentModel.findCollisions(table.TableName, "shortColumn").Count == 0);
            }

            

            btnOK.Enabled = valid;
        }


        private void dgvOutputs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataRowView dvRow = dgvOutputs.Rows[e.RowIndex].DataBoundItem as DataRowView;
            
            
            if (dvRow != null && comboModelTables.SelectedItem != null)
            {
                List<DataRow> collisions = currentModel.findCollisions(comboModelTables.SelectedItem.ToString(), "shortColumn");

                DataRow row = dvRow.Row;

                // If the column is the shortColumn column, check the value. 
                if (collisions.Contains(row))
                {
                    e.CellStyle.BackColor = Color.LightBlue;
                }
            }

            // If the column is the shortColumn column, check the value. 
            if (dgvOutputs.Columns[e.ColumnIndex].Name == "shortColumn")
            {
                // Highlight incompatible column names
                if (e.Value != null && e.Value.ToString().Length > 30)
                {
                    e.CellStyle.BackColor = Color.Red;
                }
            }
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.ScrollToCaret();
        }

        private void btnClearModel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Clear Model", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                currentModel = new JSONDataModel();
            }
        }

        
        
    }
}
