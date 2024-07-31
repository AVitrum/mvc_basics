using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Interfaces;
using MVC.Models;

namespace MVC.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<AppUser> GetUserByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception();
    }

    public async Task<bool> AddUserAsync(AppUser user)
    {
        await _context.Users.AddAsync(user);
        return await SaveAsync();
    }

    public async Task<bool> UpdateUserAsync(AppUser user)
    {
        _context.Users.Update(user);
        return await SaveAsync();
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await GetUserByIdAsync(id);
        _context.Users.Remove(user);
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        await _context.SaveChangesAsync();
        return true;
    }
}