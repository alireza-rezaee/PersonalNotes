using AlirezaRezaee.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Services.Email
{
    public class SiteEmailSender : ISiteEmailSender
    {
        public async Task SendAsync(EmailTypes from,
            string to,
            string subject,
            string body,
            bool isBodyHtml = true,
            string cc = null,
            string bcc = null,
            AttachmentCollection attachments = null)
        {
            //1- Set Email Message
            var mailMessage = new MailMessage();
            mailMessage.From = EmailSettings.GetMailAddress(from);
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = isBodyHtml;
            mailMessage.Body = body;
            if (cc != null)
                mailMessage.CC.Add(cc);
            if (bcc != null)
                mailMessage.Bcc.Add(bcc);
            if (attachments != null)
                foreach (var attachment in attachments)
                    mailMessage.Attachments.Add(attachment);


            //2- Set SmtpClient
            var smtp = EmailSettings.GetSmtpClient(from);

            //3- Send Email
            await smtp.SendMailAsync(mailMessage);
        }
    }
}