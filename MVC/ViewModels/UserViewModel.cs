namespace MVC.ViewModels;

public class UserViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public int Pace { get; set; }
    public int Mileage { get; set; }
}