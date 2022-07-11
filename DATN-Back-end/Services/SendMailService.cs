using DATN_Back_end.Config;
using DATN_Back_end.Dto.DtoMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public class SendMailService : ISendMailService
    {
        private readonly MailConfig mailConfig;

        public SendMailService(MailConfig mailConfig)
        {
            this.mailConfig = mailConfig;
        }

        public async Task Send(MailContent mailContent)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(mailConfig.Mail, mailConfig.DisplayName),
                Subject = mailContent.Subject,
                Body = mailContent.Body,
                IsBodyHtml = true,
                Priority = MailPriority.High
            };

            mail.To.Add(new MailAddress(mailContent.To));

            using var smtp = new SmtpClient(mailConfig.Host, mailConfig.Port)
            {
                Credentials = new NetworkCredential(mailConfig.Mail, mailConfig.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
