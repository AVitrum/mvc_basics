using MVC.Data.Enums;
using MVC.Models;

namespace MVC.ViewModels;

public class CreateClubViewModel
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public IFormFile Image { get; init; } = null!;
    public ClubCategory ClubCategory { get; init; }
    public Address Address { get; init; } = new();
    public string AppUserId { get; init; } = string.Empty;
}