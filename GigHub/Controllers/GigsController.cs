using GigHub.Models;
using GigHub.viewModels;
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


        
        public ActionResult New()
        {


            var genres = _context.Genres.ToList();

            var viewModel = new GigsFormViewModel
            {
                Genres=genres


            };


            return View(viewModel);
        }




    }
}