using Microsoft.AspNetCore.Identity;
using Todo.Application.DTOs;
using Todo.Application.Interface.IRepositories;
using Todo.Application.Interface.IServices;
using Todo.Domain.Entities;
using Todo.Infrastructure.Context;

namespace Todo.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public UserService(UserManager<User> userManager, IUserRepository userRepository, AppDbContext context)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<string> UpdateUserDetail(UpdateUserDTO updateUserDto, List<string> errors)
        {
            var user = await _userManager.FindByIdAsync(updateUserDto.Id);

            if (user == null)
            {
                errors.Add("User not found.");
                return "Update failed.";
            }

            bool isUpdated = false;

            // Update UserName if it has changed
            if (updateUserDto.UserName != null && updateUserDto.UserName != user.UserName)
            {
                user.UserName = updateUserDto.UserName;
                isUpdated = true;
            }

            // Update FirstName if it has changed
            if (updateUserDto.FirstName != null && updateUserDto.FirstName != user.FirstName)
            {
                user.FirstName = updateUserDto.FirstName;
                isUpdated = true;
            }

            // Update LastName if it has changed
            if (updateUserDto.LastName != null && updateUserDto.LastName != user.LastName)
            {
                user.LastName = updateUserDto.LastName;
                isUpdated = true;
            }

            // Save changes if any updates were made
            if (isUpdated)
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return "User details updated successfully.";
                }
                else
                {
                    errors.AddRange(result.Errors.Select(e => e.Description));
                    return "Update failed.";
                }
            }
            else
            {
                return "No updates necessary.";
            }
        }

        public async Task<string> UpdateUserPassword(UpdatePasswordDTO updatePasswordDto, List<string> errors)
        {
            var user = await _userManager.FindByIdAsync(updatePasswordDto.Id);

            if (user == null)
            {
                errors.Add("User not found.");
                return "Update failed.";
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(
                user,
                updatePasswordDto.CurrentPassword,
                updatePasswordDto.NewPassword
            );

            if (!changePasswordResult.Succeeded)
            {
                errors.AddRange(changePasswordResult.Errors.Select(e => e.Description));
                return "Password update failed.";
            }

            return "User updated successfully.";
        }

        public async Task<string> DeleteUser(string userId, List<string> errors)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                errors.Add("User not found.");
                return "Delete failed.";
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                errors.Add("Delete operation failed.");
                return "Delete failed.";
            }

            return "User deleted successfully.";
        }
    }
}
