using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;

namespace Snork.SerializableMail
{
    /// <summary>Store e-mail addresses that are associated with an e-mail message.</summary>
    [Serializable]
    public class SerializableAddressCollection : Collection<SerializableAddress>
    {
        public SerializableAddressCollection()
        {
        }

        public SerializableAddressCollection(IEnumerable<SerializableAddress> collection) : base(
            collection.ToList())
        {
        }

        /// <summary>Add a list of e-mail addresses to the collection.</summary>
        /// <param name="addresses">
        ///     The e-mail addresses to add to the <see cref="T:Snork.SerializableMail.SerializableMailAddressCollection" />.
        ///     Multiple e-mail addresses must be separated with a comma character (",").
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">The<paramref name=" addresses" /> parameter is null.</exception>
        /// <exception cref="T:System.ArgumentException">The<paramref name=" addresses" /> parameter is an empty string.</exception>
        /// <exception cref="T:System.FormatException">
        ///     The<paramref name="addresses" /> parameter contains an e-mail address that
        ///     is invalid or not supported.
        /// </exception>
        public void Add(string addresses)
        {
            if (addresses == null) throw new ArgumentNullException();
            if (addresses == string.Empty) throw new ArgumentNullException();
            var mailAddressCollection = new MailAddressCollection {addresses};
            foreach (var item in mailAddressCollection) Add(item);
        }

        /// <summary>
        ///     Returns a string representation of the e-mail addresses in this
        ///     <see cref="T:Snork.SerializableMail.SerializableMailAddressCollection" /> object.
        /// </summary>
        /// <returns>A <see cref="T:System.String" /> containing the e-mail addresses in this collection.</returns>
        public override string ToString()
        {
            return string.Join(", ", this.Select(i => i.ToString()));
        }

        public static implicit operator MailAddressCollection(SerializableAddressCollection list)
        {
            if (list == null) return null;
            var result = new MailAddressCollection();
            foreach (var item in list) result.Add(item);

            return result;
        }

        public static implicit operator SerializableAddressCollection(MailAddressCollection list)
        {
            if (list == null) return null;
            return new SerializableAddressCollection(list.Select(i => (SerializableAddress) i));
        }
    }
}