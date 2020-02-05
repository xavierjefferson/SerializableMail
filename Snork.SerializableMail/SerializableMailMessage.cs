using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Snork.SerializableMail
{
    public class SerializableMailMessage
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string Body { get; set; }
        public string Subject { get; set; }
        public SerializableMailAddress From { get; set; }
        public SerializableMailAddressList To { get; set; } = new SerializableMailAddressList();
        public SerializableMailAddressList ReplyToList { get; set; } = new SerializableMailAddressList();
        public SerializableMailAddressList CC { get; set; } = new SerializableMailAddressList();
        public SerializableMailAddressList Bcc { get; set; } = new SerializableMailAddressList();
        public TransferEncoding BodyTransferEncoding { get; set; }
        public bool IsBodyHtml { get; set; }
        public int HeadersCodePage { get; set; }
        public int BodyCodePage { get; set; }
        public SerializableMailAddress Sender { get; set; }
        public DeliveryNotificationOptions DeliveryNotificationOptions { get; set; }
        public List<SerializableMailAttachment> Attachments { get; set; } = new List<SerializableMailAttachment>();
        public MailPriority Priority { get; set; }
        public int SubjectCodePage { get; set; }

        public static implicit operator MailMessage(SerializableMailMessage mailMessage)
        {
            var result = new MailMessage
            {
                From = mailMessage.From,
                SubjectEncoding = Encoding.GetEncoding(mailMessage.SubjectCodePage),
                IsBodyHtml = mailMessage.IsBodyHtml,
                Sender = mailMessage.Sender,
                Body = mailMessage.Body,
                Subject = mailMessage.Subject,
                DeliveryNotificationOptions = mailMessage.DeliveryNotificationOptions,
                HeadersEncoding = Encoding.GetEncoding(mailMessage.HeadersCodePage),
                BodyEncoding = Encoding.GetEncoding(mailMessage.BodyCodePage),
                BodyTransferEncoding = mailMessage.BodyTransferEncoding,
                Priority = mailMessage.Priority
            };

            foreach (var keyValuePair in mailMessage.Headers)
                result.Headers[keyValuePair.Key] = keyValuePair.Value;

            foreach (var i in mailMessage.Attachments)
                result.Attachments.Add(i);

            foreach (var i in mailMessage.To)
                result.To.Add(i);

            foreach (var i in mailMessage.CC)
                result.CC.Add(i);

            foreach (var i in mailMessage.Bcc)
                result.Bcc.Add(i);

            foreach (var i in mailMessage.ReplyToList)
                result.ReplyToList.Add(i);

            return result;
        }

        public static implicit operator SerializableMailMessage(MailMessage mailMessage)
        {
            var headers = new Dictionary<string, string>();
            foreach (string key in mailMessage.Headers.Keys)
                headers[key] = mailMessage.Headers[key];
            return new SerializableMailMessage
            {
                Sender = mailMessage.Sender,
                SubjectCodePage = mailMessage.SubjectEncoding.CodePage,
                To = mailMessage.To,
                CC = mailMessage.CC,
                Bcc = mailMessage.Bcc,
                Subject = mailMessage.Subject,
                ReplyToList = mailMessage.ReplyToList,
                From = mailMessage.From,
                HeadersCodePage = mailMessage.HeadersEncoding.CodePage,
                Priority = mailMessage.Priority,
                BodyCodePage = mailMessage.BodyEncoding.CodePage,
                Body = mailMessage.Body,
                IsBodyHtml = mailMessage.IsBodyHtml,
                Headers = headers,
                DeliveryNotificationOptions = mailMessage.DeliveryNotificationOptions,
                Attachments = mailMessage.Attachments.Cast<SerializableMailAttachment>().ToList(),
                BodyTransferEncoding = mailMessage.BodyTransferEncoding
            };
        }
    }
}