using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using System.Data.Objects.DataClasses;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.RatingDao
{
    class RatingDaoEntityFramework
        : GenericDaoEntityFramework<Rating, long>, IRatingDao
    {

        public RatingDaoEntityFramework() { }
        
        #region IRatingDao Members;

        public ListBlock<Rating> ListForLink(long linkId, int startIndex, int count)
        {
            ObjectSet<Rating> ratings = Context.CreateObjectSet<Rating>();

            var result = (from r in ratings
                          where r.Link.linkId == linkId
                          orderby r.date descending
                          select r).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Rating>("linkId", linkId);
                }
                else
                {
                    throw new NoMoreItemsException<Rating>("linkId", linkId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Rating>(result, startIndex, hasMore);
        }

        public Rating FindForUserAndLink(long userId, long linkId)
        {
            ObjectSet<Rating> ratings = Context.CreateObjectSet<Rating>();

            var result = from r in ratings
                         where (r.Link.linkId == linkId) && (r.UserProfile.userId == userId)
                         select r;

            if (result.Count() == 0)
            {
                throw new InstanceNotFoundException<Rating>("userId", userId, "linkId", linkId);
            }
            else if (result.Count() > 1)
            {
                throw new DuplicateInstanceException<Rating>("userId", userId, "linkId", linkId);
            }

            return result.First();
        }
        
        public bool ExistsForUserAndLink(long userId, long linkId)
        {
            ObjectSet<Rating> ratings = Context.CreateObjectSet<Rating>();

            var result = from r in ratings
                         where (r.Link.linkId == linkId) && (r.UserProfile.userId == userId)
                         select r;

            return result.Count() != 0;
        }

        public int CalculateValueForLink(long linkId)
        {
            ObjectSet<Rating> ratings = Context.CreateObjectSet<Rating>();

            var result = from r in ratings
                          where r.Link.linkId == linkId
                          select r;

            if (result.Count() == 0) { return 0; }
            return result.Sum(r => r.value);
        }

        public int CalculateValueForLabel(string label)
        {
            //ObjectSet<Rating> ratings = Context.CreateObjectSet<Rating>();

            //var result = from r in ratings
            //              from b in r.Link.Labels
            //              where b.text == label
            //              select r;

            //if (result.Count() == 0) { return 0; }
            //return result.Sum(r => r.value) + result.Count();

            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            var result = from b in labels
                         where b.text == label
                         from l in b.Links
                         select l;

            return result.Count();
        }

        public int CalculateValueForUser(long userId)
        {
            Context.ContextOptions.LazyLoadingEnabled = true;
            ObjectSet<UserProfile> users = Context.CreateObjectSet<UserProfile>();
            users.MergeOption = MergeOption.PreserveChanges;

            var result = (from u in users
                          where u.userId == userId
                          from l in u.Links
                          select l.Ratings).ToList();

            int a = result.Count();
            foreach (EntityCollection<Rating> ratings in result)
            {
                a += ratings.Sum(r => r.value);
            }

            return a;
        }

        #endregion

    }
}
