using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Interfaces;
using MVC.Models;

namespace MVC.Repositories;

public class RaceRepository : IRaceRepository
{
    private readonly AppDbContext _context;
    
    public RaceRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Race>> GetAll()
    {
        return await _context.Races
            .Include(r => r.Address)
            .Include(r => r.AppUser)
            .ToListAsync();
    }

    public async Task<Race> GetByIdAsync(int id)
    {
        return await _context.Races
            .Include(c => c.Address)
            .Include(c => c.AppUser)
            .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception();
    }

    public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
    {
        return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
    }

    public bool Add(Race race)
    {
        _context.Add(race);
        return Save();
    }

    public async Task<bool> AddAsync(Race race)
    {
        await _context.AddAsync(race);
        return await SaveAsync();
    }

    public bool Update(Race race)
    {
        _context.Update(race);
        return Save();
    }

    public async Task<bool> UpdateAsync(Race userRace)
    {
        _context.Update(userRace);
        return await SaveAsync();
    }

    public bool Delete(Race race)
    {
        _context.Remove(race);
        return Save();
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