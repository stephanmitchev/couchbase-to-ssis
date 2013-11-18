using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;

namespace CouchbaseToSSIS
{
    public class JSONDataModel : DataSet
    {

        /// <summary>
        /// Finds duplicated values within the model for a given table annd within a given column
        /// </summary>
        /// <param name="tableName">The table to look into</param>
        /// <param name="columnName">The column name to inspect</param>
        /// <returns>A list of DataRow objects that contains the rows that collide with other rows over the selected column</returns>
        public List<DataRow> findCollisions(string tableName, string columnName)
        {
            List<DataRow> result = new List<DataRow>();

            Dictionary<string, int> collisions = new Dictionary<string, int>();

            if (this.Tables.Contains(tableName) && this.Tables[tableName].Columns.Contains(columnName))
            {

                foreach (DataRow row in this.Tables[tableName].Rows)
                {
                    string data = row[columnName].ToString();

                    if (collisions.ContainsKey(data))
                    {
                        collisions[data]++;

                        if (collisions[data] >= 1)
                        {
                            result.Add(row);
                        }
                    }
                    else
                    {
                        collisions[data] = 0;
                    }
                }
            }



            return result;
        }

        /// <summary>
        /// Constructs an empty model for internal use only
        /// </summary>
        public JSONDataModel()
            : base()
        {

        }

        /// <summary>
        /// Deserializes a model from an XML representation
        /// </summary>
        /// <param name="xml"></param>
        public JSONDataModel(string xml)
            : base()
        {
            this.ReadXml(new StringReader(xml));
        }

        /// <summary>
        /// Constructs the model from a given JSON document
        /// </summary>
        /// <param name="json">THe JSON document to convert</param>
        /// <param name="rootXPath">The XPath expression that points to the root from where the DataSet will be generated</param>
        /// <param name="tableNameXPath">The XPath expression that points to the name of the main parent table</param>
        /// <param name="logMessages">Log messages from the generation process</param>
        /// <returns></returns>
        public JSONDataModel(string json, string rootXPath, string tableNameXPath, out string logMessages) : base()
        {
            generateModel(json, rootXPath, tableNameXPath, out logMessages);
        }

        /// <summary>
        /// Creates a copy of the current data model
        /// </summary>
        /// <returns>A copy of the current data model</returns>
        public new JSONDataModel Copy()
        {
            return (JSONDataModel)base.Copy();
        }

        /// <summary>
        /// Computes the most general type between the two inputs
        /// </summary>
        /// <param name="type1">String representation of the first datatype</param>
        /// <param name="type2">String representation of the second datatype</param>
        /// <returns>The datatype that could hold values from both input datatypes</returns>
        protected static string getTypePrecedence(string type1, string type2)
        {

            // if we have an invalid type, return string
            try
            {
                Type.GetType(type1);
                Type.GetType(type2);
            }
            catch
            {
                return typeof(string).ToString();
            }

            // if types are the same, return the first one
            if (type1.Equals(type2))
            {
                return type1;
            }

            // Anything paired with a string results in a string
            if (type1.Equals(typeof(string).ToString()) || type2.Equals(typeof(string).ToString()))
            {
                return typeof(string).ToString();
            }

            // DateTime pared with anything else results in a string
            if (!type1.Equals(type2) && (type1.Equals(typeof(DateTime).ToString()) || type2.Equals(typeof(DateTime).ToString())))
            {
                return typeof(string).ToString();
            }

            // Order of precendence in the numeric types
            List<string> precedence = new List<string>(){
                typeof(bool).ToString(),
                typeof(long).ToString(),
                typeof(double).ToString()
            };

            // Return the order of precedence
            if (precedence.IndexOf(type1) > precedence.IndexOf(type2))
            {
                return type1;
            }
            else
            {
                return type2;
            }

        }



