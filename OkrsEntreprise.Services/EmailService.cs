using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OkrsEntreprise.Services
{

    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
        void SendEmail(string[] to, string subject, string body);
        Task SendEmailAsync(string to, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {

            if (!string.IsNullOrEmpty(to))
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("noreply@ObjectiveProcess.com", "ObjectiveProcess Team");
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.To.Add(to);

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.Credentials = new NetworkCredential("ObjectiveProcess@gmail.com", "ObjectiveProcess2016");
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


        public async Task SendEmailAsync(string to, string subject, string body)
        {

            if (!string.IsNullOrEmpty(to))
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("noreply@ObjectiveProcess.com", "ObjectiveProcess Team");
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.To.Add(to);

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.Credentials = new NetworkCredential("ObjectiveProcess@gmail.com", "ObjectiveProcess2016");
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


        public void SendEmail(string[] to, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("noreply@ObjectiveProcess.com", "ObjectiveProcess Team");
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            foreach (var recepientEmail in to)
            {
                if (string.IsNullOrEmpty(recepientEmail))
                {
                    message.To.Add(recepientEmail);
                }
            }

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Credentials = new NetworkCredential("ObjectiveProcess@gmail.com", "ObjectiveProcess2016");
            client.EnableSsl = true;

            {
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

    }
}
