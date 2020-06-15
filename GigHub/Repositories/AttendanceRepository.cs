using GigHub.Models;
using GigHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return _context.Attendances
           .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
           .ToList();
        }

        public void GetAttendance(Gig gig, GigsDetailsViewModel viewModel, string userId)
        {
            viewModel.IsAttending = _context.Attendances
                .Any(a => a.GigId == gig.Id && a.AttendeeId == userId);
        }


    }
}