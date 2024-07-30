using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Interfaces;
using MVC.Models;

namespace MVC.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public DashboardRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;   
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<List<Race>> GetAllUserRacesAsync()
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        
        if (currentUser == null)
        {
            return [];
        }
        
        var userRaces = await _context.Races.Where(r => r.AppUserId == currentUser.ToString()).ToListAsync();
        return userRaces;
    }

    public async Task<List<Club>> GetAllUserClubsAsync()
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        
        if (currentUser == null)
        {
            return [];
        }
        
        var userClubs = await _context.Clubs.Where(r => r.AppUserId == currentUser.ToString()).ToListAsync();
        return userClubs;
    }
}