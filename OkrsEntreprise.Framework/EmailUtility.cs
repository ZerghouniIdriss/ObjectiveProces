using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OkrsEntreprise.Framework
{
    public class EmailUtility
    {
        private void SendEmail(string emailId, EmailMessageDetails messageDetails)
        {
            if (!string.IsNullOrEmpty(emailId))
            {
               
                MailMessage message = new MailMessage();
                message.From = new MailAddress("okrsinfo@gmail.com", "Team OKRS");
                message.Subject = messageDetails.Subject; // string.Format("Invitation from {0} to join his team.", username);
                message.Body = messageDetails.Body;
                message.IsBodyHtml = true;
                message.To.Add(emailId);

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.Credentials = new NetworkCredential("okrsinfo@gmail.com", "okrsinfo@gmail");
                client.EnableSsl = true;

                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }
            

        }
        public void SendEmails(string[] emails, EmailMessageDetails messageDetails)
        {

            try
            {
                for (int i = 0; i < emails.Length; i++)
                {
                    SendEmail(emails[i], messageDetails);
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
         
        }


    }
}
