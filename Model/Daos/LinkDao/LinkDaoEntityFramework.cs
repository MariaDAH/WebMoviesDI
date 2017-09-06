using System.Data.Objects;
using System.Linq;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao
{
    class LinkDaoEntityFramework
        : GenericDaoEntityFramework<Link, long>, ILinkDao
    {

        public LinkDaoEntityFramework() { }
        
        #region ILinkDao Members;

        public ListBlock<Link> ListForUser(long userId, int startIndex, int count)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = (from l in links
                          where l.UserProfile.userId == userId
                          orderby l.date descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Link>("userId", userId);
                }
                else
                {
                    throw new NoMoreItemsException<Link>("userId", userId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore) {
                result.RemoveAt(count);
            }

            return new ListBlock<Link>(result.ToList(), startIndex, hasMore);
        }

        public ListBlock<Link> ListForUserRated(long userId, int startIndex, int count)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = (from l in links
                          where l.UserProfile.userId == userId
                          orderby l.Ratings.Sum(r => (int?)r.value) ?? 0 descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Link>("userId", userId);
                }
                else
                {
                    throw new NoMoreItemsException<Link>("userId", userId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Link>(result.ToList(), startIndex, hasMore);
        }

        public ListBlock<Link> ListForUserReported(long userId, int threshold, int startIndex, int count)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = (from l in links
                          where (l.UserProfile.userId == userId) && (l.Ratings.Sum(r => r.value) <= threshold) && ((l.reportRead != true) || (l.reportRead == null))
                          orderby l.Ratings.Sum(r => r.value) descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Link>("userId", userId);
                }
                else
                {
                    throw new NoMoreItemsException<Link>("userId", userId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Link>(result.ToList(), startIndex, hasMore);
        }

        public ListBlock<Link> ListForMovie(long movieId, int startIndex, int count)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = (from l in links
                          where l.movieId == movieId
                          orderby l.date descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Link>("movieId", movieId);
                }
                else
                {
                    throw new NoMoreItemsException<Link>("movieId", movieId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Link>(result, startIndex, hasMore);
        }
        
        public ListBlock<Link> ListForMovieRated(long movieId, int startIndex, int count)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = (from l in links.Include("Ratings")
                          where l.movieId == movieId
                          orderby l.Ratings.Sum(r => (int?)r.value) ?? 0 descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Link>("movieId", movieId);
                }
                else
                {
                    throw new NoMoreItemsException<Link>("movieId", movieId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Link>(result, startIndex, hasMore);
        }

        public ListBlock<Link> ListForLabel(string label, int startIndex, int count)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = (from l in links
                          where l.Labels.Any(b => b.text == label)
                          orderby l.date descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Link>("label", label);
                }
                else
                {
                    throw new NoMoreItemsException<Link>("label", label);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Link>(result.ToList(), startIndex, hasMore);
        }

        public ListBlock<Link> ListForLabelRated(string label, int startIndex, int count)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = (from l in links
                          where l.Labels.Any(b => b.text == label)
                          orderby l.Ratings.Sum(r => (int?)r.value) ?? 0 descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Link>("label", label);
                }
                else
                {
                    throw new NoMoreItemsException<Link>("label", label);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Link>(result.ToList(), startIndex, hasMore);
        }

        public ListBlock<Link> ListForUserAndMovie(long userId, long movieId, int startIndex, int count)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = (from l in links
                          where (l.UserProfile.userId == userId) && (l.movieId == movieId)
                          orderby l.date descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Link>("userId", userId, "movieId", movieId);
                }
                else
                {
                    throw new NoMoreItemsException<Link>("userId", userId, "movieId", movieId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Link>(result, startIndex, hasMore);
        }
        
        public ListBlock<Link> ListForLabel(long labelId, int startIndex, int count)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = (from l in links
                          where l.Labels.Any(b => b.labelId == labelId)
                          orderby l.date descending
                          select l).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Link>("labelId", labelId);
                }
                else
                {
                    throw new NoMoreItemsException<Link>("labelId", labelId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Link>(result, startIndex, hasMore);
        }
        
        public bool ExistsForMovieAndName(long movieId, string name)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = from l in links
                         where (l.movieId == movieId) && (l.name == name)
                         select l;

            return result.Count() != 0;
        }
        
        public bool ExistsForMovieAndUrl(long movieId, string url)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = from l in links
                         where (l.movieId == movieId) && (l.url == url)
                         select l;

            return result.Count() != 0;
        }
        
        public bool ExistsForMovieAndLink(long movieId, long linkId)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = from l in links
                         where ((l.movieId == movieId) && (l.linkId == linkId))
                         select l;

            return result.Count() != 0;
        }
        
        public bool ExistsForUserAndLink(long userId, long linkId)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = from l in links
                         where (l.UserProfile.userId == userId) && (l.linkId == linkId)
                         select l;

            return result.Count() != 0;
        }

        public int CountForMovie(long movieId)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = from l in links
                         where (l.movieId == movieId)
                         select l;

            return result.Count();
        }

        public int CountForUser(long userId)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = from l in links
                         where (l.userId == userId)
                         select l;

            return result.Count();
        }

        public int CountForUserReported(long userId, int threshold)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = from l in links
                         where (l.userId == userId) && (l.Ratings.Sum(r => r.value) <= threshold) && ((l.reportRead != true) || (l.reportRead == null))
                         select l;

            return result.Count();
        }

        public int CountForLabel(string label)
        {
            ObjectSet<Link> links = Context.CreateObjectSet<Link>();

            var result = from l in links
                         where l.Labels.Any(b => b.text == label)
                         select l;

            return result.Count();
        }

        #endregion

    }
}
