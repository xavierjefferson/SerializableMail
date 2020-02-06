using System;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Snork.SerializableMail
{
    public static class CodePageHelper
    {
        private static readonly Type MailAddressType = typeof(MailAddress);
        private static readonly Type EncodingType = typeof(Encoding);

        public static int? GetCodePage(MailAddress address)
        {
            int? codePage = null;

            var fieldInfo = MailAddressType.GetField("displayNameEncoding", BindingFlags
                                                                                .GetField | BindingFlags.NonPublic |
                                                                            BindingFlags.Instance);

            if (fieldInfo != null &&
                fieldInfo.FieldType == EncodingType) codePage = (fieldInfo.GetValue(address) as Encoding).CodePage;
            return codePage;
        }
    }
}