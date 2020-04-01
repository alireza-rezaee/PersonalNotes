using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AlirezaRezaee.Web.Helpers
{
    public enum EmailTypes : byte
    {
        Direct, Newsletter, Sales, NoReply
    }

    public class EmailSettings
    {
        public static MailAddress GetMailAddress(EmailTypes type)
        {
            if (type == EmailTypes.Direct)
                return new MailAddress("direct@rezaee.org", "علیرضا رضائی");
            else if (type == EmailTypes.Newsletter)
                return new MailAddress("newsletter@rezaee.org", "خبرنامه وبسایت علیرضا رضائی");
            else if (type == EmailTypes.Sales)
                return new MailAddress("sales@rezaee.org", "سفارشات وبسایت علیرضا رضائی");
            else //if (type == EmailTypes.NoReply || ...) 
                return new MailAddress("no-reply@rezaee.org", "وبسایت علیرضا رضائی");
        }

        public static SmtpClient GetSmtpClient(EmailTypes type)
        {
            if (type == EmailTypes.Direct)
                return new SmtpClient
                {
                    Host = "mail.rezaee.org",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential() { UserName = "direct@rezaee.org", Password = "63K%GmRLf4yJBy+7" }
                };
            else if (type == EmailTypes.Newsletter)
                return new SmtpClient
                {
                    Host = "mail.rezaee.org",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential() { UserName = "newsletter@rezaee.org", Password = "8^xu4!2yZ^WNHbFR" }
                };
            else if (type == EmailTypes.Sales)
                return new SmtpClient
                {
                    Host = "mail.rezaee.org",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential() { UserName = "sales@rezaee.org", Password = "C3$AZJ&3sndfVTQJ" }
                };
            else //if (type == EmailTypes.NoReply || ...) 
                return new SmtpClient
                {
                    Host = "mail.rezaee.org",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential() { UserName = "no-reply@rezaee.org", Password = "vb&9WNKJV@JyU-9e" }
                };
        }
    }
}
