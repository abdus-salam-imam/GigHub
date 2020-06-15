using GigHub.Models;
using GigHub.ViewModels;

namespace GigHub.Repositories
{
    public interface IFollowingRepository
    {
        void GetFollowing(Gig gig, GigsDetailsViewModel viewModel, string userId);
    }
}