using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Snork.SerializableMail
{
    public class SerializableMailAttachment
    {
        public SerializableMailAttachment(string fileName)
        {
            CloneAttachment(new Attachment(fileName), this);
        }

        public SerializableMailAttachment(Stream contentStream, string name, string mediaType)
        {
            CloneAttachment(new Attachment(contentStream, name, mediaType), this);
        }

        public SerializableMailAttachment(string fileName, string mediaType)
        {
            CloneAttachment(new Attachment(fileName, mediaType), this);
        }

        public SerializableMailAttachment(Stream contentStream, ContentType contentType)
        {
            CloneAttachment(new Attachment(contentStream, contentType), this);
        }

        public SerializableMailAttachment()
        {
        }

        public string Name { get; set; }
        public ContentDisposition ContentDisposition { get; set; }
        public string ContentId { get; set; }
        public ContentType ContentType { get; set; }

        public byte[] ContentBytes { get; set; }

        public string NameEncodingAsString { get; set; }

        public TransferEncoding TransferEncoding { get; set; }

        public static implicit operator Attachment(SerializableMailAttachment attachment)
        {
            if (attachment == null)
            {
                return null;
            }

            var memoryStream = new MemoryStream(attachment.ContentBytes);

            return new Attachment(memoryStream, attachment.ContentType)
            {
                Name = attachment.Name,
                TransferEncoding = attachment.TransferEncoding,
                NameEncoding = Encoding.GetEncoding(attachment.NameEncodingAsString),
                ContentId = attachment.ContentId,
                ContentType = attachment.ContentType
            };
        }

        private static void CloneAttachment(Attachment source, SerializableMailAttachment destination)
        {
            destination.Name = source.Name;
            destination.TransferEncoding = source.TransferEncoding;
            destination.ContentDisposition = source.ContentDisposition;
            destination.ContentId = source.ContentId;
            destination.NameEncodingAsString = source.NameEncoding.EncodingName;


            using (var mx = new MemoryStream())
            {
                source.ContentStream.CopyTo(mx);
                destination.ContentBytes = mx.GetBuffer();
            }
        }

        public static implicit operator SerializableMailAttachment(Attachment attachment)
        {
            if (attachment == null)
            {
                return null;
            }

            var result = new SerializableMailAttachment();
            CloneAttachment(attachment, result);
            return result;
        }
    }
}