using System;
using System.Net.Mime;

namespace Snork.SerializableMail
{
    /// <summary>Represents a MIME protocol Content-Type header.</summary>
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

        /// <summary>Gets or sets the value of the name parameter included in the Content-Type header represented by this instance.</summary>
        /// <returns>A <see cref="T:System.String" /> that contains the value associated with the name parameter. </returns>

        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the value of the charset parameter included in the Content-Type header represented by this
        ///     instance.
        /// </summary>
        /// <returns>A <see cref="T:System.String" /> that contains the value associated with the charset parameter.</returns>
        public string CharSet { get; set; }

        /// <summary>
        ///     Gets or sets the value of the boundary parameter included in the Content-Type header represented by this
        ///     instance.
        /// </summary>
        /// <returns>A <see cref="T:System.String" /> that contains the value associated with the boundary parameter.</returns>
        public string Boundary { get; set; }

        /// <summary>Gets or sets the media type value included in the Content-Type header represented by this instance.</summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that contains the media type and subtype value. This value does not include
        ///     the semicolon (;) separator that follows the subtype.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///     The value specified for a set operation is
        ///     <see cref="F:System.String.Empty" /> ("").
        /// </exception>
        /// <exception cref="T:System.FormatException">The value specified for a set operation is in a form that cannot be parsed.</exception>
        public string MediaType { get; set; }

        /// <summary>
        ///     Returns a string representation of this <see cref="T:Snork.SerializableMail.SerializableContentType" />
        ///     object.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that contains the current settings for this
        ///     <see cref="T:Snork.SerializableMail.SerializableContentType" />.
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