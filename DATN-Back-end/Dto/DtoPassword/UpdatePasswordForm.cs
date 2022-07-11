using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoPassword
{
    public class UpdatePasswordForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be null")]
        [MinLength(6)]
        public string OldPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be null")]
        [MinLength(6)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be null")]
        [MinLength(6)]
        public string ConfirmPassword { get; set; }
    }
}
