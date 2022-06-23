using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Xunit;

namespace Snork.SerializableMail.Tests
{
    public class Address
    {
        private const string address = "ok@ok.com";
        private const string displayName = "Yes";
        private static readonly Encoding displayNameEncoding = Encoding.ASCII;

        [Fact]
        public void MailAddressToSerializableAddress()
        {
            var mailAddress = new MailAddress(address, displayName, displayNameEncoding);
            //implicit conversion
            SerializableAddress serializableAddress = mailAddress;
            Assert.Equal(serializableAddress.DisplayNameCodePage, displayNameEncoding.CodePage);
            Assert.Equal(serializableAddress.Address, mailAddress.Address);
            Assert.Equal(serializableAddress.DisplayName, mailAddress.DisplayName);
        }

        [Fact]
        public void Overload1()
        {
            var serializableAddress = new SerializableAddress(address);
            Assert.Equal(serializableAddress.DisplayNameCodePage, Encoding.GetEncoding("utf-8").CodePage);
        }

        [Fact]
        public void Overload2()
        {
            var serializableAddress = new SerializableAddress(address, displayName, displayNameEncoding);
            Assert.Equal(serializableAddress.DisplayNameCodePage, displayNameEncoding.CodePage);
            Assert.Equal(serializableAddress.Address, address);
            Assert.Equal(serializableAddress.DisplayName, displayName);
        }

        [Fact]
        public void NullAddress()
        {
            Exception outer = null;
            try
            {
                new SerializableAddress(null);
            }
            catch (Exception ex)
            {
                outer = ex;
            }

            Assert.NotNull(outer);
        }

        [Fact]
        public void SerializableAddressToMailAddress()
        {
            var serializableAddress = new SerializableAddress(address, displayName, displayNameEncoding);
            //implicit conversion
            MailAddress mailAddress = serializableAddress;
            var codePage = CodePageHelper.GetCodePage(mailAddress);
            Assert.True(codePage.HasValue);
            Assert.Equal(codePage.Value, displayNameEncoding.CodePage);
            Assert.Equal(mailAddress.Address, serializableAddress.Address);
            Assert.Equal(mailAddress.DisplayName, serializableAddress.DisplayName);
        }

        [Fact]
        public void SerializableAddressCompare()
        {
            var a1 = new SerializableAddress(address);
            var a2 = new SerializableAddress(address);
            Assert.Equal(a1, a2);
        }
    }
}
