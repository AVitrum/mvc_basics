using MVC.Models;

namespace MVC.ViewModels;

public class DashboardViewModal
{
    public List<Race> Races { get; init; } = [];
    public List<Club> Clubs { get; init; } = [];
}