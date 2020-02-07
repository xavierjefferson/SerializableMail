using System;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Snork.SerializableMail
{
    /// <summary>Represents an e-mail message that can be sent using the <see cref="T:System.Net.Mail.SmtpClient" /> class.</summary>
    [Serializable]
    public class SerializableMessage
    {
        private SerializableAttachmentCollection _attachments;

        private string _body;
        private SerializableAddressCollection _cc, _bcc, _replyToList, _to;
        private SerializableStringDictionary _headers;

        private string _subject;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Snork.SerializableMail.SerializableMessage" /> class by using the
        ///     specified
        ///     <see cref="T:System.String" /> class objects.
        /// </summary>
        /// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message.</param>
        /// <param name="to">A <see cref="T:System.String" /> that contains the addresses of the recipients of the e-mail message.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="from" /> is <see langword="null" />.-or-
        ///     <paramref name="to" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="from" /> is <see cref="F:System.String.Empty" /> ("").-or-
        ///     <paramref name="to" /> is <see cref="F:System.String.Empty" /> ("").
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///     <paramref name="from" /> or <paramref name="to" /> is malformed.
        /// </exception>
        public SerializableMessage(string from, string to) : this()
        {
            if (to == null) throw new ArgumentNullException(nameof(to));
            From = from ?? throw new ArgumentNullException(nameof(from));
            To.Add(to);
        }

        /// <summary>Initializes a new instance of the <see cref="T:Snork.SerializableMail.SerializableMessage" /> class. </summary>
        /// <param name="from">A <see cref="T:System.String" /> that contains the address of the sender of the e-mail message.</param>
        /// <param name="to">A <see cref="T:System.String" /> that contains the address of the recipient of the e-mail message.</param>
        /// <param name="subject">A <see cref="T:System.String" /> that contains the subject text.</param>
        /// <param name="body">A <see cref="T:System.String" /> that contains the message body.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="from" /> is <see langword="null" />.-or-
        ///     <paramref name="to" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="from" /> is <see cref="F:System.String.Empty" /> ("").-or-
        ///     <paramref name="to" /> is <see cref="F:System.String.Empty" /> ("").
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///     <paramref name="from" /> or <paramref name="to" /> is malformed.
        /// </exception>
        public SerializableMessage(string from, string to, string subject, string body)
            : this(from, to)
        {
            Subject = subject;
            Body = body;
        }

        public SerializableMessage()
        {
            _to = new SerializableAddressCollection();
            _cc = new SerializableAddressCollection();
            _bcc = new SerializableAddressCollection();
            _replyToList = new SerializableAddressCollection();
            _attachments = new SerializableAttachmentCollection();
            _headers = new SerializableStringDictionary();
            _subject = string.Empty;
            _body = string.Empty;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Snork.SerializableMail.SerializableMessage" /> class by using the
        ///     specified
        ///     <see cref="T:Snork.SerializableMail.SerializableAddress" /> class objects.
        /// </summary>
        /// <param name="from">
        ///     A <see cref="T:Snork.SerializableMail.SerializableAddress" /> that contains the address of the sender of the e-mail
        ///     message.
        /// </param>
        /// <param name="to">
        ///     A <see cref="T:Snork.SerializableMail.SerializableAddress" /> that contains the address of the recipient of the
        ///     e-mail message.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="from" /> is <see langword="null" />.-or-
        ///     <paramref name="to" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///     <paramref name="from" /> or <paramref name="to" /> is malformed.
        /// </exception>
        public SerializableMessage(SerializableAddress from, SerializableAddress to) : this()
        {
            To.Add(to);
            From = from;
        }

        /// <summary>Gets the e-mail headers that are transmitted with this e-mail message.</summary>
        /// <returns>A <see cref="T:Snork.SerializableMail.SerializableStringDictionary" /> that contains the e-mail headers.</returns>
        public SerializableStringDictionary Headers
        {
            get => _headers;
            set => _headers =
                value ?? throw new ArgumentNullException(nameof(Headers));
        }

        /// <summary>Gets or sets the message body.</summary>
        /// <returns>A <see cref="T:System.String" /> value that contains the body text.</returns>
        public string Body
        {
            get => _body;
            set => _body = value ?? string.Empty;
        }

        /// <summary>Gets or sets the subject line for this e-mail message.</summary>
        /// <returns>A <see cref="T:System.String" /> that contains the subject content.</returns>
        public string Subject
        {
            get => _subject;
            set => _subject = value ?? string.Empty;
        }

        /// <summary>Gets or sets the sender's address for this e-mail message.</summary>
        /// <returns>A <see cref="T:Snork.SerializableMail.SerializableAddress" /> that contains the sender's address information.</returns>
        public SerializableAddress From { get; set; }

        /// <summary>Gets the address collection that contains the recipients of this e-mail message.</summary>
        /// <returns>A writable <see cref="T:Snork.SerializableMail.SerializableAddressCollection" /> object.</returns>
        public SerializableAddressCollection To
        {
            get => _to;
            set => _to = value ?? throw new ArgumentNullException(nameof(To));
        }

        /// <summary>Gets or sets the list of addresses to reply to for the mail message.</summary>
        /// <returns>The list of the addresses to reply to for the mail message.</returns>
        public SerializableAddressCollection ReplyToList
        {
            get => _replyToList;
            set => _replyToList =
                value ?? throw new ArgumentNullException(nameof(ReplyToList));
        }

        /// <summary>Gets the address collection that contains the carbon copy (CC) recipients for this e-mail message.</summary>
        /// <returns>A writable <see cref="T:Snork.SerializableMail.SerializableAddressCollection" /> object.</returns>
        public SerializableAddressCollection CC
        {
            get => _cc;
            set => _cc = value ?? throw new ArgumentNullException(nameof(CC));
        }

        /// <summary>Gets the address collection that contains the blind carbon copy (BCC) recipients for this e-mail message.</summary>
        /// <returns>A writable <see cref="T:Snork.SerializableMail.SerializableAddressCollection" /> object.</returns>
        public SerializableAddressCollection Bcc
        {
            get => _bcc;
            set =>
                _bcc = value ?? throw new ArgumentNullException(nameof(Bcc));
        }

        /// <summary>Gets or sets the transfer encoding used to encode the message body.</summary>
        /// <returns>
        ///     Returns <see cref="T:System.Net.Mime.TransferEncoding" />.A <see cref="T:System.Net.Mime.TransferEncoding" />
        ///     applied to the contents of the <see cref="P:Snork.SerializableMail.SerializableMessage.Body" />.
        /// </returns>
        public TransferEncoding BodyTransferEncoding { get; set; }

        /// <summary>Gets or sets a value indicating whether the mail message body is in Html.</summary>
        /// <returns>
        ///     <see langword="true" /> if the message body is in Html; else <see langword="false" />. The default is
        ///     <see langword="false" />.
        /// </returns>
        public bool IsBodyHtml { get; set; }

        /// <summary>Gets or sets the encoding used for the user-defined custom headers for this e-mail message.</summary>
        /// <returns>The encoding used for user-defined custom headers for this e-mail message.</returns>
        public int? HeadersCodePage { get; set; }

        /// <summary>Gets or sets the encoding used to encode the message body.</summary>
        /// <returns>
        ///     A code page value applied to the contents of the
        ///     <see cref="P:Snork.SerializableMail.SerializableMessage.Body" />.
        /// </returns>
        public int? BodyCodePage { get; set; }

        /// <summary>Gets or sets the sender's address for this e-mail message.</summary>
        /// <returns>A <see cref="T:Snork.SerializableMail.SerializableAddress" /> that contains the sender's address information.</returns>
        public SerializableAddress Sender { get; set; }

        /// <summary>Gets or sets the delivery notifications for this e-mail message.</summary>
        /// <returns>
        ///     A <see cref="T:System.Net.Mail.DeliveryNotificationOptions" /> value that contains the delivery notifications
        ///     for this message.
        /// </returns>
        public DeliveryNotificationOptions DeliveryNotificationOptions { get; set; }

        /// <summary>Gets the attachment collection used to store data attached to this e-mail message.</summary>
        /// <returns>A writable <see cref="T:Snork.SerializableMail.SerializableAttachmentCollection" />.</returns>
        public SerializableAttachmentCollection Attachments
        {
            get => _attachments;
            set => _attachments = value ?? throw new ArgumentNullException(nameof(Attachments));
        }

        /// <summary>Gets or sets the priority of this e-mail message.</summary>
        /// <returns>A <see cref="T:System.Net.Mail.MailPriority" /> that contains the priority of this message.</returns>
        public MailPriority Priority { get; set; }

        /// <summary>Gets or sets the encoding used for the subject content for this e-mail message.</summary>
        /// <returns>
        ///     A code page value that was used to encode the
        ///     <see cref="P:Snork.SerializableMail.SerializableMessage.Subject" /> property.
        /// </returns>
        public int? SubjectCodePage { get; set; }

        /// <summary>
        ///     Convert an instance of <see cref="T:SerializableMessage" /> to <see cref="T:Snork.SerializableMail.SerializableMessage" />
        /// </summary>
        /// <param name="mailMessage"></param>
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

        /// <summary>
        ///     Convert an instance of <see cref="T:System.Net.Mail.MailMessage" /> to
        ///     <see cref="T:Snork.SerializableMail.SerializableMessage" />
        /// </summary>
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
                BodyTransferEncoding = mailMessage.BodyTransferEncoding,
                Attachments =
                    new SerializableAttachmentCollection(
                        mailMessage.Attachments.Select(i => (SerializableAttachment) i))
            };

            return result;
        }
    }
}