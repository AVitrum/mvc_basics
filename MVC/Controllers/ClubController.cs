using Microsoft.AspNetCore.Mvc;
using MVC.Interfaces;

namespace MVC.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;

        public ClubController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
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
            
            var club = await _clubRepository.GetById(id.Value);
            return View(club);
        }
    }
}
