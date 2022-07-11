using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Models
{
    public class User : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }

        public Guid? DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]

        public Department? Department { get; set; }

        public Role Role { get; set; }
        public List<ForgetPassword> ForgetPasswords { get; internal set; }
    }
}
