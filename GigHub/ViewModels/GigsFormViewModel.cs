using GigHub.Models;
using System.Collections.Generic;

namespace GigHub.viewModels
{
    public class GigsFormViewModel
    {

        public int Id { get; set; }

        public string Venue { get; set; }
        public string Date { get; set; }

        public string Time { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public Genre GenreId { get; set; }

    }
}