using GigHub.Models;
using GigHub.ViewModels;
using System.Collections.Generic;

namespace GigHub.Repositories
{
    public interface IAttendanceRepository
    {
        void GetAttendance(Gig gig, GigsDetailsViewModel viewModel, string userId);
        IEnumerable<Attendance> GetFutureAttendances(string userId);
    }
}