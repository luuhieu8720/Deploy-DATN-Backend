using DATN_Back_end.Dto.DtoMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public interface ISendMailService
    {
        Task Send(MailContent mailContent);
    }
}
