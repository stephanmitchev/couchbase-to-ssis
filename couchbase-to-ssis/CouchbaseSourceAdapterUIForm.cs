using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

using Couchbase;
using Couchbase.Configuration;
using Couchbase.Management;
using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;
using System.Net;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CouchbaseToSSIS
{
    public partial class CouchbaseSourceAdapterUIForm : Form
    {
        private Connections connections;
        private Variables variables;
        private IDTSComponentMetaData100 metaData;
        private CManagedComponentWrapper designTimeInstance;

        private CouchbaseClient client;
        IView<IViewRow> currentView = null;

        private JSONDataModel model = new JSONDataModel();

        public CouchbaseSourceAdapterUIForm(Connections cons, Variables vars, IDTSComponentMetaData100 md)
        {
            InitializeComponent();

            variables = vars;
            connections = cons;
            metaData = md;

            this.Text = metaData.Name + " Configuration";
            
            if (designTimeInstance == null)
                designTimeInstance = metaData.Instantiate();

            txtURL.Text = metaData.CustomPropertyCollection["url"].Value.ToString();
            txtBucket.Text = metaData.CustomPropertyCollection["bucket"].Value.ToString();
            txtPassword.Text = metaData.CustomPropertyCollection["password"].Value.ToString();
            txtDesignDoc.Text = metaData.CustomPropertyCollection["designDoc"].Value.ToString();
            txtView.Text = metaData.CustomPropertyCollection["view"].Value.ToString();
            cbForceReindex.Checked = metaData.CustomPropertyCollection["forceReindex"].Value;
            cbDescending.Checked = metaData.CustomPropertyCollection["descending"].Value;
            txtStartKey.Text = metaData.CustomPropertyCollection["startKey"].Value.ToString();
            txtEndKey.Text = metaData.CustomPropertyCollection["endKey"].Value.ToString();
            
            string modelXML = metaData.CustomPropertyCollection["modelXML"].Value.ToString();
            model = new JSONDataModel();
            if (!modelXML.Trim().Equals(""))
                model = new JSONDataModel(modelXML);
        }

        
        private void btnOK_Click(object sender, EventArgs e)
        {
            designTimeInstance.SetComponentProperty("url", txtURL.Text);
            designTimeInstance.SetComponentProperty("bucket", txtBucket.Text);
            designTimeInstance.SetComponentProperty("password", txtPassword.Text);
            designTimeInstance.SetComponentProperty("designDoc", txtDesignDoc.Text);
            designTimeInstance.SetComponentProperty("view", txtView.Text);
            designTimeInstance.SetComponentProperty("forceReindex", cbForceReindex.Checked);
            designTimeInstance.SetComponentProperty("descending", cbDescending.Checked);
            designTimeInstance.SetComponentProperty("startKey", txtStartKey.Text);
            designTimeInstance.SetComponentProperty("endKey", txtEndKey.Text);

            designTimeInstance.SetComponentProperty("modelXML", model.GetXml());

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CouchbaseSourceAdapterUIForm_Load(object sender, EventArgs e)
        {

        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            txtTestResult.Text = "";

            CouchbaseClientConfiguration config = new CouchbaseClientConfiguration();
            config.Urls.Add(new Uri(txtURL.Text.TrimEnd('/')+"/pools/"));
            config.Bucket = txtBucket.Text;
            config.BucketPassword = txtPassword.Text;


            try
            {
                client = new CouchbaseClient(config);

                txtTestResult.Text += "Connected to Couchbase...\r\n";

                var view = client.GetView(txtDesignDoc.Text, txtView.Text)
                    .Stale(cbForceReindex.Checked ? StaleMode.False : StaleMode.AllowStale)
                    .Descending(cbDescending.Checked)
                    .Limit(10);

                int rowCount = 0;
                foreach (var row in view)
                {
                    rowCount++;
                }
                txtTestResult.Text += "Found " + rowCount + " rows from view...\r\n";

                currentView = client.GetView(txtDesignDoc.Text, txtView.Text)
                    .Stale(cbForceReindex.Checked ? StaleMode.False : StaleMode.AllowStale)
                    .Descending(cbDescending.Checked);

            }
            catch (Exception ex)
            {
                currentView = null;
                MessageBox.Show("There was a problem connecting to the Couchbase Server.\nError says:\n" + ex.Message);
            }

            btnMapping.Enabled = currentView != null;
        }

        private void btnMapping_Click(object sender, EventArgs e)
        {
            // Create and display the form for the user interface.
            DataMappingForm dataMappingForm = new DataMappingForm(metaData, designTimeInstance, currentView);

            DialogResult result = dataMappingForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                model = dataMappingForm.currentModel;
            }
        }


       
    }
}
