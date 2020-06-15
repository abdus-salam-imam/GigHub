using GigHub.Models;
using GigHub.Persistence;
using GigHub.viewModels;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {

        
        private readonly IUnitOfWork _unitOfWork;
        

        public GigsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public ViewResult Mine()
        {
            var userId = User.Identity.GetUserId();
         
            var gigs = _unitOfWork.Gigs.GetUpComingGigsByArtist(userId);

            return View(gigs);
        }

        




        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();





            var viewModel = new GigsViewModel
            {

                UpcomingGigs = _unitOfWork.Gigs.GetGigsUserAttending(userId),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs i am Attending ",
                Attendances =_unitOfWork.Attendances.GetFutureAttendances(userId).ToLookup(a => a.GigId)

            };

            return View("Gigs", viewModel);
        }

        

        

        public ActionResult Details(int id)
        {
            Gig gig = _unitOfWork.Gigs.GetGig(id);

            if (gig == null)

                return HttpNotFound();

            var viewModel = new GigsDetailsViewModel
            {
                Gig = gig
            };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                
                _unitOfWork.Attendances.GetAttendance(gig, viewModel, userId);

                _unitOfWork.Followings.GetFollowing(gig, viewModel, userId);

            }



            return View("Details", viewModel);



        }

        


        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var gig =_unitOfWork.Gigs.GetGig(id);

            if (gig == null)
                return HttpNotFound();
            if (gig.ArtistId != userId)
                return new HttpUnauthorizedResult();

            var viewModel = new GigsFormViewModel
            {
                Genres = _unitOfWork.Genres.GetGenres(),
                Id = gig.Id,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Venue = gig.Venue,
                GenreId = gig.GenreId,
                Heading = "Edit a Gig"


            };


            return View("GigForm", viewModel);
        }

        

        [Authorize]
        public ActionResult Create()
        {
            var  genres =_unitOfWork.Genres.GetGenres();

            var viewModel = new GigsFormViewModel
            {
                Genres = genres,
                Heading = "Create a Gig"

            };


            return View("GigForm", viewModel);
        }

        

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigsFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                viewModel.Genres = _unitOfWork.Genres.GetGenres();
                
                return View("GigForm", viewModel);


                  }

            
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                Venue = viewModel.Venue,
                GenreId = viewModel.GenreId


            };


            _unitOfWork.Gigs.Add(gig);


            _unitOfWork.Complete();


            return RedirectToAction("Mine","Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigsFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                viewModel.Genres = _unitOfWork.Genres.GetGenres();

                return View("GigForm", viewModel);


            }


            var gig = _unitOfWork.Gigs.GetGigWithAttendees(viewModel.Id);

            if (gig == null)
                return HttpNotFound();

            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult(); 


            gig.Modify(viewModel.GetDateTime(),viewModel.Venue,viewModel.GenreId);


            _unitOfWork.Complete();


            return RedirectToAction("Mine", "Gigs");
        }







    }
}