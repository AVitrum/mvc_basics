using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Data;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly AppDbContext _context;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    // GET: Account/Login
    public ActionResult Login()
    {
        var response = new LoginViewModel();
        return View(response);
    }

    // POST: Account/Login
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

    // GET: Account/Register
    public ActionResult Register()
    {
        var response = new RegisterViewModel();
        return View(response);
    }

    // POST: Account/Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerViewModel);
        }

        var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
        if (user != null)
        {
            TempData["Error"] = "Email address is already in use.";
            return View(registerViewModel);
        }

        var newUser = new AppUser
        {
            UserName = registerViewModel.EmailAddress,
            Email = registerViewModel.EmailAddress
        };
        var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

        if (newUserResponse.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        }

        return RedirectToAction("Index", "Home");
    }
    
    // GET: Account/Logout
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}