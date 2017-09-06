using System;
using System.Collections.Generic;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.RatingDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;

namespace Es.Udc.DotNet.WebMovies.Model.Services.LinkService
{
    public class LinkService
        : ILinkService
    {

        [Dependency]
        public IUserProfileDao UserDao { private get; set; }

        [Dependency]
        public ILinkDao LinkDao { private get; set; }

        [Dependency]
        public IRatingDao RatingDao { private get; set; }

        #region ILinkService Members

        public LinkDetails GetLink(long linkId)
        {
            Link link;
            try
            {
                link = LinkDao.Find(linkId);
            }
            catch (InstanceNotFoundException<Link> ex)
            {
                throw new InstanceNotFoundException<LinkDetails>(ex);
            }

            UserProfile user;
            try
            {
                user = UserDao.Find(link.userId);
            }
            catch (InstanceNotFoundException<UserProfile> ex)
            {
                throw new InternalErrorException(ex);
            }

            int rating = RatingDao.CalculateValueForLink(link.linkId);
        
            return new LinkDetails(link.linkId, link.userId, user.userLogin, link.movieId, link.name, link.description, link.url, rating, link.reportRead.GetValueOrDefault(), link.date);
        }

        public ListBlock<LinkDetails> GetLinksForMovie(long movieId, int startIndex, int count)
        {
            ListBlock<Link> links;
            try
            {
                links = LinkDao.ListForMovie(movieId, startIndex, count);
            }
            catch (InstanceNotFoundException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }

            List<LinkDetails> details = new List<LinkDetails>();
            foreach (Link link in links)
            {
                UserProfile user;
                try
                {
                    user = UserDao.Find(link.userId);
                }
                catch (InstanceNotFoundException<UserProfile> ex)
                {
                    throw new InternalErrorException(ex);
                }

                int rating = RatingDao.CalculateValueForLink(link.linkId);

                details.Add(new LinkDetails(link.linkId, link.userId, user.userLogin, movieId, link.name, link.description, link.url, rating, link.reportRead.GetValueOrDefault(), link.date)); 
            }

            return new ListBlock<LinkDetails>(details, links.Index, links.HasMore);
        }

        public ListBlock<LinkDetails> GetMostValuedLinksForMovie(long movieId, int startIndex, int count)
        {
            ListBlock<Link> links;
            try
            {
                links = LinkDao.ListForMovieRated(movieId, startIndex, count);
            }
            catch (InstanceNotFoundException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }

            List<LinkDetails> details = new List<LinkDetails>();
            foreach (Link link in links)
            {
                UserProfile user;
                try
                {
                    user = UserDao.Find(link.userId);
                }
                catch (InstanceNotFoundException<UserProfile> ex)
                {
                    throw new InternalErrorException(ex);
                }

                int rating = RatingDao.CalculateValueForLink(link.linkId);

                details.Add(new LinkDetails(link.linkId, user.userId, user.userLogin, movieId, link.name, link.description, link.url, rating, link.reportRead.GetValueOrDefault(), link.date));//Me cago en IS-> María
            }

            return new ListBlock<LinkDetails>(details, links.Index, links.HasMore);
        }

        public int CountLinksForMovie(long movieId)
        {
            return LinkDao.CountForMovie(movieId);
        }

        public ListBlock<LinkDetails> GetLinksForUser(long userId, int startIndex, int count)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            ListBlock<Link> links;
            try
            {
                links = LinkDao.ListForUser(userId, startIndex, count);
            }
            catch (InstanceNotFoundException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }

            List<LinkDetails> details = new List<LinkDetails>();
            foreach (Link link in links)
            {
                UserProfile user;
                try
                {
                    user = UserDao.Find(link.userId);
                }
                catch (InstanceNotFoundException<UserProfile> ex)
                {
                    throw new InternalErrorException(ex);
                }

                int rating = RatingDao.CalculateValueForLink(link.linkId);

                details.Add(new LinkDetails(link.linkId, link.userId, user.userLogin, link.movieId, link.name, link.description, link.url, rating, link.reportRead.GetValueOrDefault(), link.date));
            }

            return new ListBlock<LinkDetails>(details, links.Index, links.HasMore);
        }

        public ListBlock<LinkDetails> GetMostValuedLinksForUser(long userId, int startIndex, int count)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            ListBlock<Link> links;
            try
            {
                links = LinkDao.ListForUserRated(userId, startIndex, count);
            }
            catch (InstanceNotFoundException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }

            List<LinkDetails> details = new List<LinkDetails>();
            foreach (Link link in links)
            {
                UserProfile user;
                try
                {
                    user = UserDao.Find(link.userId);
                }
                catch (InstanceNotFoundException<UserProfile> ex)
                {
                    throw new InternalErrorException(ex);
                }

                int rating = RatingDao.CalculateValueForLink(link.linkId);

                details.Add(new LinkDetails(link.linkId, link.userId, user.userLogin, link.movieId, link.name, link.description, link.url, rating, link.reportRead.GetValueOrDefault(), link.date));
            }

