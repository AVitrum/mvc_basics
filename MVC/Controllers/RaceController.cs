using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Interfaces;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers;
public class RaceController : Controller
{
    private readonly IRaceRepository _raceRepository;
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
    {
        _raceRepository = raceRepository;
        _photoService = photoService;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: Race
    public async Task<IActionResult> Index()
    {
        var races = await _raceRepository.GetAll();
        return View(races);
    }

    // GET: Race/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var race = await _raceRepository.GetByIdAsync(id.Value);
        return View(race);
    }

    // GET: Race/Create
    public IActionResult Create()
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        if (currentUserId is null)
        {
            return RedirectToAction(nameof(Index));
        }
        
        var createRaceViewModel = new CreateRaceViewModel{ AppUserId = currentUserId };
        return View(createRaceViewModel);
    }

    // POST: Race/Create
    [HttpPost]
    public async Task<IActionResult> Create(CreateRaceViewModel createRaceViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(createRaceViewModel.Image);
            
            var race = new Race
            {
                Title = createRaceViewModel.Title,
                Description = createRaceViewModel.Description,
                Image = result.Url.ToString(),
                RaceCategory = createRaceViewModel.RaceCategory,
                AppUserId = createRaceViewModel.AppUserId,
                Address = new Address
                {
                    State = createRaceViewModel.Address.State,
                    City = createRaceViewModel.Address.City,
                    Street = createRaceViewModel.Address.Street
                }
            };
            await _raceRepository.AddAsync(race);
            return RedirectToAction(nameof(Index));
        }
        ModelState.AddModelError("", "Please fill all the required fields");
        return View(createRaceViewModel);
    }

    // GET: Race/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var race = await _raceRepository.GetByIdAsync(id.Value);
        
        var raceVm = new EditRaceViewModel
        {
            Id = race.Id,
            Title = race.Title,
            Description = race.Description,
            Url = race.Image,
            RaceCategory = race.RaceCategory,
            AddressId = race.AddressId,
            Address = race.Address,
        };
        
        return View(raceVm);
    }
    
    // POST: Club/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVm)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit club due to invalid model state.");
            return View("Edit", raceVm);
        }

        var userRace = await _raceRepository.GetByIdAsync(id);

        if (raceVm.Image != null)
        {
            var photoResult = await _photoService.AddPhotoAsync(raceVm.Image);
            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed: " + photoResult.Error.Message);
                return View("Edit", raceVm);
            }

            if (!string.IsNullOrEmpty(userRace.Image))
            {
                _ = _photoService.DeletePhotoAsync(userRace.Image);
            }

            userRace.Image = photoResult.Url.ToString();
        }

        userRace.Title = raceVm.Title;
        userRace.Description = raceVm.Description;
        userRace.RaceCategory = raceVm.RaceCategory;
        userRace.AddressId = raceVm.AddressId;
        userRace.Address = raceVm.Address;

        await _raceRepository.UpdateAsync(userRace);
        return RedirectToAction(nameof(Index));
    }

    // GET: Race/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var race = await _raceRepository.GetByIdAsync(id.Value);

        return View(race);
    }

    // POST: Race/Delete/5
    [HttpPost, ActionName("DeleteRace")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var race = await _raceRepository.GetByIdAsync(id.Value);
        await _raceRepository.DeleteAsync(race);
        return RedirectToAction(nameof(Index));
    }
}
