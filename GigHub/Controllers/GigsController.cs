using GigHub.Models;
using GigHub.viewModels;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {

        private readonly ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g=>g.Artist)
                .Include(g=>g.Genre)
                .ToList();

            var viewModel = new GigsViewModel
            {

                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs i am Attending "


            };

            return View("Gigs",viewModel);
        }










        [Authorize]
        
        public ActionResult Create()
        {


            var genres = _context.Genres.ToList();

            var viewModel = new GigsFormViewModel
            {
                Genres=genres


            };


            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigsFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                viewModel.Genres = _context.Genres.ToList();
                return View("Create", viewModel);


            }

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                Venue = viewModel.Venue,
                GenreId = viewModel.GenreId


            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();


            return RedirectToAction("index","Home");
        }





    }
}