using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CouchbaseToSSIS
{
    public class JsonObject
    {

        /// <summary>
        /// A type for a token within an XPath expression.
        /// VALUE means that the token is a property that points to a primitive value
        /// KEY  means that the token is a property that points to a dictionary
        /// INDEX means that the token is numeric and points to an object in a list
        /// </summary>
        public enum XPathTokenType
        {
            VALUE,
            KEY,
            INDEX
        }

        /// <summary>
        /// A structure to hold a token
        /// </summary>
        public class XPathToken
        {

            public string name;
            public XPathTokenType type;
        }

        /// <summary>
        /// Raw store for the value of this object
        /// </summary>
        public object data = null;

        /// <summary>
        /// The root of this object
        /// </summary>
        public JsonObject root = null;
        
        /// <summary>
        /// XPath from the root to this node
        /// </summary>
        public string path = "";

        /// <summary>
        /// Index of this object if found in a list. -1 if this is part of another object
        /// </summary>
        public int index = -1;

        /// <summary>
        /// Name of the property holding this object's value. Empty if this is a part of a list
        /// </summary>
        public string name = "";

        /// <summary>
        /// XPath token cache to speed up the tokenization process
        /// </summary>
        static Dictionary<string, XPathToken[]> cache = new Dictionary<string, XPathToken[]>();

        /// <summary>
        /// Type of the value that this object represents
        /// </summary>
        public XPathTokenType valueType { 
            get { 
                return data.GetType() == typeof(Dictionary<string,object>) 
                    ? XPathTokenType.KEY 
                    : data.GetType() == typeof(List<object>) 
                        ? XPathTokenType.INDEX
                        : XPathTokenType.VALUE;
            }
        }

        /// <summary>
        /// Constructor based on a json document
        /// </summary>
        /// <param name="jsonString">The document that would be processed</param>
        public JsonObject(string jsonString) {

            data = Json.Deserialize(jsonString);
            root = this;
            path = "";
            index = -1;
            
        }

        /// <summary>
        /// Returns a serialized view of the document
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Json.Serialize(data);
        }

        /// <summary>
        /// Fluent interface over the object to get a value from an XPath
        /// </summary>
        /// <param name="xpath">The XPath to the value</param>
        /// <returns>A JsonObject wrapping the value</returns>
        public JsonObject get(string xpath)
        {

            object fragment = data;
            XPathToken[] tokens = tokenize(xpath);
            int index = -1;
            string name = "";
            for (int i = 0; i < tokens.Length; i++)
            {
                if (fragment != null)
                {

                    switch (tokens[i].type)
                    {
                        case XPathTokenType.KEY:
                            fragment = (fragment as Dictionary<string, object>).ContainsKey(tokens[i].name) ? (fragment as Dictionary<string, object>)[tokens[i].name] : null;
                            index = -1;
                            name = tokens[i].name;
                            break;

                        case XPathTokenType.INDEX:
                            fragment = (fragment as List<object>).Count > int.Parse(tokens[i].name) ? (fragment as List<object>)[int.Parse(tokens[i].name)] : null;
                            index = int.Parse(tokens[i].name.Trim('[', ']'));
                            break;
                    }
                }
            }

            JsonObject result = new JsonObject("{}");
            result.data = fragment;
            result.root = this.root;
            result.path = (this.path.Equals("") ? "" : this.path + "/") + xpath;
            result.index = index;
            result.name = name;

            return result;

        }


        /// <summary>
        /// Runs the XPath query and typecasts the result
        /// </summary>
        /// <typeparam name="T">The Type used for the casting</typeparam>
        /// <param name="xpath">The XPath query</param>
        /// <returns>A generic type-casted result value from the XPath query</returns>
        public T get<T>(string xpath)
        {
            return (T)get(xpath).data;
        }

        /// <summary>
        /// Runs the XPath query and parsed the string representation of the result to int
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>An int-parsed result value from the XPath query</returns>
        public int getInt(string xpath) { return int.Parse(get(xpath).data == null ? "0" : get(xpath).data.ToString()); }

        /// <summary>
        /// Runs the XPath query and parsed the string representation of the result to long
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>A long-parsed result value from the XPath query</returns>
        public long getLong(string xpath) { return long.Parse(get(xpath).data == null ? "0" : get(xpath).data.ToString()); }

        /// <summary>
        /// Runs the XPath query and parsed the string representation of the result to float
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>A float-parsed result value from the XPath query</returns>
        public float getFloat(string xpath) { return float.Parse(get(xpath).data == null ? "0" : get(xpath).data.ToString()); }

        /// <summary>
        /// Runs the XPath query and parsed the string representation of the result to double
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>A double(as in class)-parsed result value from the XPath query</returns>
        public double getDouble(string xpath) { return double.Parse(get(xpath).data == null ? "0" : get(xpath).data.ToString()); }

        /// <summary>
        /// Runs the XPath query and parsed the string representation of the result to bool
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>A bool-parsed result value from the XPath query</returns>
        public bool getBool(string xpath) { return bool.Parse(get(xpath).data == null ? "false" : get(xpath).data.ToString()); }

        /// <summary>
        /// Runs the XPath query and returns the string representation of the result
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>The string representation of the result value from the XPath query</returns>
        public string getString(string xpath)
        {
            JsonObject o = get(xpath);
            return o.data != null ? o.data.ToString() : null;
        }

        /// <summary>
        /// Runs the XPath query and returns the Dictionary-casted result
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>The string representation of the result value from the XPath query</returns>
        public Dictionary<string, object> getDictionary(string xpath)
        {
            JsonObject o = get(xpath);
            return o.data != null && o.isDictionary() ? (Dictionary<string, object>)o.data : new Dictionary<string, object>();
        }

        public List<object> getList(string xpath)
        {
            JsonObject o = get(xpath);
            return o.data != null ? (List<object>)o.data : new List<object>();
        }

        /// <summary>
        /// Checks whether the underlying value is an object (Dictionary)
        /// </summary>
        /// <returns>True if the value is a Dictionary; otherwise false</returns>
        public bool isDictionary()
        {
            return this.data.GetType() == typeof(Dictionary<string, object>);
        }
                
        /// <summary>
        /// Checks whether the underlying value is an object (Dictionary)
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>True if the value is a Dictionary; otherwise false</returns>
        public bool isDictionary(string xpath)
        {
            return get(xpath).isDictionary();
        }

        /// <summary>
        /// Checks whether the underlying value is an array (List)
        /// </summary>
        /// <returns>True if the value is a List; otherwise false</returns>
        public bool isList()
        {
            return this.data.GetType() == typeof(List<object>);
        }

        /// <summary>
        /// Checks whether the underlying value is an array (List)
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>True if the value is a List; otherwise false</returns>
        public bool isList(string xpath)
        {
            return get(xpath).isList();
        }

        /// <summary>
        /// Checks whether a certain XPath query results in a non-null value
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>True if the XPath result is not null; otherwise false</returns>
        public bool exists(string xpath) { return get(xpath) != null; }
        
        /// <summary>
        /// Returns the count of elements in the case when the result value is a collection in general
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <returns>The count of values within the collection if the value is a collection; 0 otherwise</returns>
        public int count(string xpath)
        {
            object o = get(xpath);
            if (o is Dictionary<string, object>)
            {
                return (o as Dictionary<string, object>).Count;
            }
            else if (o is List<object>)
            {
                return (o as List<object>).Count;
            }
            else
                return 0;
        }

        /// <summary>
        /// Adds an item to the object at a given XPath. THe XPath must exist and must be a list
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <param name="data">The value to put at the XPath address</param>
        public void addToList(string xpath, object data)
        {
            getList(xpath).Add(data);
        }

        /// <summary>
        /// Adds an item to the object at a given XPath. THe XPath must exist and must be a dictionary
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <param name="key">The key at which to put the value</param>
        /// <param name="data">The value to put at the XPath address</param>
        public void setToDictionary(string xpath, string key, object data)
        {
            getDictionary(xpath)[key] = data;
        }

        /// <summary>
        /// Removes an item from the object at a given XPath. The XPath must exist and must be a dictionary
        /// </summary>
        /// <param name="xpath">The XPath query</param>
        /// <param name="key">The key at which to put the value</param>
        public void removeFromDictionary(string xpath, string key)
        {
            getDictionary(xpath).Remove(key);
        }
 
        /// <summary>
        /// Basic XPath tokenizer with cache support
        /// </summary>
        /// <param name="xpath">The XPath string</param>
        /// <returns>List of tokens extracted from the string</returns>
        XPathToken[] tokenize(string xpath)
        {

            if (cache.ContainsKey(xpath))
            {
                return cache[xpath];
            }

            Regex reg = new Regex("([a-zA-Z:\\-_0-9]+)|(\\[[0-9]+\\])+");
            MatchCollection matches = reg.Matches(xpath);
            XPathToken[] res = new XPathToken[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                XPathToken token = new XPathToken();

                if (Regex.IsMatch(matches[i].Value, "\\[[0-9]+\\]"))
                {
                    token.type = XPathTokenType.INDEX;
                    token.name = matches[i].Value.Trim('[', ']');
                }
                else if (Regex.IsMatch(matches[i].Value, "[a-zA-Z:\\-_0-9]+"))
                {
                    token.type = XPathTokenType.KEY;
                    token.name = matches[i].Value.TrimStart('/');
                }

                res[i] = token;

            }

            cache[xpath] = res;

            return res;
        }
    }
}