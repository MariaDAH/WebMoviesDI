using System;
using System.Collections.Generic;
using Es.Udc.DotNet.WebMovies.Model.Daos.FavoriteDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;

namespace Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService
{
    public class FavoriteService
        : IFavoriteService
    {

        [Dependency]
        public IUserProfileDao UserProfileDao { private get; set; }

        [Dependency]
        public ILinkDao LinkDao { private get; set; }

        [Dependency]
        public IFavoriteDao FavoriteDao { private get; set; }

        #region IFavoriteService Members

        public bool HasInFavorites(long userId, long linkId)
        {
            if (!UserProfileDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            if (!LinkDao.Exists(linkId))
            {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            return FavoriteDao.ExistsForUserAndLink(userId, linkId);
        }

        public FavoriteDetails GetFavorite(long userId, long linkId)
        {
            if (!UserProfileDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            if (!LinkDao.Exists(linkId))
            {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            Favorite favorite;
            try
            {
                favorite = FavoriteDao.FindForUserAndLink(userId, linkId);
            }
            catch (InstanceNotFoundException<Favorite> ex)
            {
                throw new InstanceNotFoundException<FavoriteDetails>(ex.Properties);
            }
            catch (DuplicateInstanceException<Favorite> ex)
            {
                throw new InternalErrorException(ex);
            }

            return new FavoriteDetails(favorite.favoriteId, favorite.linkId, favorite.name, favorite.description, favorite.date);
        }

        public ListBlock<FavoriteDetails> GetFavoritesForUser(long userId, int startIndex, int count)
        {
            if (!UserProfileDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            ListBlock<Favorite> favorites;
            try
            {
                favorites = FavoriteDao.ListForUser(userId, startIndex, count);
            }
            catch (InstanceNotFoundException<Favorite>)
            {
                return new ListBlock<FavoriteDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Favorite>)
            {
                return new ListBlock<FavoriteDetails>(startIndex, false);
            }

            List<FavoriteDetails> details = new List<FavoriteDetails>();
            foreach (Favorite favorite in favorites)
            {
                details.Add(new FavoriteDetails(favorite.favoriteId, favorite.linkId, favorite.name, favorite.description, favorite.date));
            }

            return new ListBlock<FavoriteDetails>(details, favorites.Index, favorites.HasMore);
        }

        public ListBlock<FavoriteDetails> GetFavoritesForUserByRating(long userId, int startIndex, int count)
        {
            if (!UserProfileDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            ListBlock<Favorite> favorites;
            try
            {
                favorites = FavoriteDao.ListForUserRated(userId, startIndex, count);
            }
            catch (InstanceNotFoundException<Favorite>)
            {
                return new ListBlock<FavoriteDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Favorite>)
            {
                return new ListBlock<FavoriteDetails>(startIndex, false);
            }

            List<FavoriteDetails> details = new List<FavoriteDetails>();
            foreach (Favorite favorite in favorites)
            {
                details.Add(new FavoriteDetails(favorite.favoriteId, favorite.linkId, favorite.name, favorite.description, favorite.date));
            }

            return new ListBlock<FavoriteDetails>(details, favorites.Index, favorites.HasMore);
        }

        public int CountFavoritesForUser(long userId)
        {
            if (!UserProfileDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            return FavoriteDao.CountForUser(userId);
        }

        public long AddToFavorites(long userId, long linkId, string name, string description)
        {
            if (!UserProfileDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }
            if (!LinkDao.Exists(linkId))
            {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            if (FavoriteDao.ExistsForUserAndLink(userId, linkId))
            {
                throw new DuplicateInstanceException<FavoriteDetails>("userId", userId, "linkId", linkId);
            }

            Favorite favorite = Favorite.CreateFavorite(-1, userId, linkId, name, description, DateTime.Now);
            try
            {
                FavoriteDao.Create(favorite);
            }
            catch (DuplicateInstanceException<Favorite> ex)
            {
                throw new InternalErrorException(ex);
            }

            return favorite.favoriteId;
        }

        public void UpdateFavorite(long userId, long linkId, string name, string description)
        {
            if (!UserProfileDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            if (!LinkDao.Exists(linkId))
            {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            Favorite favorite;
            try
            {
                favorite = FavoriteDao.FindForUserAndLink(userId, linkId);
            }
            catch (InstanceNotFoundException<Favorite> ex)
            {
                throw new InstanceNotFoundException<FavoriteDetails>(ex.Properties);
            }
            catch (DuplicateInstanceException<Favorite> ex)
            {
                throw new InternalErrorException(ex);
            }

            favorite.name = name;
            favorite.description = description;

            try
            {
                FavoriteDao.Update(favorite);
            }
            catch (InstanceNotFoundException<Favorite> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        public void RemoveFromFavorites(long userId, long linkId)
        {
            if (!UserProfileDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            if (!LinkDao.Exists(linkId))
            {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            Favorite favorite;
            try
            {
                favorite = FavoriteDao.FindForUserAndLink(userId, linkId);
            }
            catch (InstanceNotFoundException<Favorite> ex)
            {
                throw new InstanceNotFoundException<FavoriteDetails>(ex.Properties);
            }
            catch (DuplicateInstanceException<Favorite> ex)
            {
                throw new InternalErrorException(ex);
            }

            try
            {
                FavoriteDao.Remove(favorite.favoriteId);
            }
            catch (InstanceNotFoundException<Favorite> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        #endregion

    }
}
