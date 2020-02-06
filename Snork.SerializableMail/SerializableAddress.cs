using System;
using System.Net.Mail;
using System.Text;

namespace Snork.SerializableMail
{
    [Serializable]
    public class SerializableAddress
    {
        private static readonly int DefaultDisplayNameCodePage = Encoding.GetEncoding("utf-8").CodePage;

        public SerializableAddress()
        {
            DisplayNameCodePage = DefaultDisplayNameCodePage;
        }

        public SerializableAddress(string address, string displayName = null, Encoding encoding = null) : this()
        {
            var mailAddress = displayName == null ? new MailAddress(address) : new MailAddress(address, displayName);

            Address = mailAddress.Address;
            DisplayName = mailAddress.DisplayName;

            if (encoding != null)
                DisplayNameCodePage = encoding.CodePage;
        }

        public int DisplayNameCodePage { get; set; }
        public string Address { get; set; }
        public string DisplayName { get; set; }

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

        /// <summary>Compares two mail addresses.</summary>
        /// <param name="value">A <see cref="T:System.Net.Mail.MailAddress" /> instance to compare to the current instance.</param>
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
            var codePage = CodePageHelper.GetCodePage(address);
            return new SerializableAddress
            {
                Address = address.Address, DisplayName = address.DisplayName,
                DisplayNameCodePage = codePage ?? DefaultDisplayNameCodePage
            };
        }
    }
}