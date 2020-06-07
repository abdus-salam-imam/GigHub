using GigHub.Models;
using GigHub.viewModels;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
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

        
        [HttpPost]
        public ActionResult Search(GigsViewModel gigsViewModel)
        {

            return RedirectToAction("Index", "Home", new { query = gigsViewModel.SearchTerm });

        }



        
        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now && !g.IsCanceled)
                .Include(g=>g.Genre)
                .ToList();

            return View(gigs);
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
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);


            var viewModel = new GigsFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Id=gig.Id,
                Date =  gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Venue = gig.Venue,
                GenreId = gig.GenreId,
                Heading = "Edit a Gig"


            };


            return View("GigForm",viewModel);
        }






        [Authorize]
        public ActionResult Create()
        {


            var genres = _context.Genres.ToList();

            var viewModel = new GigsFormViewModel
            {
                Genres=genres,
                Heading = "Create a Gig"

            };


            return View("GigForm",viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigsFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                viewModel.Genres = _context.Genres.ToList();
                
                return View("GigForm", viewModel);


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


            return RedirectToAction("Mine","Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigsFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                viewModel.Genres = _context.Genres.ToList();

                return View("GigForm", viewModel);


            }

            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs

                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            gig.Modify(viewModel.GetDateTime(),viewModel.Venue,viewModel.GenreId);
            
     
            _context.SaveChanges();


            return RedirectToAction("Mine", "Gigs");
        }







    }
}