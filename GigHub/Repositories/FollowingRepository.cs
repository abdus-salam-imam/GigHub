using GigHub.Models;
using GigHub.ViewModels;
using System.Linq;

namespace GigHub.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GetFollowing(Gig gig, GigsDetailsViewModel viewModel, string userId)
        {
            viewModel.IsFollowing = _context.Followings
                .Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId);
        }

    }
}