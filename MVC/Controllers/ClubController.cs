using Microsoft.AspNetCore.Mvc;
using MVC.Data;

namespace MVC.Controllers
{
    public class ClubController : Controller
    {
        private readonly AppDbContext _context;

        public ClubController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ClubController
        public ActionResult Index()
        {
            var clubs = _context.Clubs.ToList();
            return View(clubs);
        }

    }
}
