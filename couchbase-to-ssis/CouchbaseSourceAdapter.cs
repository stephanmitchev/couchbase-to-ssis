// Type: Microsoft.SqlServer.Dts.Pipeline.XmlSourceAdapter
// Assembly: Microsoft.SqlServer.XmlSrc, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// Assembly location: C:\Program Files\Microsoft SQL Server\110\DTS\PipelineComponents\Microsoft.SqlServer.XMLSrc.dll

using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Microsoft.SqlServer.Dts.Pipeline;
using Couchbase;
using Couchbase.Configuration;
using Couchbase.Management;
using System.Text.RegularExpressions;

namespace CouchbaseToSSIS
{

    // Set the Component Properties that will be used when registering in SSIS
    [DtsPipelineComponent(ComponentType = ComponentType.SourceAdapter,
        DisplayName = "Couchbase Source",
        CurrentVersion = 1,
        IconResource = "USAC.IntegrationServices.Couchbase.icons.couchbase.ico",
        RequiredProductLevel = DTSProductLevel.DTSPL_NONE,
        UITypeName = "USAC.IntegrationServices.Couchbase.CouchbaseSourceAdapterUI, USAC.IntegrationServices.Couchbase, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f50b7a5caaa78ecb")]
    public sealed class CouchbaseSourceAdapter : PipelineComponent
    {
        // A common repository for all columns found using LineageIDs for every table of the model
        private Dictionary<string, Dictionary<string, int>> columnIndexes = new Dictionary<string,Dictionary<string,int>>();

        // Provide the design-time properties and initialize the defaults
        public override void ProvideComponentProperties()
        {
            // Reset the component.
            base.RemoveAllInputsOutputsAndCustomProperties();
            ComponentMetaData.RuntimeConnectionCollection.RemoveAll();

            // Set up the connection props
            IDTSCustomProperty100 componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "url";
            componentCustomProperty.Description = "Couchbase URL (e.g. 'http://127.0.0.1:8091/')";
            componentCustomProperty.Value = "http://127.0.0.1:8091/";

            componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "bucket";
            componentCustomProperty.Description = "Bucket which contains the view";
            componentCustomProperty.Value = "";

            componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "password";
            componentCustomProperty.Description = "Bucket password for the Couchbase cluster";
            componentCustomProperty.Value = "secret";

            componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "designDoc";
            componentCustomProperty.Description = "Design Document that contains the view";
            componentCustomProperty.Value = "ssis";

            componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "view";
            componentCustomProperty.Description = "View which has datetime as key and null as value";
            componentCustomProperty.Value = "allForms";

            componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "forceReindex";
            componentCustomProperty.Description = "Whether to show stale data or not";
            componentCustomProperty.Value = true;

            componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "descending";
            componentCustomProperty.Description = "Whether to orderthe view results in descending mode by key";
            componentCustomProperty.Value = false;

            componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "startKey";
            componentCustomProperty.Description = "Whether to return results greater or equal to the startKey";
            componentCustomProperty.Value = "";
            componentCustomProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NOTIFY;
            
            componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "endKey";
            componentCustomProperty.Description = "Whether to return results lesser or equal to the endKey";
            componentCustomProperty.Value = "";
            componentCustomProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NOTIFY;
            
            componentCustomProperty = ComponentMetaData.CustomPropertyCollection.New();
            componentCustomProperty.Name = "modelXML";
            componentCustomProperty.Description = "XML Representation of the mapping model";
            componentCustomProperty.Value = "";

            /// We wont be adding outputs here as those will come during the ReinitializeMetaData call
            /// when Validate checks the outputs against the model
        }

        // This will be a pass-through for now
        public override void PerformUpgrade(int pipelineVersion)
        {
            // Obtain the current component version from the attribute.
            DtsPipelineComponentAttribute componentAttribute = (DtsPipelineComponentAttribute)Attribute
                .GetCustomAttribute(this.GetType(), typeof(DtsPipelineComponentAttribute), false);

            int currentVersion = componentAttribute.CurrentVersion;

            if (ComponentMetaData.Version < currentVersion) { }

            // Update the saved component version metadata to the current version.
            ComponentMetaData.Version = currentVersion;
        }

