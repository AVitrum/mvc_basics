using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardRepository _dashboardRepository;
    
    public DashboardController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }
    
    // GET: Dashboard
    public async Task<IActionResult> Index()
    {
        List<Race> userRaces = await _dashboardRepository.GetAllUserRacesAsync();
        List<Club> userClubs = await _dashboardRepository.GetAllUserClubsAsync();

        var dashboardViewModel = new DashboardViewModal
        {
            Races = userRaces,
            Clubs = userClubs
        };
        
        return View(dashboardViewModel);
    }
}