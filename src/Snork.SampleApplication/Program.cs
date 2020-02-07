using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Snork.SerializableMail;

namespace Snork.SampleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //build a message
            var mailMessage = new SerializableMessage("fromme@somewhere.com", "toyou@somewhere.com")
            {
                Subject = "This is the subject",
                Body = "This is the body",
                IsBodyHtml = true,
                Headers = {["X-Something-Here"] = "something we don't need"}
            };

            //create a memory stream for the attachment
            var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.ASCII, 1000, true))
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
            Console.WriteLine();
            Console.WriteLine("Press ENTER to send");
            Console.ReadLine();
            using (var client = new SmtpClient())
            {
                //perform implicit conversion between types here.  No other code is necessary!
                client.Send(mailMessage);
            }
            Console.ReadLine();
        }

        private static string SerializeToXml(MailMessage mailMessage)
        {
            var xmlSerializer = new XmlSerializer(typeof(SerializableMessage));
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, (SerializableMessage) mailMessage);
                return stringWriter.ToString();
            }
        }

        private static string SerializeToJson(MailMessage mailMessage)
        {
            return JsonConvert.SerializeObject((SerializableMessage) mailMessage, Formatting.Indented);
        }
    }
}