            return new ListBlock<LinkDetails>(details, links.Index, links.HasMore);
        }

        public int CountLinksForUser(long userId)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            return LinkDao.CountForUser(userId);
        }

        public ListBlock<LinkDetails> GetReportedLinksForUser(long userId, int threshold, int startIndex, int count)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            ListBlock<Link> links;
            try
            {
                links = LinkDao.ListForUserReported(userId, threshold, startIndex, count);
            }
            catch (InstanceNotFoundException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }

            List<LinkDetails> details = new List<LinkDetails>();
            foreach (Link link in links)
            {
                UserProfile user;
                try
                {
                    user = UserDao.Find(link.userId);
                }
                catch (InstanceNotFoundException<UserProfile> ex)
                {
                    throw new InternalErrorException(ex);
                }

                int rating = RatingDao.CalculateValueForLink(link.linkId);

                details.Add(new LinkDetails(link.linkId, link.userId, user.userLogin, link.movieId, link.name, link.description, link.url, rating, link.reportRead.GetValueOrDefault(), link.date));
            }

            return new ListBlock<LinkDetails>(details, links.Index, links.HasMore);
        }

        public int CountReportedLinksForUser(long userId, int threshold)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            return LinkDao.CountForUserReported(userId, threshold);
        }

        public void SetReportedLinkAsRead(long userId, long linkId)
        {
            if (!UserDao.Exists(userId))
            {
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

            if (userId != link.UserProfile.userId)
            {
                throw new UserNotAuthorizedException<LinkDetails>(userId, "linkId", linkId); ;
            }

            link.reportRead = true;

            try
            {
                LinkDao.Update(link);
            }
            catch (InstanceNotFoundException<Link> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        public ListBlock<LinkDetails> GetLinksForLabel(string label, int startIndex, int count)
        {
            ListBlock<Link> links;
            try
            {
                links = LinkDao.ListForLabel(label, startIndex, count);
            }
            catch (InstanceNotFoundException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }

            List<LinkDetails> details = new List<LinkDetails>();
            foreach (Link link in links)
            {
                UserProfile user;
                try
                {
                    user = UserDao.Find(link.userId);
                }
                catch (InstanceNotFoundException<UserProfile> ex)
                {
                    throw new InternalErrorException(ex);
                }

                int rating = RatingDao.CalculateValueForLink(link.linkId);

                details.Add(new LinkDetails(link.linkId, link.userId, user.userLogin, link.movieId, link.name, link.description, link.url, rating, link.reportRead.GetValueOrDefault(), link.date));
            }

            return new ListBlock<LinkDetails>(details, links.Index, links.HasMore);
        }

        public ListBlock<LinkDetails> GetMostValuedLinksForLabel(string label, int startIndex, int count)
        {
            ListBlock<Link> links;
            try
            {
                links = LinkDao.ListForLabelRated(label, startIndex, count);
            }
            catch (InstanceNotFoundException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Link>)
            {
                return new ListBlock<LinkDetails>(startIndex, false);
            }

            List<LinkDetails> details = new List<LinkDetails>();
            foreach (Link link in links)
            {
                UserProfile user;
                try
                {
                    user = UserDao.Find(link.userId);
                }
                catch (InstanceNotFoundException<UserProfile> ex)
                {
                    throw new InternalErrorException(ex);
                }

                int rating = RatingDao.CalculateValueForLink(link.linkId);

                details.Add(new LinkDetails(link.linkId, link.userId, user.userLogin, link.movieId, link.name, link.description, link.url, rating, link.reportRead.GetValueOrDefault(), link.date));
            }

            return new ListBlock<LinkDetails>(details, links.Index, links.HasMore);
        }

        public int CountLinksForLabel(string label)
        {
            return LinkDao.CountForLabel(label);
        }

        public long AddLink(long userId, long movieId, string name, string description, string url)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            if (LinkDao.ExistsForMovieAndName(movieId, name))
            {
                throw new DuplicateInstanceException<LinkDetails>("movieId", movieId, "name", name);
            }
            if (LinkDao.ExistsForMovieAndUrl(movieId, url))
            {
                throw new DuplicateInstanceException<LinkDetails>("movieId", movieId, "url", url);
            }

            Link link = Link.CreateLink(-1, movieId, userId, name, description, url, DateTime.Now);
            link.reportRead = false;

            try
            {
                LinkDao.Create(link);
            }
            catch (DuplicateInstanceException<Link> ex)
            {
                throw new DuplicateInstanceException<LinkDetails>(ex.Properties);
            }

            return link.linkId;
        }

        public void UpdateLink(long userId, long linkId, string name, string description)
        {
            if (!UserDao.Exists(userId))
            {
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

            if ((link.name != name) && (LinkDao.ExistsForMovieAndName(link.movieId, name)))
            {
                throw new DuplicateInstanceException<LinkDetails>("movieId", link.movieId, "name", name);
            }

            if (userId != link.UserProfile.userId)
            {
                throw new UserNotAuthorizedException<LinkDetails>(userId, "linkId", linkId); ;
            }

            link.name = name;
            link.description = description;

            try
            {
                LinkDao.Update(link);
            }
            catch (InstanceNotFoundException<Link> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        public void RemoveLink(long userId, long linkId)
        {
            if (!UserDao.Exists(userId))
            {
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

            if (link.UserProfile.userId != userId)
            {
                throw new UserNotAuthorizedException<LinkDetails>(userId, "linkId", linkId);
            }

            try
            {
                LinkDao.Remove(linkId);
            }
            catch (InstanceNotFoundException<Link> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        #endregion

    }
}
