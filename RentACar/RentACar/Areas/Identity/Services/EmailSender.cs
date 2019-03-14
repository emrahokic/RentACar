using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RentACar.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {

            string FromAddress = "noreplay@p1748.app.fit.ba";
            string FromAdressTitle = "Email confirmation for SunnyCar";
            //To Address  
            string ToAddress = email;
            string ToAdressTitle = "";
            string Subject = subject;
            string BodyContent = message;


            string SmtpServer = "smtp.sendgrid.net";
            //Smtp Port Number  
            int SmtpPortNumber = 587;

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
            mimeMessage.Subject = Subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = BodyContent

            };

            using (var client = new SmtpClient())
            {

                client.Connect(SmtpServer, SmtpPortNumber, false);
                // Note: only needed if the SMTP server requires authentication  
                // Error 5.5.1 Authentication   
                client.Authenticate("apikey", "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");
                client.Send(mimeMessage);
                Console.WriteLine("The mail has been sent successfully !!");
                client.Disconnect(true);

            }
            return Task.CompletedTask;
        }
    }
}
