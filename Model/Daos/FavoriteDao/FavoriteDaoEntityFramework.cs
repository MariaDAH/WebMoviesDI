using System.Data.Objects;
using System.Linq;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.FavoriteDao
{
    class FavoriteDaoEntityFramework
        : GenericDaoEntityFramework<Favorite, long>, IFavoriteDao
    {

        public FavoriteDaoEntityFramework() { }

        #region IFavoriteDao Members;

        public ListBlock<Favorite> ListForUser(long userId, int startIndex, int count)
        {
            ObjectSet<Favorite> favorites = Context.CreateObjectSet<Favorite>();

            var result = (from f in favorites
                          where f.UserProfile.userId == userId
                          orderby f.date descending
                          select f).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Favorite>("userId", userId);
                }
                else
                {
                    throw new NoMoreItemsException<Favorite>("userId", userId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Favorite>(result, startIndex, hasMore);
        }

        public ListBlock<Favorite> ListForUserRated(long userId, int startIndex, int count)
        {
            ObjectSet<Favorite> favorites = Context.CreateObjectSet<Favorite>();

            var result = (from f in favorites.Include("Link.Ratings")
                          where f.UserProfile.userId == userId
                          orderby f.Link.Ratings.Sum(r => (int?)r.value) ?? 0 descending
                          select f).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Favorite>("userId", userId);
                }
                else
                {
                    throw new NoMoreItemsException<Favorite>("userId", userId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Favorite>(result, startIndex, hasMore);
        }

        public Favorite FindForUserAndLink(long userId, long linkId)
        {
            ObjectSet<Favorite> favorites = Context.CreateObjectSet<Favorite>();

            var result = from f in favorites
                         where (f.Link.linkId == linkId) && (f.UserProfile.userId == userId)
                         select f;

            if (result.Count() == 0)
            {
                throw new InstanceNotFoundException<Favorite>("userId", userId, "linkId", linkId);
            }
            else if (result.Count() > 1)
            {
                throw new DuplicateInstanceException<Favorite>("userId", userId, "linkId", linkId);
            }

            return result.First();
        }

        public bool ExistsForUserAndLink(long userId, long linkId)
        {
            ObjectSet<Favorite> favorites = Context.CreateObjectSet<Favorite>();

            var result = from f in favorites
                         where (f.Link.linkId == linkId) && (f.UserProfile.userId == userId)
                         select f;

            return result.Count() > 0;
        }

        public int CountForUser(long userId)
        {
            ObjectSet<Favorite> favorites = Context.CreateObjectSet<Favorite>();

            var result = from f in favorites
                         where f.UserProfile.userId == userId
                         select f;

            return result.Count();
        }

        #endregion

    }
}
