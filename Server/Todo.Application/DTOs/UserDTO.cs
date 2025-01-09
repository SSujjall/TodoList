using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UpdatePasswordDTO
    {
        public string? Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        //public string ConfirmPassword { get; set; }
    }

    public class UpdateUserDTO
    {
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
