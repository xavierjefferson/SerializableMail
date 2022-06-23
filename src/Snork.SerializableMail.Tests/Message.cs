using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Xunit;

namespace Snork.SerializableMail.Tests
{
    public class Message
    {
        private const string address = "ok@ok.com";
        private const string Toaddress = "ok1@ok1.com";
        private const string displayName = "Yes";
        private static readonly Encoding displayNameEncoding = Encoding.ASCII;

        [Fact]
        public void MailAddressToSerializableAddress()
        {
            var mailMessage = new MailMessage(new MailAddress(address, displayName), new MailAddress(Toaddress))
            {
                BodyEncoding = Encoding.ASCII,
                HeadersEncoding = Encoding.BigEndianUnicode,
                SubjectEncoding = Encoding.Unicode
            };
            var memoryStream = new MemoryStream();
            using (var sw = new StreamWriter(memoryStream, Encoding.Default, 1024, true))
            {
                sw.Write(Guid.NewGuid().ToString());
                sw.Flush();
            }

            memoryStream.Position = 0;
            mailMessage.Attachments.Add(new Attachment(memoryStream, new ContentType("text/plain")));

            SerializableMessage serializableMessage = mailMessage;
            Compare(mailMessage, serializableMessage);
        }

        [Fact]
        public void SerializableAddressToMailAddress()
        {
            var serializableMessage = new SerializableMessage(new SerializableAddress(address, displayName),
                new SerializableAddress(Toaddress));
            serializableMessage.BodyCodePage = displayNameEncoding.CodePage;
            MailMessage mailMessage = serializableMessage;
            Compare(mailMessage, serializableMessage);
        }
 
        private static void Compare(MailMessage mailMessage, SerializableMessage serializableMessage)
        {
            Assert.Equal(mailMessage.To.Count, serializableMessage.To.Count);
            Assert.Equal(mailMessage.CC.Count, serializableMessage.CC.Count);
            Assert.Equal(mailMessage.Bcc.Count, serializableMessage.Bcc.Count);
            Assert.Equal(mailMessage.ReplyToList.Count, serializableMessage.ReplyToList.Count);
            Assert.Equal(mailMessage.Attachments.Count, serializableMessage.Attachments.Count);
            Assert.Equal(mailMessage.Subject, serializableMessage.Subject);
            Assert.Equal(mailMessage.Body, serializableMessage.Body);
            Assert.Equal(mailMessage.IsBodyHtml, serializableMessage.IsBodyHtml);
            Assert.Equal(mailMessage.BodyEncoding,
                serializableMessage.BodyCodePage == null
                    ? null
                    : Encoding.GetEncoding(serializableMessage.BodyCodePage.Value));
            Assert.Equal(mailMessage.HeadersEncoding,
                serializableMessage.HeadersCodePage == null
                    ? null
                    : Encoding.GetEncoding(serializableMessage.HeadersCodePage.Value));
            Assert.Equal(mailMessage.SubjectEncoding,
                serializableMessage.SubjectCodePage == null
                    ? null
                    : Encoding.GetEncoding(serializableMessage.SubjectCodePage.Value));
            foreach (var tuple in mailMessage.Attachments.Select((item, index) => new { Item = item, Index = index }))
            {
                var serializableAttachment = serializableMessage.Attachments[tuple.Index];
                if (tuple.Item.ContentStream.CanSeek)
                    Assert.Equal(tuple.Item.ContentStream.Length, serializableAttachment.ContentBytes.Length);

                Assert.Equal(tuple.Item.Name, serializableAttachment.Name);
                Assert.Equal(tuple.Item.ContentType.ToString(), serializableAttachment.ContentType.ToString());
                var contentDisposition = tuple.Item.ContentDisposition;
                var serializableAttachmentContentDisposition = serializableAttachment.ContentDisposition;

                Compare(contentDisposition, serializableAttachmentContentDisposition);
            }
        }

        private static void Compare(ContentDisposition contentDisposition,
            SerializableContentDisposition serializableAttachmentContentDisposition)
        {
            Assert.Equal(contentDisposition.Parameters.Count, serializableAttachmentContentDisposition.Parameters.Count);
            Assert.Equal(contentDisposition.CreationDate, serializableAttachmentContentDisposition.CreationDate);
            Assert.Equal(contentDisposition.ModificationDate,
                serializableAttachmentContentDisposition.ModificationDate);
            Assert.Equal(contentDisposition.Size, serializableAttachmentContentDisposition.Size);
            Assert.Equal(contentDisposition.FileName, serializableAttachmentContentDisposition.FileName);
            Assert.Equal(contentDisposition.DispositionType,
                serializableAttachmentContentDisposition.DispositionType);
            Assert.Equal(contentDisposition.Inline, serializableAttachmentContentDisposition.Inline);
        }


        //[Fact]
        //public void SerializableAddressToMailAddress()
        //{
        //    var serializableAddress = new SerializableAddress(address, displayName, displayNameEncoding);
        //    //implicit conversion
        //    MailAddress mailAddress = serializableAddress;
        //    var codePage = CodePageHelper.GetCodePage(mailAddress);
        //    Assert.IsTrue(codePage.HasValue);
        //    Assert.Equal(codePage.Value, displayNameEncoding.CodePage);
        //    Assert.Equal(mailAddress.Address, serializableAddress.Address);
        //    Assert.Equal(mailAddress.DisplayName, serializableAddress.DisplayName);
        //}

        //[Fact]
        //public void SerializableAddressCompare()
        //{
        //    var a1 = new SerializableAddress(address);
        //    var a2 = new SerializableAddress(address);
        //    Assert.Equal(a1, a2);
        //}
    }
}