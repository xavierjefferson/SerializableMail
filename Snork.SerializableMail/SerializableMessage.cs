using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Snork.SerializableMail
{
    [Serializable]
    public class SerializableMessage
    {
        private List<SerializableAttachment> _attachments;

        private string _body;
        private SerializableAddressCollection _cc, _bcc, _replyToList, _to;
        private SerializableStringDictionary _headers;

        private string _subject;

        public SerializableMessage()
        {
            _to = new SerializableAddressCollection();
            _cc = new SerializableAddressCollection();
            _bcc = new SerializableAddressCollection();
            _replyToList = new SerializableAddressCollection();
            _attachments = new List<SerializableAttachment>();
            _headers = new SerializableStringDictionary();
            _subject = string.Empty;
            _body = string.Empty;
        }

        public SerializableMessage(SerializableAddress from, SerializableAddress to) : this()
        {
            To.Add(to);
            From = from;
        }

        public SerializableStringDictionary Headers
        {
            get => _headers;
            set => _headers =
                value ?? throw new ArgumentNullException(nameof(Headers));
        }

        public string Body
        {
            get => _body;
            set => _body = value ?? string.Empty;
        }

        public string Subject
        {
            get => _subject;
            set => _subject = value ?? string.Empty;
        }

        public SerializableAddress From { get; set; }

        public SerializableAddressCollection To
        {
            get => _to;
            set => _to = value ?? throw new ArgumentNullException(nameof(To));
        }

        public SerializableAddressCollection ReplyToList
        {
            get => _replyToList;
            set => _replyToList =
                value ?? throw new ArgumentNullException(nameof(ReplyToList));
        }

        public SerializableAddressCollection CC
        {
            get => _cc;
            set => _cc = value ?? throw new ArgumentNullException(nameof(CC));
        }

        public SerializableAddressCollection Bcc
        {
            get => _bcc;
            set =>
                _bcc = value ?? throw new ArgumentNullException(nameof(Bcc));
        }

        public TransferEncoding BodyTransferEncoding { get; set; }
        public bool IsBodyHtml { get; set; }
        public int? HeadersCodePage { get; set; }
        public int? BodyCodePage { get; set; }
        public SerializableAddress Sender { get; set; }
        public DeliveryNotificationOptions DeliveryNotificationOptions { get; set; }

        public List<SerializableAttachment> Attachments
        {
            get => _attachments;
            set => _attachments = value ?? throw new ArgumentNullException(nameof(Attachments));
        }

        public MailPriority Priority { get; set; }

        public int? SubjectCodePage { get; set; }

        public static implicit operator MailMessage(SerializableMessage mailMessage)
        {
            var result = new MailMessage
            {
                From = mailMessage.From,
                IsBodyHtml = mailMessage.IsBodyHtml,
                Sender = mailMessage.Sender,
                Body = mailMessage.Body,
                Subject = mailMessage.Subject,
                DeliveryNotificationOptions = mailMessage.DeliveryNotificationOptions,
                BodyTransferEncoding = mailMessage.BodyTransferEncoding,
                Priority = mailMessage.Priority
            };
            if (mailMessage.BodyCodePage.HasValue)
                result.BodyEncoding = Encoding.GetEncoding(mailMessage.BodyCodePage.Value);
            if (mailMessage.SubjectCodePage.HasValue)
                result.SubjectEncoding = Encoding.GetEncoding(mailMessage.SubjectCodePage.Value);

            if (mailMessage.HeadersCodePage.HasValue)
                result.HeadersEncoding = Encoding.GetEncoding(mailMessage.HeadersCodePage.Value);
            mailMessage.Headers.CopyKeyValuePairs(result.Headers);

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

        public static implicit operator SerializableMessage(MailMessage mailMessage)
        {
            var result = new SerializableMessage
            {
                Sender = mailMessage.Sender,
                SubjectCodePage = mailMessage.SubjectEncoding?.CodePage,
                To = mailMessage.To,
                CC = mailMessage.CC,
                Bcc = mailMessage.Bcc,
                Subject = mailMessage.Subject,
                ReplyToList = mailMessage.ReplyToList,
                From = mailMessage.From,
                HeadersCodePage = mailMessage.HeadersEncoding?.CodePage,
                Priority = mailMessage.Priority,
                BodyCodePage = mailMessage.BodyEncoding?.CodePage,
                Body = mailMessage.Body,
                IsBodyHtml = mailMessage.IsBodyHtml,
                Headers = mailMessage.Headers,
                DeliveryNotificationOptions = mailMessage.DeliveryNotificationOptions,
                BodyTransferEncoding = mailMessage.BodyTransferEncoding
            };
            var attachments = new List<SerializableAttachment>();
            foreach (var attachment in mailMessage.Attachments) attachments.Add(attachment);
            result.Attachments = attachments;
            return result;
        }
    }
}