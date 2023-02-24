using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Pdf.ModelUtility.Model;

namespace Pdf.CommonUtility.EmailUtility
{
    public class EmailUtility
    {
        public static bool SendMail1(Email emailDetails)
        {
            bool isEmailSent = false;
            MimeMessage message = new();
            //sender information
            message.From.Add(new MailboxAddress("Neet UG Prep", "ugneetpreparations@gmail.com"));
            
            //receiver email
            message.To.Add(MailboxAddress.Parse(emailDetails.ReceiverAddress));

            //message subject
            message.Subject = emailDetails.Subject;

            //message body
            message.Body = new TextPart("plain")
            {
                Text = emailDetails.Message
            };

            using(SmtpClient client = new())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate("ugneetpreparations@gmail.com", "abcdefghijklmnopqrstuvwxyz");
                    client.Send(message);
                    isEmailSent = true;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    //client.Dispose();
                }
            }
            return isEmailSent;
        }
        public static bool SendMail(Email emailDetails)
        {
            bool isEmailSent = false;
            var mail = new MimeMessage();
            //SENDER INFORMATION
            mail.From.Add(new MailboxAddress("Neet UG Prep", "ugneetpreparations@gmail.com"));

            //RECEIVER INFORMATION
            mail.To.Add(new MailboxAddress(emailDetails.ReceiverName, emailDetails.ReceiverAddress));
            
            //MESSAGE SUBJECT
            mail.Subject = emailDetails.Subject;
            
            //MESSAGE BODY
            mail.Body = new TextPart("plain")
            {
                Text = emailDetails.Message
            };
            
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate("ugneetpreparations@gmail.com", "abcdefghijklmnopqrstuvwxyz");
                    client.Send(mail);
                    isEmailSent = true;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
            return isEmailSent;
        }
    }
}
