using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.RatingDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;

namespace Es.Udc.DotNet.WebMovies.Model.Services.RatingService
{
    public interface IRatingService
    {

        IUserProfileDao UserProfileDao { set; }

        ILinkDao LinkDao { set; }

        IRatingDao RatingDao { set; }

        /// <summary>
        /// Gets the value rated for a link by an user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name= "linkId">Link ID</param>
        /// <returns>Value</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;" />
        [Transactional]
        int GetRating(long userId, long linkId);

        /// <summary>
        /// Adds a rating entry
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name= "linkId">Link ID</param>
        /// <param name="value">Value</param>
        /// <returns>Rating ID</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;" />
        /// <exception cref="UserNotAuthorizedException&lt;RatingDetails&gt;" />
        [Transactional]
        long Rate(long userId, long linkId, int value);

    }
}
