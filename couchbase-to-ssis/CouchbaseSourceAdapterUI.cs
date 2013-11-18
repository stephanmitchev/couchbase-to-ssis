using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Design;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace CouchbaseToSSIS
{
    class CouchbaseSourceAdapterUI : IDtsComponentUI
    {
        IDTSComponentMetaData100 md;
        
        public void Help(System.Windows.Forms.IWin32Window parentWindow)
        {
        }
        public void New(System.Windows.Forms.IWin32Window parentWindow)
        {
        }
        public void Delete(System.Windows.Forms.IWin32Window parentWindow)
        {
        }
        public bool Edit(System.Windows.Forms.IWin32Window parentWindow, Variables vars, Connections cons)
        {
            // Create and display the form for the user interface.
            CouchbaseSourceAdapterUIForm componentEditor = new CouchbaseSourceAdapterUIForm(cons, vars, md);

            DialogResult result = componentEditor.ShowDialog(parentWindow);

            if (result == DialogResult.OK)
                return true;

            return false;
        }
        public void Initialize(IDTSComponentMetaData100 dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            // Store the component metadata.
            this.md = dtsComponentMetadata;

        }
    }
}
