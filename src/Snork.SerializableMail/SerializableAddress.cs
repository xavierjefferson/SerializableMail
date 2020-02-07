using System;
using System.Net.Mail;
using System.Text;

namespace Snork.SerializableMail
{
    /// <summary>Represents the address of an electronic mail sender or recipient.</summary>
    [Serializable]
    public class SerializableAddress
    {
        private static readonly int DefaultDisplayNameCodePage = Encoding.UTF8.CodePage;

        public SerializableAddress()
        {
            DisplayNameCodePage = DefaultDisplayNameCodePage;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Snork.SerializableMail.SerializableMailAddress" /> class using the specified
        ///     address and display name.
        /// </summary>
        /// <param name="address">A <see cref="T:System.String" /> that contains an e-mail address.</param>
        /// <param name="displayName">
        ///     A <see cref="T:System.String" /> that contains the display name associated with
        ///     <paramref name="address" />. This parameter can be <see langword="null" />.
        /// </param>
        /// <param name="displayNameEncoding">
        ///     The <see cref="T:System.Text.Encoding" /> that defines the character set used for
        ///     <paramref name="displayName" />.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="address" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="address" /> is <see cref="F:System.String.Empty" /> ("").
        /// </exception>
        /// <exception cref="T:System.FormatException">
        ///     <paramref name="address" /> is not in a recognized format.-or-
        ///     <paramref name="address" /> contains non-ASCII characters.
        /// </exception>
        public SerializableAddress(string address, string displayName = null, Encoding displayNameEncoding = null) :
            this()
        {
            MailAddress mailAddress;
            if (displayNameEncoding != null)
                mailAddress = new MailAddress(address, displayName, displayNameEncoding);
            else if (displayName != null)
                mailAddress = new MailAddress(address, displayName);
            else
                mailAddress = new MailAddress(address);

            CopyProperties(mailAddress, this);
        }

        /// <summary>Gets the user information from the address specified when this instance was created.</summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that contains the user name portion of the
        ///     <see cref="P:Snork.SerializableMail.SerializableMailAddress.Address" />.
        /// </returns>

        public string User { get; set; }

        /// <summary>Gets the host portion of the address specified when this instance was created.</summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that contains the name of the host computer that accepts e-mail for the
        ///     <see cref="P:Snork.SerializableMail.SerializableMailAddress.User" /> property.
        /// </returns>
        public string Host { get; set; }

        public int DisplayNameCodePage { get; set; }

        /// <summary>Gets the e-mail address specified when this instance was created.</summary>
        /// <returns>A <see cref="T:System.String" /> that contains the e-mail address.</returns>
        public string Address { get; set; }

        /// <summary>
        ///     Gets the display name composed from the display name and address information specified when this instance was
        ///     created.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that contains the display name; otherwise,
        ///     <see cref="F:System.String.Empty" /> ("") if no display name information was specified when this instance was
        ///     created.
        /// </returns>
        public string DisplayName { get; set; }

        private static SerializableAddress CopyProperties(MailAddress source, SerializableAddress destination)
        {
            var codePage = CodePageHelper.GetCodePage(source);
            var mailAddress = source;
            destination.Address = mailAddress.Address;
            destination.DisplayName = mailAddress.DisplayName;
            destination.Host = mailAddress.Host;
            destination.User = mailAddress.User;
            destination.DisplayNameCodePage =
                codePage ?? DefaultDisplayNameCodePage;
            return destination;
        }

        /// <summary>Returns a string representation of this instance.</summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that contains the contents of this
        ///     <see cref="T:Snork.SerializableMail.SerializableMailAddress" />.
        /// </returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(DisplayName))
                return Address;
            return string.Format("\"{0}\" {1}", DisplayName, Address);
        }

        public static implicit operator MailAddress(SerializableAddress address)
        {
            if (address == null) return null;
            return new MailAddress(address.Address, address.DisplayName,
                Encoding.GetEncoding(address.DisplayNameCodePage));
        }

        public static implicit operator SerializableAddress(string address)
        {
            if (address == null) return null;
            return new SerializableAddress(address);
        }

        /// <summary>Compares two mail addresses.</summary>
        /// <param name="value">A <see cref="T:Snork.SerializableMail.SerializableMailAddress" /> instance to compare to the current instance.</param>
        /// <returns>
        ///     <see langword="true" /> if the two mail addresses are equal; otherwise, <see langword="false" />.
        /// </returns>
        public override bool Equals(object value)
        {
            if (value == null)
                return false;
            return ToString().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>Returns a hash value for a mail address.</summary>
        /// <returns>An integer hash value.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static implicit operator SerializableAddress(MailAddress address)
        {
            if (address == null) return null;

            return CopyProperties(address, new SerializableAddress());
        }
    }
}