using GigHub.Models;
using GigHub.viewModels;
using Microsoft.AspNet.Identity;
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