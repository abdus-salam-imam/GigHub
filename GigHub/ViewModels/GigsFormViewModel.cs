﻿using GigHub.Controllers;
using GigHub.Models;
using GigHub.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

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


        public string  Heading { get; set; }


        public string  Action 
        {
            get
            {
                Expression<Func<GigsController, ActionResult>> update = (c => c.Update(this));

                Expression<Func<GigsController, ActionResult>> create = (c => c.Create(this));

                var action = (Id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            } 
        
        
        
        }

    }
}