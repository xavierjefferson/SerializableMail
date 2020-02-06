# Snork.SerializableMail
This is a library for C#.  In normal conditions, the following objects aren't serializable:

 - System.Net.Mail.Message
 - System.Net.Mail.MailAddress
 - System.Net.Mail.Attachment
 - System.Net.Mail.AddressCollection
 - System.Net.Mime.ContentDisposition
 - System.Net.Mime.ContentType

This means that if you're interested in serializing the contents of a mail message to binary, XML, or JSON, you're normally going to be out of luck.

Snork.SerializableMail does implicit conversions from the built-in .NET types to instances of classes that will serialize just fine.   This includes addresses, text encoding settings, and even the attachments.

## Sample Code (needs Newtonsoft.JSON package)
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

## JSON output
    {
      "Headers": {
        "X-Something-Here": "something we don't need"
      },
      "Body": "This is the body",
      "Subject": "This is the subject",
      "From": {
        "DisplayNameCodePage": 65001,
        "Address": "fromme@somewhere.com",
        "DisplayName": ""
      },
      "To": [
        {
          "DisplayNameCodePage": 65001,
          "Address": "toyou@somewhere.com",
          "DisplayName": ""
        }
      ],
      "ReplyToList": [],
      "CC": [],
      "Bcc": [],
      "BodyTransferEncoding": -1,
      "IsBodyHtml": true,
      "HeadersCodePage": null,
      "BodyCodePage": 20127,
      "Sender": null,
      "DeliveryNotificationOptions": 0,
      "Attachments": [
        {
          "Name": "somefile.txt",
          "NameEncodingCodePage": null,
          "ContentDisposition": {
            "Parameters": {
              "abc": "def"
            },
            "FileName": null,
            "DispositionType": "attachment",
            "Inline": false,
            "Boundary": null,
            "MediaDisposition": null,
            "CreationDate": "0001-01-01T00:00:00",
            "ModificationDate": "0001-01-01T00:00:00",
            "Size": -1,
            "ReadDate": "0001-01-01T00:00:00"
          },
          "ContentId": "0cccface-1b83-45c7-9802-e77c471c1587",
          "ContentType": {
            "Name": "somefile.txt",
            "CharSet": null,
            "Boundary": null,
            "MediaType": "text/plain"
          },
          "ContentBytes": "//5UAGgAaQBzACAAaQBzACAAdABoAGUAIABhAHQAdABhAGMAaABtAGUAbgB0ACAAYwBvAG4AdABlAG4AdABzAA==",
          "TransferEncoding": 1
        }
      ],
      "Priority": 0,
      "SubjectCodePage": null
    }
