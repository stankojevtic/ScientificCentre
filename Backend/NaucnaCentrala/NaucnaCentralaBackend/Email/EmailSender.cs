using NaucnaCentralaBackend.Interfaces;
using System;
using System.Net;
using System.Net.Mail;

namespace NaucnaCentralaBackend.Email
{
    public class EmailSender : IEmailSender
    {
        public void Send(string emailSubject, string emailMessage)
        {
            try
            {
                var networkCredential = new NetworkCredential("stankojevtictfs@gmail.com", "Bucapro1");
                var mail = new MailMessage()
                {
                    From = new MailAddress("stankojevtictfs@gmail.com"),
                    Subject = emailSubject,
                    Body = emailMessage
                };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress("stankojevtictfs@gmail.com"));

                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = networkCredential
                };

                client.Send(mail);
            }
            catch (Exception exception)
            {
                
            }
        }
    }
}
