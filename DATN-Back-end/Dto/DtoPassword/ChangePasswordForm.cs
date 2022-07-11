using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoPassword
{
    public class ChangePasswordForm
    {
        public string Code { get; set; }

        [EmailAddress(ErrorMessage = "This field must match the email format")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be null")]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