        /// <summary>
        /// Performs a deep merge between two model datasets by comparing column definition rows by 
        /// their first element, and enforcing type precedence over the second elements
        /// </summary>
        /// <param name="ds1">First Dataset</param>
        /// <param name="model">Model to merge with</param>
        /// <returns>A merged vercion of both datasets. The merged version prefers the first dataset's entries
        /// for column indexes greater than 1</returns>
        public JSONDataModel mergeWith(DataSet model)
        {
            // The result will begin as a copy of the first dataset
            JSONDataModel result = this.Copy();

            // Compare tables
            foreach (DataTable table2 in model.Tables)
            {
                // Add the table if missing
                if (!result.Tables.Contains(table2.TableName))
                {
                    result.Tables.Add(table2.TableName);
                    foreach (DataColumn col2 in table2.Columns)
                    {
                        DataColumn col1 = new DataColumn(col2.ColumnName);
                        col1.DataType = col2.DataType;
                        result.Tables[table2.TableName].Columns.Add(col1);
                    }
                }

                // Cache the table from the first dataset
                DataTable table1 = result.Tables[table2.TableName];

                // Compare the rows and perform the merge
                foreach (DataRow row2 in table2.Rows)
                {

                    // Find the row by comparing the first column which holds the column name of the output
                    int rowIndex = -1;
                    for (int i = 0; i < table1.Rows.Count; i++)
                    {
                        if (row2[0].Equals(table1.Rows[i][0]))
                        {
                            rowIndex = i;
                            break;
                        }
                    }

                    // If the row exists, merge the datatype stored in the second column
                    if (rowIndex >= 0)
                    {
                        table1.Rows[rowIndex][1] = getTypePrecedence(table1.Rows[rowIndex][1].ToString(), row2[1].ToString());
                    }
                    // otherwise copy the row
                    else
                    {
                        table1.ImportRow(row2);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Generates a descriptive relational model from a sample JSON file
        /// </summary>
        /// <param name="json">THe JSON document to convert</param>
        /// <param name="rootXPath">The XPath expression that points to the root dictionary from where the DataSet will be generated</param>
        /// <param name="tableNameXPath">The XPath expression that points to the name of the main parent table</param>
        /// <param name="logMessages">Log messages from the generation process</param>
        /// <returns>A DataSet that contains a schema representation infered from the sample document.
        /// Tables within the DataSet map to output tables. Each table contains three columns: column, datatype, and xpath.
        /// Column holds the output column name. Datatype is a string representation of the datatype infered from the value.
        /// Xpath is a generalized pattern to access a specific value. During runtime, the xpath will be expanded for specific values of the indices
        /// </returns>
        protected void generateModel(string json, string rootXPath, string tableNameXPath, out string logMessages)
        {
            JsonObject jp = new JsonObject(json);

            StringBuilder logs = new StringBuilder();

            if (jp.get(rootXPath) == null || jp.get(rootXPath).valueType != JsonObject.XPathTokenType.KEY)
            {
                logs.AppendLine("Root property must contain an object. Null, value, or list found.");
            }
            else if (jp.get(tableNameXPath) == null || jp.get(tableNameXPath).Equals(""))
            {
                logs.AppendLine("Table Name XPath must point to a valid value within the document.");
            }
            else
            {
                modelObject(jp.get(rootXPath), new List<List<string>>(), 0, jp.getString(tableNameXPath), "");
            }

            logMessages = logs.ToString();
        }

        /// <summary>
        /// Generates a model for an object (viewed as a Dictionary)
        /// </summary>
        /// <param name="jp">JsonObject holding the JSON Document</param>
        /// <param name="ansestry">Holds a list of indexing columns using when processing nested arrays</param>
        /// <param name="nestedTableIdx">The index sequential number</param>
        /// <param name="table">Specific table name to be used in the model. If empty, the property name of the JsonObject will be used.</param>
        /// <param name="columnPrefix">Prefix to use when naming columns (using in nesting)</param>
        protected void modelObject(JsonObject jp, List<List<string>> ansestry, int nestedTableIdx, string table, string columnPrefix)
        {
            string tableName = table.Equals("")
                ? jp.name
                : table;

            foreach (string prop in jp.getDictionary("").Keys)
            {
                JsonObject j = jp.get(prop);

                string columnName = columnPrefix.Equals("")
                    ? j.name
                    : columnPrefix + "_" + j.name;

                switch (j.valueType)
                {
                    case JsonObject.XPathTokenType.VALUE:

                        createTableIfMissing(tableName, ansestry);
                        this.Tables[tableName].Rows.Add(columnName, inferDataType(j.data).ToString(), j.path, getAbbreviatedName(columnName, 30));

                        break;

                    case JsonObject.XPathTokenType.KEY:
                        modelObject(j, ansestry, nestedTableIdx, table, columnName);
                        break;

                    case JsonObject.XPathTokenType.INDEX:

                        ansestry.Add(new List<string>() { tableName + "_" + jp.name + "_index", "#" + nestedTableIdx });
                        nestedTableIdx++;

                        modelList(j, ansestry, nestedTableIdx, tableName + "_" + jp.name + "_" + j.name, "");

                        ansestry.RemoveAt(ansestry.Count - 1);
                        break;
                }
            }
        }

        /// <summary>
        /// Generates a model for a list (viewed as a List)
        /// </summary>
        /// <param name="jp">JsonObject holding the JSON Document</param>
        /// <param name="ansestry">Holds a list of indexing columns using when processing nested arrays</param>
        /// <param name="nestedTableIdx">The index sequential number</param>
        /// <param name="table">Specific table name to be used in the model. If empty, the property name of the JsonObject will be used.</param>
        /// <param name="columnPrefix">Prefix to use when naming columns (using in nesting)</param>
        protected void modelList(JsonObject jp, List<List<string>> ansestry, int nestedTableIdx, string table, string columnPrefix)
        {
            string tableName = table.Equals("")
                ? jp.name
                : table;

            int index = 0;
            foreach (object row in jp.getList(""))
            {
                // Create a brand new fork of the current JsonObject to process a child table
                JsonObject j = new JsonObject("{}");
                j.data = row;
                j.index = index;
                j.name = "";
                j.path = jp.path + "[" + index + "]";
                j.root = jp.root;

                switch (j.valueType)
                {
                    case JsonObject.XPathTokenType.VALUE:

                        string columnName = columnPrefix.Equals("")
                            ? j.name
                            : columnPrefix + "_" + j.name;

                        createTableIfMissing(tableName, ansestry);

                        this.Tables[tableName].Rows.Add(columnName, inferDataType(j.data).ToString(), j.path, getAbbreviatedName(columnName, 30));

                        break;

                    case JsonObject.XPathTokenType.KEY:

                        modelObject(j, ansestry, nestedTableIdx, table, j.name);

                        break;

                    case JsonObject.XPathTokenType.INDEX:

                        throw new InvalidOperationException("Array elements cannot be arrays as we cannot infer the table structure from anonymous values.");
                }

                return;
            }
        }

        /// <summary>
        /// A helper method to intitialize a model table
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="ansestry">A set of column definitions that represent the nesting properties of the table</param>
        /// <param name="model">The model to update</param>
        protected void createTableIfMissing(string tableName, List<List<string>> ansestry)
        {
            if (!this.Tables.Contains(tableName))
            {
                // Create the table and add default columns
                DataTable table = new DataTable(tableName);
                table.Columns.Add("column", typeof(string));
                table.Columns.Add("datatype", typeof(string));
                table.Columns.Add("xpath", typeof(string));
                table.Columns.Add("shortColumn", typeof(string));

                // Add line items for the reference to the root table
                table.Rows.Add("DOC_ID", typeof(string), "#ID", "DOC_ID");

                // Add indexers for each level of nesting based on the ansestry
                foreach (List<string> row in ansestry)
                {
                    table.Rows.Add(row[0], typeof(long), row[1], getAbbreviatedName(row[0], 30));
                }

                this.Tables.Add(table);
            }
        }

        /// <summary>
        /// Infers a datatype from a value
        /// </summary>
        /// <param name="value">The value to analyse</param>
        /// <returns>A Type that corresponds to the value</returns>
        protected Type inferDataType(object value)
        {
            Type result = null;
            DateTime dtRes;

            // Handle strong types
            if (value.GetType().Equals(typeof(Boolean)))
                result = typeof(Boolean);
            else if (value.GetType().Equals(typeof(double)))
                result = typeof(double);
            else if (value.GetType().Equals(typeof(long)))
                result = typeof(long);
            else
            {
                // It should be a string of some sort
                string valueStr = value.ToString();

                if (DateTime.TryParse(value.ToString(), out dtRes))
                    result = typeof(DateTime);
            }

            // Its just a sime string
            if (result == null)
                result = typeof(string);

            return result;

        }

        /// <summary>
        /// Attempts to abbreviate a string so that it fits a character limit by removing vowels starting from the tail
        /// </summary>
        /// <param name="name">The string to be abbreviated</param>
        /// <param name="charLimit">The character limit</param>
        /// <returns>The abbreviated string</returns>
        protected string getAbbreviatedName(string name, int charLimit)
        {
            // Return if the name is equal or below the char limit
            if (name.Length <= charLimit)
            {
                return name;
            }

            string res = name;
            MatchCollection matches = Regex.Matches(name, "([a-zA-Z]+|[0-9]+)");

            char[] vowels = new char[] { 'a', 'e', 'o', 'u', 'i' };

            // Incrementatlly remove vowels until you get below or at the char limit
            int idx = res.LastIndexOfAny(vowels);

            while (res.Length > charLimit && idx >= 0)
            {
                idx = res.LastIndexOfAny(vowels);

                if (idx >= 0)
                {
                    res = res.Remove(res.LastIndexOfAny(vowels), 1);
                }

            }

            return res;
        }

        
        

    }
}
