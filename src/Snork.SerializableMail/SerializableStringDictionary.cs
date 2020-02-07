using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Snork.SerializableMail
{
    [Serializable]
    public class SerializableStringDictionary : Dictionary<string, string>, IXmlSerializable
    {
        private const string ElementName = "Item";
        private const string NameAttribute = "Name";
        private const string ValueAttribute = "Value";

        public XmlSchema GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
                return;

            while (reader.Read()
                   && reader.NodeType != XmlNodeType.EndElement
                   && reader.NodeType != XmlNodeType.None)
                if (reader.NodeType == XmlNodeType.Element && reader.LocalName == ElementName)
                {
                    reader.MoveToAttribute(NameAttribute);
                    var name = reader.Value;
                    reader.MoveToAttribute(ValueAttribute);
                    var value = reader.Value;
                    Add(name, value);
                }

            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach (var name in Keys)
            {
                writer.WriteStartElement(ElementName);
                var value = this[name];
                writer.WriteAttributeString(NameAttribute, name);
                writer.WriteAttributeString(ValueAttribute, value);
                writer.WriteEndElement();
            }
        }

        internal void CopyKeyValuePairs(NameValueCollection destination)
        {
            foreach (var key in Keys)
                destination[key] = this[key];
        }

        internal void CopyKeyValuePairs(StringDictionary destination)
        {
            foreach (var key in Keys)
                destination[key] = this[key];
        }

        public static implicit operator SerializableStringDictionary(NameValueCollection collection)
        {
            var result = new SerializableStringDictionary();
            foreach (string key in collection.Keys)
                result[key] = collection[key];
            return result;
        }

        public static implicit operator SerializableStringDictionary(StringDictionary collection)
        {
            var result = new SerializableStringDictionary();
            foreach (var key in collection.Keys.OfType<string>()) result[key] = collection[key];
            return result;
        }
    }
}