using System.Collections.Generic;
using System.Net.Mail;

namespace Snork.SerializableMail
{
    public class SerializableMailAddressList : List<SerializableMailAddress>
    {
        public SerializableMailAddressList()
        {
        }

        public SerializableMailAddressList(IEnumerable<SerializableMailAddress> collection) : base(collection)
        {
        }

        public void Add(string addresses)
        {
            var mailAddressCollection = new MailAddressCollection();
            mailAddressCollection.Add(addresses);
            foreach (var item in mailAddressCollection)
            {
                Add(item);
            }
        }

        public static implicit operator MailAddressCollection(SerializableMailAddressList list)
        {
            if (list == null)
            {
                return null;
            }
            var result = new MailAddressCollection();
            foreach (var item in list)
            {
                result.Add(item);
            }

            return result;
        }

        public static implicit operator SerializableMailAddressList(MailAddressCollection list)
        {
            if (list == null)
            {
                return null;
            }
            var result = new SerializableMailAddressList();
            foreach (var item in list)
            {
                result.Add(item);
            }

            return result;
        }
    }
}