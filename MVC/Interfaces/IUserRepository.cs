using MVC.Models;

namespace MVC.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetAllUsersAsync();
    Task<AppUser> GetUserByIdAsync(string id);
    Task<bool> AddUserAsync(AppUser user);
    Task<bool> UpdateUserAsync(AppUser user);
    Task<bool> DeleteUserAsync(string id);
    Task<bool> SaveAsync();
}