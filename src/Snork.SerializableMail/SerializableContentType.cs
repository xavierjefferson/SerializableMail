using System;
using System.Net.Mime;

namespace Snork.SerializableMail
{
    [Serializable]
    public class SerializableContentType
    {
        public SerializableContentType()
        {
            CopyProperties(new ContentType(), this);
        }

        public SerializableContentType(string contentType)
        {
            CopyProperties(new ContentType(contentType), this);
        }

        public string Name { get; set; }
        public string CharSet { get; set; }
        public string Boundary { get; set; }
        public string MediaType { get; set; }

        /// <summary>Returns a string representation of this <see cref="T:SerializableContentType" /> object.</summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that contains the current settings for this
        ///     <see cref="T:SerializableContentType" />.
        /// </returns>
        public override string ToString()
        {
            return ((ContentType) this).ToString();
        }

        public static implicit operator ContentType(SerializableContentType serializableContentType)
        {
            return new ContentType
            {
                Boundary = serializableContentType.Boundary, Name = serializableContentType.Name,
                CharSet = serializableContentType.CharSet, MediaType = serializableContentType.MediaType
            };
        }

        private static SerializableContentType CopyProperties(ContentType contentType,
            SerializableContentType serializableContentType)
        {
            serializableContentType.MediaType = contentType.MediaType;
            serializableContentType.Boundary = contentType.Boundary;
            serializableContentType.CharSet = contentType.CharSet;
            serializableContentType.Name = contentType.Name;
            return serializableContentType;
        }

        public static implicit operator SerializableContentType(ContentType contentType)
        {
            return CopyProperties(contentType, new SerializableContentType());
        }
    }
}