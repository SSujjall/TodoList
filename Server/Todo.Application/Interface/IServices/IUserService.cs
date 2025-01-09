using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.DTOs;

namespace Todo.Application.Interface.IServices
{
    public interface IUserService
    {
        Task<string> UpdateUserPassword(UpdatePasswordDTO updatePasswordDto, List<string> errors);
        Task<string> UpdateUserDetail(UpdateUserDTO updateUserDto, List<string> errors);
        Task<string> DeleteUser(string userId, List<string> errors);
    }
}