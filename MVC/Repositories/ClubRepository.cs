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

    public async Task<Club> GetById(int id)
    {
        return await _context.Clubs
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.Id == id);
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

    public bool Update(Club club)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Club club)
    {
        _context.Remove(club);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}