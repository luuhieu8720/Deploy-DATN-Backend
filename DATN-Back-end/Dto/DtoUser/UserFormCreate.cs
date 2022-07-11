using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoUser
{
    public class UserFormCreate
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username cannot be null")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First name cannot be null")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name cannot be null")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Date of birth cannot be null")]
        public DateTime DateOfBirth { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email cannot be null")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be null")]
        [MinLength(6)]
        public string Password { get; set; }

        public Guid? DepartmentId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You must choose user role")]
        public Role Role { get; set; }
    }
}
