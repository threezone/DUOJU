using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Duoju.DAO.Utilities
{
    public class MailUtility
    {
        public static void Send(string toUser, string fromUser, string subject, string body, string userName, string password, string smtpHost, string displayName)
        {
            MailAddress mailFrom = new MailAddress(fromUser, displayName);
            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.From = mailFrom;
            mail.Priority = MailPriority.Normal;
            mail.To.Add(new MailAddress(toUser));
            
            mail.Body = body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;

            mail.Priority = MailPriority.Normal;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            SmtpClient client = new SmtpClient();
            client.Host = smtpHost;
            client.Port = 25;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(userName, password);
            //client.EnableSsl = true;
            //client.SendAsync(mail, null);

            client.Send(mail);
        }
    }
}
