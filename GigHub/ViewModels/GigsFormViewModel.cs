using GigHub.Models;
using GigHub.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GigHub.viewModels
{
    public class GigsFormViewModel
    {

        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }
        
        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [FutureTime]
        public string Time { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        [Required]
        [Display(Name ="Genre")]
        public byte GenreId { get; set; }

        public DateTime GetDateTime()
        {

            return DateTime.Parse(string.Format("{0} {1}", Date, Time));

        }



    }
}