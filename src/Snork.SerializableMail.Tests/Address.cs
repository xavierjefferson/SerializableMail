using System;
using System.Net.Mail;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Snork.SerializableMail.Tests
{
    [TestClass]
    public class Address
    {
        private const string address = "ok@ok.com";
        private const string displayName = "Yes";
        private static readonly Encoding displayNameEncoding = Encoding.ASCII;

        [TestMethod]
        public void MailAddressToSerializableAddress()
        {
            var mailAddress = new MailAddress(address, displayName, displayNameEncoding);
            //implicit conversion
            SerializableAddress serializableAddress = mailAddress;
            Assert.AreEqual(serializableAddress.DisplayNameCodePage, displayNameEncoding.CodePage);
            Assert.AreEqual(serializableAddress.Address, mailAddress.Address);
            Assert.AreEqual(serializableAddress.DisplayName, mailAddress.DisplayName);
        }

        [TestMethod]
        public void Overload1()
        {
            var serializableAddress = new SerializableAddress(address);
            Assert.AreEqual(serializableAddress.DisplayNameCodePage, Encoding.GetEncoding("utf-8").CodePage);
        }

        [TestMethod]
        public void Overload2()
        {
            var serializableAddress = new SerializableAddress(address, displayName, displayNameEncoding);
            Assert.AreEqual(serializableAddress.DisplayNameCodePage, displayNameEncoding.CodePage);
            Assert.AreEqual(serializableAddress.Address, address);
            Assert.AreEqual(serializableAddress.DisplayName, displayName);
        }

        [TestMethod]
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

            Assert.IsNotNull(outer);
        }

        [TestMethod]
        public void SerializableAddressToMailAddress()
        {
            var serializableAddress = new SerializableAddress(address, displayName, displayNameEncoding);
            //implicit conversion
            MailAddress mailAddress = serializableAddress;
            var codePage = CodePageHelper.GetCodePage(mailAddress);
            Assert.IsTrue(codePage.HasValue);
            Assert.AreEqual(codePage.Value, displayNameEncoding.CodePage);
            Assert.AreEqual(mailAddress.Address, serializableAddress.Address);
            Assert.AreEqual(mailAddress.DisplayName, serializableAddress.DisplayName);
        }

        [TestMethod]
        public void SerializableAddressCompare()
        {
            var a1 = new SerializableAddress(address);
            var a2 = new SerializableAddress(address);
            Assert.AreEqual(a1, a2);
        }
    }
}