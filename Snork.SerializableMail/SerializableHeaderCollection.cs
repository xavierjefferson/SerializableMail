using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Snork.SerializableMail
{
    [Serializable]
    public class SerializableHeaderCollection : Dictionary<string, string>
    {
        public static implicit operator SerializableHeaderCollection(NameValueCollection collection)
        {
            var headers = new SerializableHeaderCollection();
            foreach (string key in collection.Keys)
                headers[key] = collection[key];
            return headers;
        }
    }
}