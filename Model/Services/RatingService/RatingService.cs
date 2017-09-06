using System;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.RatingDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;

namespace Es.Udc.DotNet.WebMovies.Model.Services.RatingService
{
    public class RatingService
        : IRatingService
    {

        [Dependency]
        public IUserProfileDao UserProfileDao { private get; set; }

        [Dependency]
        public ILinkDao LinkDao { private get; set; }

        [Dependency]
        public IRatingDao RatingDao { private get; set; }
        
        #region IRatingService Members

        public int GetRating(long userId, long linkId)
        {
            if (!UserProfileDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            if (!LinkDao.Exists(linkId))
            {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            try
            {
                return RatingDao.FindForUserAndLink(userId, linkId).value;
            }
            catch (InstanceNotFoundException<Rating>)
            {
                return 0;
            }
            catch (DuplicateInstanceException<Rating> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        public long Rate(long userId, long linkId, int value)
        {
            if (!UserProfileDao.Exists(userId)) {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            Link link;
            try
            {
                link = LinkDao.Find(linkId);
            }
            catch (InstanceNotFoundException<Link> ex)
            {
                throw new InstanceNotFoundException<LinkDetails>(ex.Properties);
            }

            if (link.UserProfile.userId == userId)
            {
                throw new UserNotAuthorizedException<RatingDetails>(userId, "linkId", linkId);
            }

            if (!RatingDao.ExistsForUserAndLink(userId, linkId))
            {
                if (value == 0)
                {
                    return -1;
                }

                Rating rating = Rating.CreateRating(-1, userId, linkId, value, DateTime.Now);
                try
                {
                    RatingDao.Create(rating);
                }
                catch (DuplicateInstanceException<Rating> ex)
                {
                    throw new InternalErrorException(ex);
                }

                return rating.ratingId;
            }
            else
            {
                Rating rating;
                try
                {
                    rating = RatingDao.FindForUserAndLink(userId, linkId);
                }
                catch (InstanceNotFoundException<Rating> ex)
                {
                    throw new InternalErrorException(ex);
                }
                catch (DuplicateInstanceException<Rating> ex)
                {
                    throw new InternalErrorException(ex);
                }

                if (value == 0)
                {
                    try
                    {
                        RatingDao.Remove(rating.ratingId);
                    }
                    catch (InstanceNotFoundException<Rating> ex)
                    {
                        throw new InternalErrorException(ex);
                    }
                    return -1;
                }

                rating.value = value;
                try
                {
                    RatingDao.Update(rating);
                }
                catch (InstanceNotFoundException<Rating> ex)
                {
                    throw new InternalErrorException(ex);
                }

                return rating.ratingId;
            }
        }

        #endregion

    }
}
