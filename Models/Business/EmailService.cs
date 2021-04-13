using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

// using System.Net;
// using System.Net.Mail;

namespace Repository.Models.Business
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
        
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "spam.corp.asp@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
        
            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync("spam.corp.asp@gmail.com", "56MBNx76DhJEdgZ");
            await client.SendAsync(emailMessage);
            await client.SendAsync(emailMessage);
        
            await client.DisconnectAsync(true);
        }
        // public bool Send(string receiverEmail, string receiverName, string subject, string body)
        // {
        //     var mailMessage = new MailMessage();
        //     var mailAddress = new MailAddress
        //         ("spam.corp.asp@gmail.com", "Sender Name"); // abc@gmail.com = input Sender Email Address 
        //     mailMessage.From = mailAddress;
        //     mailAddress = new MailAddress(receiverEmail, receiverName);
        //     mailMessage.To.Add(mailAddress);
        //     mailMessage.Subject = subject;
        //     mailMessage.Body = body;
        //     mailMessage.IsBodyHtml = true;
        //
        //     var mailSender = new SmtpClient("smtp.gmail.com", 587)
        //     {
        //         EnableSsl = true,
        //         UseDefaultCredentials = false,
        //         DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
        //         Credentials = new NetworkCredential
        //             ("spam.corp.asp@gmail.com", "56MBNx76DhJEdgZ")   // abc@gmail.com = input sender email address  
        //         //pass = sender email password
        //     };
        //
        //     try
        //     {
        //         mailSender.Send(mailMessage);
        //         return true;
        //     }
        //     catch (SmtpFailedRecipientException ex)
        //     { 
        //         // Write the exception to a Log file.
        //     }
        //     catch (SmtpException ex)
        //     { 
        //         // Write the exception to a Log file.
        //     }
        //     finally
        //     {
        //         mailSender = null;
        //         mailMessage.Dispose();
        //     }
        //     return false;
        // }
    }
}