        // The Validate phase will be checking the model against its outputs
        // and will ensure that all properties are set correctly
        public override DTSValidationStatus Validate()
        {
            bool pbCancel = false;

            // Validate that the url custom property is set.
            if (ComponentMetaData.CustomPropertyCollection["url"].Value == null
                || ((string)ComponentMetaData.CustomPropertyCollection["url"].Value).Length == 0)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "The URL property must be set.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISBROKEN;
            }

            // Validate that the bucket custom property is set.
            if (ComponentMetaData.CustomPropertyCollection["bucket"].Value == null
                || ((string)ComponentMetaData.CustomPropertyCollection["bucket"].Value).Length == 0)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "The Bucket property must be set.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISBROKEN;
            }

            // Validate that the password custom property is set.
            if (ComponentMetaData.CustomPropertyCollection["password"].Value == null
                || ((string)ComponentMetaData.CustomPropertyCollection["password"].Value).Length == 0)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "The Password property must be set.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISBROKEN;
            }

            // Validate that the designDoc custom property is set.
            if (ComponentMetaData.CustomPropertyCollection["designDoc"].Value == null
                || ((string)ComponentMetaData.CustomPropertyCollection["designDoc"].Value).Length == 0)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "The Design Document property must be set.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISBROKEN;
            }

            // Validate that the view custom property is set.
            if (ComponentMetaData.CustomPropertyCollection["view"].Value == null
                || ((string)ComponentMetaData.CustomPropertyCollection["view"].Value).Length == 0)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "The View property must be set.", "", 0, out pbCancel);
                return DTSValidationStatus.VS_ISBROKEN;
            }

            // Validate outputs against the model
            JSONDataModel model = getModel();

            // If the output count does not match, regenerate it
            if (ComponentMetaData.OutputCollection.Count != model.Tables.Count)
            {
                ComponentMetaData.FireError(0, ComponentMetaData.Name, "Number of outputs must match the number of tables in the model", "", 0, out pbCancel);
                return DTSValidationStatus.VS_NEEDSNEWMETADATA;
            }

            // Inpect every table
            foreach (DataTable table in model.Tables)
            {
                string name = table.TableName;
                IDTSOutput100 output = ComponentMetaData.OutputCollection[name];

                // If the column count does not match, or the specific output is not found, regenerate it
                if (output == null || output.OutputColumnCollection.Count != table.Rows.Count)
                {
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }

                // If the columns do not match, regenerate it
                foreach (DataRow row in table.Rows)
                {
                    string colName = row["shortColumn"].ToString();
                    IDTSOutputColumn100 outputColumn = output.OutputColumnCollection[colName];

                    // If the specific column is not found, regenerate it
                    if (outputColumn == null)
                    {
                        return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                    }
                }
            }

            // Let the base class verify that the input column reflects the output 
            // of the upstream component.
            return base.Validate();
        }
        
        // Rebuild the component outputs based on the model
        public override void ReinitializeMetaData()
        {
            // Clean out the outputs
            ComponentMetaData.OutputCollection.RemoveAll();

            // Get the model
            JSONDataModel model = getModel();

            // For every output table model crete the SSIS outputs
            foreach (DataTable table in model.Tables)
            {
                // Define a new output
                IDTSOutput100 output = ComponentMetaData.OutputCollection.New();
                output.Name = table.TableName;

                // For every column definition in the model, create the corresponding output column
                foreach (DataRow row in table.Rows)
                {
                    // Create a new Output Column
                    IDTSOutputColumn100 outColumn = output.OutputColumnCollection.New();
                    
                    // Set column data type properties.
                    bool isLong = false;
                    
                    // Assume string for missing datatypes
                    Type dataType = row["datatype"].Equals("")
                        ? typeof(string)
                        : Type.GetType(row["datatype"].ToString());

                    // Translate the datatype into the SSIS intermediate type
                    DataType dt = DataRecordTypeToBufferType(dataType);
                    dt = ConvertBufferDataTypeToFitManaged(dt, ref isLong);
                    
                    // Enforce numeric datatypes
                    switch (dt)
                    {
                        case DataType.DT_R4:
                        case DataType.DT_R8:
                        case DataType.DT_DECIMAL:
                            dt = DataType.DT_NUMERIC;
                            break;

                        case DataType.DT_BOOL:
                        case DataType.DT_I1:
                        case DataType.DT_I2:
                        case DataType.DT_I4:
                           dt = DataType.DT_I4;
                            break;

                        case DataType.DT_UI1:
                        case DataType.DT_UI2:
                        case DataType.DT_UI4:
                            dt = DataType.DT_UI4;
                            break;

                        case DataType.DT_DBTIMESTAMP:
                        case DataType.DT_DBTIMESTAMP2:
                        case DataType.DT_DBDATE:
                        case DataType.DT_DATE:
                        case DataType.DT_FILETIME:
                            dt = DataType.DT_DBTIMESTAMP;
                            break;



                    }


                    // Assume defaults and limits
                    int length = 0;
                    int precision = 2000;
                    int scale = 0;
                    int codepage = table.Locale.TextInfo.ANSICodePage;

                    // Handle the datatype cases
                    switch (dt)
                    {
                        // The length cannot be zero, and the code page property must contain a valid code page.
                        case DataType.DT_STR:
                        case DataType.DT_TEXT:
                            length = precision;
                            precision = 0;
                            scale = 0;
                            break;

                        case DataType.DT_WSTR:
                            length = precision;
                            codepage = 0;
                            scale = 0;
                            precision = 0;
                            break;

                        case DataType.DT_NUMERIC:
                            length = 0;
                            codepage = 0;
                            precision = 24;
                            scale = 6;
                            break;

                        default:
                            length = 0;
                            precision = 0;
                            codepage = 0;
                            scale = 0;
                            break;

                    }


                    // Set the properties of the output column.
                    outColumn.Name = (string)row["shortColumn"];
                    outColumn.Description = (string)row["xpath"];
                    outColumn.SetDataTypeProperties(dt, length, precision, scale, codepage);

                    // Set the properties of the metadata column to facilitate automatic binding
                    IDTSExternalMetadataColumn100 extColumn = output.ExternalMetadataColumnCollection.New();
                    extColumn.Name = (string)row["shortColumn"];
                    extColumn.Description = (string)row["xpath"];
                    extColumn.DataType = dt;
                    extColumn.Length = length;
                    extColumn.Precision = precision;
                    extColumn.Scale = scale;
                    extColumn.CodePage = codepage;
                    extColumn.MappedColumnID = outColumn.ID;
                  
                }
            }

        }

        /// <summary>
        /// Deserialize the model stored during design time
        /// </summary>
        /// <returns>A JSONDataModel</returns>
        public JSONDataModel getModel()
        {
            string modelXML = ComponentMetaData.CustomPropertyCollection["modelXML"].Value.ToString();

            return new JSONDataModel(modelXML);
        }

        // In the PreExecute phase we will be caching the column indexes looked up by the LineageID
        public override void PreExecute()
        {
            // Initialize the cache
            columnIndexes = new Dictionary<string,Dictionary<string,int>>();

            // Get the model
            JSONDataModel model = getModel();

            // For each table definition
            foreach (DataTable table in model.Tables) {

                // Find the ouput
                IDTSOutput100 output = ComponentMetaData.OutputCollection[table.TableName];

                // ... and initialize the corresponding cache index
                columnIndexes[table.TableName] = new Dictionary<string,int>();

                // For every column
                foreach (IDTSOutputColumn100 col in output.OutputColumnCollection)
                {
                    // We wrap this in a try-catch without exception handling so that 
                    // we allow for a subset of the outputs to be used. If an output is not
                    // connected, the columns defined in that output will not be in the Buffer
                    try
                    {
                        // Cache the buffer column index
                        columnIndexes[table.TableName][col.Name] = BufferManager.FindColumnByLineageID(output.Buffer, col.LineageID);
                    }
                    catch
                    {
                        // Do nothing for now
                    }
                }
            }
        }

        // The main data flow function
        public override void PrimeOutput(int outputs, int[] outputIDs, PipelineBuffer[] buffers)
        {
            // Get teh model
            JSONDataModel model = getModel();
            
            // Initialize Couchbase Client
            CouchbaseClientConfiguration config = new CouchbaseClientConfiguration();
            config.Urls.Add(new Uri(ComponentMetaData.CustomPropertyCollection["url"].Value.ToString().TrimEnd('/')+"/pools/"));
            config.Bucket = ComponentMetaData.CustomPropertyCollection["bucket"].Value.ToString();
            config.BucketPassword = ComponentMetaData.CustomPropertyCollection["password"].Value.ToString();
            CouchbaseClient client = new CouchbaseClient(config);

            // Extract the parameters
            string designDoc = ComponentMetaData.CustomPropertyCollection["designDoc"].Value.ToString();
            string viewName = ComponentMetaData.CustomPropertyCollection["view"].Value.ToString();
            bool forceReindex = (bool)ComponentMetaData.CustomPropertyCollection["forceReindex"].Value;
            bool descending = (bool)ComponentMetaData.CustomPropertyCollection["descending"].Value;


            // Define the view to be executed
            IView<IViewRow> view = ((IView<IViewRow>)client.GetView(designDoc, viewName))
                .Stale(forceReindex ? StaleMode.False : StaleMode.AllowStale)
                .Descending(descending);

            // Extract the variables from the package
            IDTSVariables100 variables = null; 
            
            // StartKey can be set from another task prior of running this task
            string startKey = ComponentMetaData.CustomPropertyCollection["startKey"].Value;
            if (startKey != null && startKey.StartsWith("@"))
            {
                VariableDispenser.LockOneForRead(startKey.Substring(1), ref variables);
                startKey = variables[0].Value.ToString();
                variables.Unlock();

                ComponentMetaData.PostLogMessage("Couchbase", ComponentMetaData.Name, "Found a variable StartKey. Using " + startKey + " as value.", DateTime.Now, DateTime.Now, 0, null);
            }
            
            // EndKey can be set from another task prior of running this task
            string endKey = ComponentMetaData.CustomPropertyCollection["endKey"].Value;
            if (endKey != null && endKey.StartsWith("@"))
            {
                VariableDispenser.LockOneForRead(endKey.Substring(1), ref variables);
                endKey = variables[0].Value.ToString();
                variables.Unlock();
                
                ComponentMetaData.PostLogMessage("Couchbase", ComponentMetaData.Name, "Found a variable EndKey. Using " + endKey + " as value.", DateTime.Now, DateTime.Now, 0, null);
            }
            
            // Apply variables to the view if necessary
            if (startKey != null && !startKey.Equals(""))
            {
                view = view.StartKey<string>(startKey);
            }
            
            if (endKey != null && !endKey.Equals(""))
            {
                view = view.EndKey<string>(endKey);
            }
            
            // Iterate over each document returned by the view
            foreach (IViewRow row in view)
            {
                // Say that we have read it
                ComponentMetaData.IncrementPipelinePerfCounter(101, 1);

                // Write it out to the outputs
                writeDocToBuffers(row.ItemId, row.GetItem().ToString(), model, outputIDs, buffers);

                // Say that we wrote it
                ComponentMetaData.IncrementPipelinePerfCounter(103, 1);

            }

            // Flush out all buffers and get outta here
            foreach (PipelineBuffer buffer in buffers)
            {
                /// Notify the data flow task that no more rows are coming.
                buffer.SetEndOfRowset();
            }
        }

        /// <summary>
        /// Write a json document to the output buffers
        /// </summary>
        /// <param name="id">The document key</param>
        /// <param name="json">The JSON representation of the document</param>
        /// <param name="model">A model used to decompose the document</param>
        /// <param name="outputIDs">Indexes of the output buffers</param>
        /// <param name="buffers">The Pipline Buffers</param>
        private void writeDocToBuffers(string id, string json, JSONDataModel model, int[] outputIDs, PipelineBuffer[] buffers)
        {
            // Deserialize the document
            JsonObject jp = new JsonObject(json);
           
            // For each model table
            foreach (DataTable modelTable in model.Tables)
            {
                // Obtain the output
                IDTSOutput100 output = ComponentMetaData.OutputCollection[modelTable.TableName];

                // Continue if output is not connected
                if (Array.IndexOf(outputIDs, output.ID) == -1)
                {
                    continue;
                }
                
                // Obtain the buffer tho write to
                PipelineBuffer buffer = buffers[Array.IndexOf(outputIDs, output.ID)];
               
                // If we are processing a table that has indexes on the second column
                // this must be a child table
                if (modelTable.Rows.Count > 2 && modelTable.Rows[1]["xpath"].ToString().StartsWith("#"))
                {
                    // Go and process the related table
                    processChildTable(id, jp, model, modelTable.TableName, outputIDs, buffers);
                }
                else
                {
                    // Add a new row to the buffer
                    buffer.AddRow();

                    // For every column definition in teh model
                    foreach (DataRow row in modelTable.Rows)
                    {
                        // Find the column index that the buffer will recognize
                        int columnIndex = columnIndexes[modelTable.TableName][row["shortColumn"].ToString()];

                        // Get the model datatype
                        Type dataType = Type.GetType(row["datatype"].ToString());

                        // And XPath
                        string xpath = row["xpath"].ToString();

                        // Handle special case of DOC_ID
                        if (xpath.Equals("#ID"))
                        {
                            buffer.SetString(columnIndex, id);
                        }
                        else
                        {
                            // Write all other fields as usual
                            writeDataToBuffer(jp, buffer, columnIndex, dataType, xpath);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Writes out a child table. Uses an iterative approach. Would have been much more 
        /// readable if this was recursive, but we really should not be using it.
        /// </summary>
        /// <param name="id">The document key</param>
        /// <param name="jp">A deserialized object holding the document</param>
        /// <param name="model">A model used to decompose the document</param>
        /// <param name="tableName">The name of the child table</param>
        /// <param name="outputIDs">Indexes of the output buffers</param>
        /// <param name="buffers">The Pipline Buffers</param>
        void processChildTable(string id, JsonObject jp, JSONDataModel model, string tableName, int[] outputIDs, PipelineBuffer[] buffers)
        {
            // Obtain references to the model table, output and buffer
            DataTable modelTable = model.Tables[tableName];
            IDTSOutput100 output = ComponentMetaData.OutputCollection[modelTable.TableName];
            PipelineBuffer buffer = buffers[Array.IndexOf(outputIDs, output.ID)];
            
            // The counters will track how many levels of nesting we have
            int counters = 0;

            // We will first represent all records that need to be written out as XPaths
            List<List<string>> xPathRows = new List<List<string>>();

            //
            List<string> xPathRow = new List<string>();

            // Collect counters and initial row XPaths
            foreach (DataRow row in modelTable.Rows)
            {
                
                string xPath = row["xpath"].ToString();

                if (xPath.Equals("#ID"))
                {
                    // Nothing for now
                }
                else if (xPath.StartsWith("#"))
                {
                    counters++;
                }

                xPathRow.Add(xPath);
                
            }

            // Add the initial Row
            xPathRows.Add(xPathRow);

            // Iteratively expand each row based on the indexes found
            for (int i = 0; i < counters; i++)
            {
                xPathRows = expandRows(jp, i, xPathRows, modelTable);
            }

            // Iterate over the expanded rows. They should contain 
            // full XPaths to be used to obtain the values for each cell
            foreach (List<string> row in xPathRows)
            {
                // Add a row to the buffer
                buffer.AddRow();

                // Keep track of which column we are looking at
                int colIdx = 0;

                // For each column definition in the model
                foreach (DataRow modelCol in modelTable.Rows)
                {
                    // Get the name and the index
                    string col = modelCol["shortColumn"].ToString();
                    int columnIndex = columnIndexes[modelTable.TableName][col];

                    // Handle special case of DOC_ID
                    if (row[colIdx].Equals("#ID"))
                    {
                        buffer.SetString(columnIndex, id);
                    }
                    // Handle special case of child table indexes
                    else if (row[colIdx].StartsWith("#"))
                    {
                        // Obtain the nesting index from the XPath by looking at the first index found
                        int counterIndex = int.Parse(row[colIdx].Substring(1));
                        
                        // Extract the current value of the index
                        Match m = Regex.Match(row[row.Count - 1], "\\[([0-9]+)\\]");
                        long index = long.Parse(m.Groups[counterIndex + 1].Value);

                        // and write it out to the buffer
                        buffer.SetInt64(columnIndex, index);
                    }
                    else
                    {
                        // Write all other fields as usual
                        Type dataType = Type.GetType(modelCol["datatype"].ToString());

                        string xpath = row[colIdx];

                        writeDataToBuffer(jp, buffer, columnIndex, dataType, xpath);
                    }
                    
                    // Increment to look at the next column
                    colIdx++;
                }
            }
        }

        /// <summary>
        /// Writes a specific cell to the output buffer based on its datatype
        /// </summary>
        /// <param name="jp">A deserialized object holding the document</param>
        /// <param name="buffer">The buffer we will write to</param>
        /// <param name="columnIndex">The index of the column we will write to</param>
        /// <param name="dataType">The data type of the value being written</param>
        /// <param name="xpath">The XPath of the cell that will be looked up in the jp</param>
        private static void writeDataToBuffer(JsonObject jp, PipelineBuffer buffer, int columnIndex, Type dataType, string xpath)
        {
            // Really self-explanatory
            if (dataType == typeof(long))
            {
                buffer.SetInt64(columnIndex, jp.getLong(xpath));
            }
            else if (dataType == typeof(double))
            {
                buffer.SetDecimal(columnIndex, new Decimal(jp.getDouble(xpath)));
            }
            else if (dataType == typeof(bool))
            {
                buffer.SetInt32(columnIndex, jp.getBool(xpath) ? 1 : 0);
            }
            else if (dataType == typeof(DateTime))
            {
                DateTime dt;
                if (jp.getString(xpath) != null && !jp.getString(xpath).Trim().Equals("") && DateTime.TryParse(jp.getString(xpath), out dt))
                    buffer.SetDateTime(columnIndex, DateTime.Parse(jp.getString(xpath)));

            }
            else if (dataType == typeof(string))
            {
                // Write the strings if no null and trim them down to 2k size, beware of UTF-16
                string value = jp.getString(xpath);
                if (value != null)
                {
                    int byteSize = System.Text.ASCIIEncoding.Unicode.GetByteCount(value);
                    if (byteSize > 2000)
                    {
                        int limit = (int)(2000 * value.Length * 1f / byteSize);
                        buffer.SetString(columnIndex, value.Substring(0, Math.Min(limit , value.Length)));
                    }
                    else
                    {
                        buffer.SetString(columnIndex, value);
                    }
                    
                }
            }
        }

        /// <summary>
        /// Iteratively expands rows that include concrete XPaths agains the currently loaded document
        /// </summary>
        /// <param name="jp">A deserialized object holding the document</param>
        /// <param name="counterIndex">Which level of nesting we will be expanding</param>
        /// <param name="xPathRows">The rows that contain the non-exapnded set</param>
        /// <param name="model">A model table used to decompose the document</param>
        /// <returns>An expanded set of rows based on the nesting index</returns>
        private List<List<string>> expandRows(JsonObject jp, int counterIndex, List<List<string>> xPathRows, DataTable modelTable)
        {
            // Create a new result
            List<List<string>> result = new List<List<string>>();

            // For every row that requires expansion
            foreach (List<string> row in xPathRows)
            {
                // Last column must contain a counter as the indexes and ids are in the beginning
                string xPath = row[row.Count - 1];
                
                // Find the n-th ocurrence of an index where n == counterIndex
                int cIdx = 0;
                for (int i = 0; i <= counterIndex; i++)
                {
                    cIdx = xPath.IndexOf("[", cIdx + 1);
                }

                // Obtain the real count within the document that we are currently writing out
                int resultRowCount = jp.getList(xPath.Substring(0, cIdx)).Count;

                // For every entry found
                for (int i = 0; i < resultRowCount; i++) {

                    // Create a new expanded row
                    List<string> newRow = new List<string>(row);

                    // and for every cell of that row
                    for (int j = 0; j < newRow.Count; j++)
                    {
                        // Skipping the special cases
                        if (!newRow[j].StartsWith("#"))
                        {
                            // Create a sequential index within the XPath expression
                            newRow[j] = newRow[j].Substring(0, cIdx)
                                + "[" + i + "]" + newRow[j].Substring(cIdx + 3);
                        }
                    }

                    // Add the expanded row back to the result set
                    result.Add(newRow);
                }
            }

            // Return the expanded table
            return result;
        }
    }
}
