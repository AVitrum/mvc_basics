using MVC.Data.Enums;
using MVC.Models;

namespace MVC.ViewModels;

public class EditClubViewModel
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public IFormFile? Image { get; set; }
    public string? Url { get; set; }
    public required int AddressId { get; set; }
    public required Address Address { get; set; }
    public required ClubCategory ClubCategory { get; set; }
}