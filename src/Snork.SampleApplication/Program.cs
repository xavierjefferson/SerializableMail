using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Snork.SerializableMail;

namespace SampleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //build a message
            var mailMessage = new MailMessage("fromme@somewhere.com", "toyou@somewhere.com");
            mailMessage.Subject = "This is the subject";
            mailMessage.Body = "This is the body";
            mailMessage.IsBodyHtml = true;
            mailMessage.Headers["X-Something-Here"] = "something we don't need";

            //create a memory stream for the attachment
            var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.Unicode, 1000, true))
            {
                streamWriter.Write("This is the attachment contents");
            }
            //move pointer back to zero
            memoryStream.Position = 0;
            var attachment = new Attachment(memoryStream, "somefile.txt", "text/plain");
            attachment.ContentDisposition.Parameters["abc"] = "def";
            mailMessage.Attachments.Add(attachment);

            Console.WriteLine("JSON Version:");
            Console.WriteLine(SerializeToJson(mailMessage));
            Console.WriteLine();
            Console.WriteLine("XML Version:");
            Console.WriteLine(SerializeToXml(mailMessage));
            Console.ReadLine();
        }

        private static string SerializeToXml(MailMessage mailMessage)
        {
            //perform implicit conversion between types here
            SerializableMessage newMessage = mailMessage;
            var xmlSerializer = new XmlSerializer(typeof(SerializableMessage));
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, newMessage);
                return stringWriter.ToString();
            }
        }

        private static string SerializeToJson(MailMessage mailMessage)
        {
            //perform implicit conversion between types here
            SerializableMessage newMessage = mailMessage;
            return JsonConvert.SerializeObject(newMessage, Formatting.Indented);
        }
    }
}