using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Interfaces;
using MVC.Models;

namespace MVC.Repositories;

public class ClubRepository : IClubRepository
{
    private readonly AppDbContext _context;
    
    public ClubRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Club>> GetAll()
    {
        return await _context.Clubs.ToListAsync();
    }

    public async Task<Club> GetByIdAsync(int id)
    {
        return await _context.Clubs
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception();
    }

    public async Task<IEnumerable<Club>> GetClubsByCity(string city)
    {
        return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
    }

    public bool Add(Club club)
    {
        _context.Add(club);
        return Save();
    }

    public async Task<bool> AddAsync(Club club)
    {
        await _context.AddAsync(club);
        return await SaveAsync();
    }

    public bool Update(Club club)
    {
        _context.Update(club);
        return Save();
    }

    public async Task<bool> UpdateAsync(Club club)
    {
        _context.Update(club);
        return await SaveAsync();
    }
    
    public bool Delete(Club club)
    {
        _context.Remove(club);
        return Save();
    }

    public async Task<bool> DeleteAsync(Club club)
    {
        _context.Remove(club);
        return await SaveAsync();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
    
    public async Task<bool> SaveAsync()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0;
    }
}