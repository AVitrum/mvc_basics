using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Interfaces;
using MVC.Models;

namespace MVC.Controllers
{
    public class RaceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IRaceRepository _raceRepository;

        public RaceController(AppDbContext context, IRaceRepository raceRepository)
        {
            _context = context;
            _raceRepository = raceRepository;
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

            var race = await _raceRepository.GetById(id.Value);
            return View(race);
        }

        // GET: Race/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id");
            ViewData["AppUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id");
            return View();
        }

        // POST: Race/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Title,Description,Image,AddressId,RaceCategory,AppUserId")] Race race)
        {
            if (ModelState.IsValid)
            {
                _raceRepository.Add(race);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", race.AddressId);
            ViewData["AppUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", race.AppUserId);
            return View(race);
        }

        // GET: Race/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races.FindAsync(id);
            if (race == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", race.AddressId);
            ViewData["AppUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", race.AppUserId);
            return View(race);
        }

        // POST: Race/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Image,AddressId,RaceCategory,AppUserId")] Race race)
        {
            if (id != race.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(race);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceExists(race.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", race.AddressId);
            ViewData["AppUserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", race.AppUserId);
            return View(race);
        }

        // GET: Race/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _raceRepository.GetById(id.Value);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        // POST: Race/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var race = await _context.Races.FindAsync(id);
            if (race != null)
            {
                _context.Races.Remove(race);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceExists(int id)
        {
            return _context.Races.Any(e => e.Id == id);
        }
    }
}
