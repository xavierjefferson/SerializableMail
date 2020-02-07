using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Snork.SerializableMail
{
    /// <summary>Represents an attachment to an e-mail.</summary>
    [Serializable]
    public class SerializableAttachment : IDisposable
    {
        internal bool disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Snork.SerializableMail.SerializableAttachment" /> class with the
        ///     specified content
        ///     string.
        /// </summary>
        /// <param name="fileName">A <see cref="T:System.String" /> that contains a file path to use to create this attachment.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="fileName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="fileName" /> is empty.
        /// </exception>
        public SerializableAttachment(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (fileName == string.Empty) throw new ArgumentException(nameof(fileName));
            CloneAttachment(new Attachment(fileName), this);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Snork.SerializableMail.SerializableAttachment" /> class with the
        ///     specified stream,
        ///     name, and MIME type information.
        /// </summary>
        /// <param name="contentStream">A readable <see cref="T:System.IO.Stream" /> that contains the content for this attachment.</param>
        /// <param name="name">
        ///     A <see cref="T:System.String" /> that contains the value for the
        ///     <see cref="P:System.Net.Mime.ContentType.Name" /> property of the <see cref="T:System.Net.Mime.ContentType" />
        ///     associated with this attachment. This value can be <see langword="null" />.
        /// </param>
        /// <param name="mediaType">
        ///     A <see cref="T:System.String" /> that contains the MIME Content-Header information for this
        ///     attachment. This value can be <see langword="null" />.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="stream" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///     <paramref name="mediaType" /> is not in the correct format.
        /// </exception>
        public SerializableAttachment(Stream contentStream, string name, string mediaType)
        {
            if (contentStream == null) throw new ArgumentNullException(nameof(contentStream));
            CloneAttachment(new Attachment(contentStream, name, mediaType), this);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Snork.SerializableMail.SerializableAttachment" /> class with the
        ///     specified stream
        ///     and name.
        /// </summary>
        /// <param name="contentStream">A readable <see cref="T:System.IO.Stream" /> that contains the content for this attachment.</param>
        /// <param name="name">
        ///     A <see cref="T:System.String" /> that contains the value for the
        ///     <see cref="P:System.Net.Mime.ContentType.Name" /> property of the <see cref="T:System.Net.Mime.ContentType" />
        ///     associated with this attachment. This value can be <see langword="null" />.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="contentStream" /> is <see langword="null" />.
        /// </exception>
        public SerializableAttachment(Stream contentStream, string name)

        {
            Name = name;
            CloneAttachment(new Attachment(contentStream, name), this);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Snork.SerializableMail.SerializableAttachment" /> class with the
        ///     specified content
        ///     string and MIME type information.
        /// </summary>
        /// <param name="fileName">A <see cref="T:System.String" /> that contains the content for this attachment.</param>
        /// <param name="mediaType">
        ///     A <see cref="T:System.String" /> that contains the MIME Content-Header information for this
        ///     attachment. This value can be <see langword="null" />.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="fileName" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///     <paramref name="mediaType" /> is not in the correct format.
        /// </exception>
        public SerializableAttachment(string fileName, string mediaType)
        {
            CloneAttachment(new Attachment(fileName, mediaType), this);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Snork.SerializableMail.SerializableAttachment" /> class with the
        ///     specified stream
        ///     and content type.
        /// </summary>
        /// <param name="contentStream">A readable <see cref="T:System.IO.Stream" /> that contains the content for this attachment.</param>
        /// <param name="contentType">
        ///     A <see cref="T:System.Net.Mime.ContentType" /> that describes the data in
        ///     <paramref name="stream" />.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="contentType" /> is <see langword="null" />.-or-
        ///     <paramref name="contentStream" /> is <see langword="null" />.
        /// </exception>
        public SerializableAttachment(Stream contentStream, ContentType contentType)
        {
            CloneAttachment(new Attachment(contentStream, contentType), this);
        }

        public SerializableAttachment()
        {
        }

        /// <summary>Gets or sets the MIME content type name value in the content type associated with this attachment.</summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that contains the value for the content type <paramref name="name" />
        ///     represented by the <see cref="P:System.Net.Mime.ContentType.Name" /> property.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///     The value specified for a set operation is
        ///     <see cref="F:System.String.Empty" /> ("").
        /// </exception>
        public string Name { get; set; }

        /// <summary>
        ///     Specifies the encoding code page for the <see cref="T:Snork.SerializableMail.SerializableAttachment" />
        ///     <see cref="P:Snork.SerializableMail.SerializableAttachment.Name" />.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:int" /> value that specifies the code page for the name encoding. The default value is
        ///     determined from the name of the attachment.
        /// </returns>
        public int? NameEncodingCodePage { get; set; }

        /// <summary>Gets the MIME content disposition for this attachment.</summary>
        /// <returns>
        ///     A <see cref="T:Snork.SerializableMail.SerializableContentDisposition" /> that provides the presentation information
        ///     for this
        ///     attachment.
        /// </returns>
        public SerializableContentDisposition ContentDisposition { get; set; }

        /// <summary>Gets or sets the MIME content ID for this attachment.</summary>
        /// <returns>A <see cref="T:System.String" /> holding the content ID.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     Attempted to set <see cref="P:Snork.SerializableMail.SerializableAttachment.ContentId" /> to
        ///     <see langword="null" />.
        /// </exception>
        public string ContentId { get; set; }

        /// <summary>Gets the content type of this attachment.</summary>
        /// <returns>A <see cref="T:Snork.SerializableMail.SerializableContentType" />.The content type for this attachment.</returns>
        public SerializableContentType ContentType { get; set; }

        public byte[] ContentBytes { get; set; }

        /// <summary>Gets or sets the encoding of this attachment.</summary>
        /// <returns>A <see cref="T:System.Net.Mime.TransferEncoding" />.The encoding for this attachment. </returns>
        public TransferEncoding TransferEncoding { get; set; }

        /// <summary>Releases the resources used by the <see cref="T:Snork.SerializableMail.SerializableAttachment" />. </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the <see cref="T:Snork.SerializableMail.SerializableAttachmen" /> and optionally
        ///     releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || disposed)
                return;
            disposed = true;
        }

        /// <summary>
        ///     Creates a mail attachment using the content from the specified string, and the specified MIME content type
        ///     name.
        /// </summary>
        /// <param name="content">A <see cref="T:System.String" /> that contains the content for this attachment.</param>
        /// <param name="name">The MIME content type name value in the content type associated with this attachment.</param>
        /// <returns>An object of type <see cref="T:Snork.SerializableMail.SerializableAttachment" />.</returns>
        public static SerializableAttachment CreateAttachmentFromString(string content, string name)
        {
            return Attachment.CreateAttachmentFromString(content, name);
        }

        /// <summary>
        ///     Creates a mail attachment using the content from the specified string, and the specified
        ///     <see cref="T:System.Net.Mime.ContentType" />.
        /// </summary>
        /// <param name="content">A <see cref="T:System.String" /> that contains the content for this attachment.</param>
        /// <param name="contentType">
        ///     A <see cref="T:System.Net.Mime.ContentType" /> object that represents the Multipurpose
        ///     Internet Mail Exchange (MIME) protocol Content-Type header to be used.
        /// </param>
        /// <returns>An object of type <see cref="T:Snork.SerializableMail.SerializableAttachment" />.</returns>
        public static SerializableAttachment CreateAttachmentFromString(
            string content,
            ContentType contentType)
        {
            return Attachment.CreateAttachmentFromString(content, contentType);
        }

        /// <summary>
        ///     Creates a mail attachment using the content from the specified string, the specified MIME content type name,
        ///     character encoding, and MIME header information for the attachment.
        /// </summary>
        /// <param name="content">A <see cref="T:System.String" /> that contains the content for this attachment.</param>
        /// <param name="name">The MIME content type name value in the content type associated with this attachment.</param>
        /// <param name="contentEncoding">An <see cref="T:System.Text.Encoding" />. This value can be <see langword="null" />.</param>
        /// <param name="mediaType">
        ///     A <see cref="T:System.String" /> that contains the MIME Content-Header information for this
        ///     attachment. This value can be <see langword="null" />.
        /// </param>
        /// <returns>An object of type <see cref="T:Snork.SerializableMail.SerializableAttachment" />.</returns>
        public static SerializableAttachment CreateAttachmentFromString(
            string content,
            string name,
            Encoding contentEncoding,
            string mediaType)
        {
            return Attachment.CreateAttachmentFromString(content, name, contentEncoding, mediaType);
        }


        public static implicit operator Attachment(SerializableAttachment attachment)
        {
            if (attachment == null) return null;

            var memoryStream = new MemoryStream(attachment.ContentBytes);

            var result = new Attachment(memoryStream, attachment.ContentType)
            {
                Name = attachment.Name,
                TransferEncoding = attachment.TransferEncoding,
                NameEncoding = attachment.NameEncodingCodePage.HasValue
                    ? Encoding.GetEncoding(attachment.NameEncodingCodePage.Value)
                    : null,
                ContentId = attachment.ContentId,
                ContentType = attachment.ContentType
            };
            SerializableContentDisposition.CopyProperties(attachment.ContentDisposition, result.ContentDisposition);
            return result;
        }

        private static void CloneAttachment(Attachment source, SerializableAttachment destination)
        {
            destination.Name = source.Name;
            destination.TransferEncoding = source.TransferEncoding;
            destination.ContentDisposition = source.ContentDisposition;
            destination.ContentId = source.ContentId;
            destination.ContentType = source.ContentType;
            destination.NameEncodingCodePage = source.NameEncoding?.CodePage;

            using (var memoryStream = new MemoryStream())
            {
                source.ContentStream.CopyTo(memoryStream);
                destination.ContentBytes = memoryStream.ToArray();
            }
        }

        public static implicit operator SerializableAttachment(Attachment attachment)
        {
            if (attachment == null) return null;

            var result = new SerializableAttachment();
            CloneAttachment(attachment, result);
            return result;
        }
    }
}