using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Data;
using MVC.Models;

namespace MVC.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly AppDbContext _context;
    
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    

    // GET: AccountController
    public ActionResult Login()
    {
        var response = new LoginViewModel();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel) 
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
        if (user == null)
        {
            TempData["Error"] = "Wrong email or password. Please try again.";
            return View(loginViewModel);
        }

        var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
        if (!passwordCheck)
        {
            TempData["Error"] = "Invalid login attempt";
            return View(loginViewModel);
        }
        
        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Race");
        }

        TempData["Error"] = "Invalid login attempt";
        return View(loginViewModel);
    }
}
