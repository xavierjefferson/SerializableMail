using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Snork.SerializableMail.Tests
{
    [TestClass]
    public class Message
    {
        private const string address = "ok@ok.com";
        private const string Toaddress = "ok1@ok1.com";
        private const string displayName = "Yes";
        private static readonly Encoding displayNameEncoding = Encoding.ASCII;

        [TestMethod]
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

        [TestMethod]
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
            Assert.AreEqual(mailMessage.To.Count, serializableMessage.To.Count, "To-count failed");
            Assert.AreEqual(mailMessage.CC.Count, serializableMessage.CC.Count, "Cc-count failed");
            Assert.AreEqual(mailMessage.Bcc.Count, serializableMessage.Bcc.Count, "Bcc-count failed");
            Assert.AreEqual(mailMessage.ReplyToList.Count, serializableMessage.ReplyToList.Count,
                "Replytolist-count failed");
            Assert.AreEqual(mailMessage.Attachments.Count, serializableMessage.Attachments.Count,
                "Attachment-count failed");
            Assert.AreEqual(mailMessage.Subject, serializableMessage.Subject, "Subject failed");
            Assert.AreEqual(mailMessage.Body, serializableMessage.Body, "Body failed");
            Assert.AreEqual(mailMessage.IsBodyHtml, serializableMessage.IsBodyHtml, "IsBodyHtml failed");
            Assert.AreEqual(mailMessage.BodyEncoding,
                serializableMessage.BodyCodePage == null
                    ? null
                    : Encoding.GetEncoding(serializableMessage.BodyCodePage.Value), "BodyCodePage failed");
            Assert.AreEqual(mailMessage.HeadersEncoding,
                serializableMessage.HeadersCodePage == null
                    ? null
                    : Encoding.GetEncoding(serializableMessage.HeadersCodePage.Value), "HeadersCodePage failed");
            Assert.AreEqual(mailMessage.SubjectEncoding,
                serializableMessage.SubjectCodePage == null
                    ? null
                    : Encoding.GetEncoding(serializableMessage.SubjectCodePage.Value), "SubjectCodePage failed");
            foreach (var tuple in mailMessage.Attachments.Select((item, index) => new {Item = item, Index = index}))
            {
                var serializableAttachment = serializableMessage.Attachments[tuple.Index];
                if (tuple.Item.ContentStream.CanSeek)
                    Assert.AreEqual(tuple.Item.ContentStream.Length, serializableAttachment.ContentBytes.Length,
                        string.Format("Attachment length mismatch for item {0}", tuple.Index));

                Assert.AreEqual(tuple.Item.Name, serializableAttachment.Name);
                Assert.AreEqual(tuple.Item.ContentType.ToString(), serializableAttachment.ContentType.ToString(),
                    "Content Type failed");
                var contentDisposition = tuple.Item.ContentDisposition;
                var serializableAttachmentContentDisposition = serializableAttachment.ContentDisposition;

                Compare(contentDisposition, serializableAttachmentContentDisposition);
            }
        }

        private static void Compare(ContentDisposition contentDisposition,
            SerializableContentDisposition serializableAttachmentContentDisposition)
        {
            Assert.AreEqual(contentDisposition.Parameters.Count, serializableAttachmentContentDisposition.Parameters.Count,
                "Content Disposition param count failed");
            Assert.AreEqual(contentDisposition.CreationDate, serializableAttachmentContentDisposition.CreationDate,
                "Content Disposition creation date failed");
            Assert.AreEqual(contentDisposition.ModificationDate,
                serializableAttachmentContentDisposition.ModificationDate,
                "Content Disposition modification date failed");
            Assert.AreEqual(contentDisposition.Size, serializableAttachmentContentDisposition.Size,
                "Content Disposition size failed");
            Assert.AreEqual(contentDisposition.FileName, serializableAttachmentContentDisposition.FileName,
                "Content Disposition filename failed");
            Assert.AreEqual(contentDisposition.DispositionType,
                serializableAttachmentContentDisposition.DispositionType,
                "Content Disposition disp typefailed");
            Assert.AreEqual(contentDisposition.Inline, serializableAttachmentContentDisposition.Inline,
                "Content Disposition inline failed");
        }


        //[TestMethod]
        //public void SerializableAddressToMailAddress()
        //{
        //    var serializableAddress = new SerializableAddress(address, displayName, displayNameEncoding);
        //    //implicit conversion
        //    MailAddress mailAddress = serializableAddress;
        //    var codePage = CodePageHelper.GetCodePage(mailAddress);
        //    Assert.IsTrue(codePage.HasValue);
        //    Assert.AreEqual(codePage.Value, displayNameEncoding.CodePage);
        //    Assert.AreEqual(mailAddress.Address, serializableAddress.Address);
        //    Assert.AreEqual(mailAddress.DisplayName, serializableAddress.DisplayName);
        //}

        //[TestMethod]
        //public void SerializableAddressCompare()
        //{
        //    var a1 = new SerializableAddress(address);
        //    var a2 = new SerializableAddress(address);
        //    Assert.AreEqual(a1, a2);
        //}
    }
}