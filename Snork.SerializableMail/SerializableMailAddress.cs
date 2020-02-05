using System.Net.Mail;

namespace Snork.SerializableMail
{
    public class SerializableMailAddress
    {
        public SerializableMailAddress()
        {
        }

        public SerializableMailAddress(string address, string displayName = null)
        {
            var mailAddress = displayName != null ? new MailAddress(address, displayName) : new MailAddress(address);

            Address = mailAddress.Address;
            DisplayName = mailAddress.DisplayName;
        }


        public string Address { get; set; }
        public string DisplayName { get; set; }

        public static implicit operator MailAddress(SerializableMailAddress address)
        {
            if (address == null)
            {
                return null;
            }
            return new MailAddress(address.Address, address.DisplayName);
        }

        public static implicit operator SerializableMailAddress(MailAddress address)
        {
            if (address == null)
            {
                return null;
            }
            return new SerializableMailAddress {Address = address.Address, DisplayName = address.DisplayName};
        }
    }
}