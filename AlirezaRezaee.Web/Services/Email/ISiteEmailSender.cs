using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Services.Email
{
    public interface ISiteEmailSender
    {
        Task SendAsync(Helpers.EmailTypes from,
            string to,
            string subject,
            string body,
            bool isBodyHtml = true,
            string cc = null,
            string bcc = null,
            AttachmentCollection attachments = null);
    }
}
