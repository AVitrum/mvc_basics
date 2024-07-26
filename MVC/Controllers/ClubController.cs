using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers;

public class ClubController : Controller
{
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photoService;
    
    public ClubController(IClubRepository clubRepository, IPhotoService photoService)
    {
        _clubRepository = clubRepository;
        _photoService = photoService;
    }

    // GET: ClubController
    public async Task<IActionResult> Index()
    {
        var clubs = await _clubRepository.GetAll();
        return View(clubs);
    }

    //GET: ClubController/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var club = await _clubRepository.GetByIdAsync(id.Value);
        return View(club);
    }

    // GET: Club/Create
    public IActionResult Create()
    {
        return View();
    }
    
    // POST: Club/Create
    [HttpPost]
    public async Task<IActionResult> Create(CreateClubViewModel createClubViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(createClubViewModel.Image);
            
            var club = new Club
            {
                Title = createClubViewModel.Title,
                Description = createClubViewModel.Description,
                Image = result.Url.ToString(),
                ClubCategory = createClubViewModel.ClubCategory,
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
        
        var club = await _clubRepository.GetByIdAsync(id.Value);
        
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

        var userClub = await _clubRepository.GetByIdAsync(id);

        if (editClubViewModel.Image != null)
        {
            var photoResult = await _photoService.AddPhotoAsync(editClubViewModel.Image);
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
}
