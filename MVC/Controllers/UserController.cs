using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers;

[Route("users")]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    // GET: /users
    public async Task<IActionResult> Index()
    {
        IEnumerable<AppUser> users = await _userRepository.GetAllUsersAsync();
        List<UserViewModel> result = [];
        
        foreach (var user in users)
        {
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Pace = user.Pace ?? 0,
                Mileage = user.Mileage ?? 0
            };
            result.Add(userViewModel);
        }
        return View(result);
    }
    
    // GET: /users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Details(string id)
    {
        AppUser user = await _userRepository.GetUserByIdAsync(id);
        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Pace = user.Pace ?? 0,
            Mileage = user.Mileage ?? 0
        };
        return View(userViewModel);
    }
}