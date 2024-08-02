using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Interfaces;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers;

public class ClubController : Controller
{
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
    {
        _clubRepository = clubRepository;
        _photoService = photoService;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: Club
    public async Task<IActionResult> Index()
    {
        IEnumerable<Club> clubs = await _clubRepository.GetAll();
        return View(clubs);
    }

    //GET: Club/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        
        Club club = await _clubRepository.GetByIdAsync(id.Value);
        return View(club);
    }

    // GET: Club/Create
    public IActionResult Create()
    {
        string? currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var createClubViewModel = new CreateClubViewModel { AppUserId = currentUserId };
        return View(createClubViewModel);
    }
    
    // POST: Club/Create
    [HttpPost]
    public async Task<IActionResult> Create(CreateClubViewModel createClubViewModel)
    {
        if (ModelState.IsValid)
        {
            ImageUploadResult result = await _photoService.AddPhotoAsync(createClubViewModel.Image);
            
            var club = new Club
            {
                Title = createClubViewModel.Title,
                Description = createClubViewModel.Description,
                Image = result.Url.ToString(),
                ClubCategory = createClubViewModel.ClubCategory,
                AppUserId = createClubViewModel.AppUserId,
                Address = new Address
                {
                    State = createClubViewModel.Address.State,
                    City = createClubViewModel.Address.City,
                    Street = createClubViewModel.Address.Street
                }
            };
            await _clubRepository.AddAsync(club);
            return RedirectToAction(nameof(Index));
        }
        ModelState.AddModelError("", "Please fill all the required fields");
        return View(createClubViewModel);
    }
    
    // GET: Club/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        Club club = await _clubRepository.GetByIdAsync(id.Value);
        
        var clubViewModel = new EditClubViewModel
        {
            Id = club.Id,
            Title = club.Title,
            Description = club.Description,
            Url = club.Image,
            ClubCategory = club.ClubCategory,
            AddressId = club.AddressId,
            Address = club.Address,
        };
        
        return View(clubViewModel);
    }
    
    // POST: Club/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditClubViewModel editClubViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit club due to invalid model state.");
            return View("Edit", editClubViewModel);
        }

        Club userClub = await _clubRepository.GetByIdAsync(id);

        if (editClubViewModel.Image != null)
        {
            ImageUploadResult photoResult = await _photoService.AddPhotoAsync(editClubViewModel.Image);
            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed: " + photoResult.Error.Message);
                return View("Edit", editClubViewModel);
            }

            if (!string.IsNullOrEmpty(userClub.Image))
            {
                _ = _photoService.DeletePhotoAsync(userClub.Image);
            }

            userClub.Image = photoResult.Url.ToString();
        }

        userClub.Title = editClubViewModel.Title;
        userClub.Description = editClubViewModel.Description;
        userClub.ClubCategory = editClubViewModel.ClubCategory;
        userClub.AddressId = editClubViewModel.AddressId;
        userClub.Address = editClubViewModel.Address;

        await _clubRepository.UpdateAsync(userClub);
        return RedirectToAction(nameof(Index));
    }
    
    // GET: Club/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        Club club = await _clubRepository.GetByIdAsync(id.Value);
        return View(club);
    }

    // POST: Club/Delete/5
    [HttpPost, ActionName("DeleteClub")]
    public async Task<IActionResult> DeleteClub(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        Club club = await _clubRepository.GetByIdAsync(id.Value);
        
        await _clubRepository.DeleteAsync(club);
        return RedirectToAction(nameof(Index));
    }
}
