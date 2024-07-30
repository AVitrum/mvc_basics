using MVC.Models;

namespace MVC.Interfaces;

public interface IDashboardRepository
{
    Task<List<Race>> GetAllUserRacesAsync();
    Task<List<Club>> GetAllUserClubsAsync();
    
}