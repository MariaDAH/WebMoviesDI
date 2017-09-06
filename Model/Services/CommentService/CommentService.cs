using System;
using System.Collections.Generic;
using Es.Udc.DotNet.WebMovies.Model.Daos.CommentDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;

namespace Es.Udc.DotNet.WebMovies.Model.Services.CommentService
{
    public class CommentService
        : ICommentService
    {

        [Dependency]
        public IUserProfileDao UserDao { private get; set; }

        [Dependency]
        public ILinkDao LinkDao { private get; set; }

        [Dependency]
        public ICommentDao CommentDao { private get; set; }

        #region ICommentService Members

        public CommentDetails GetComment(long commentId)
        {
            Comment comment;
            try
            {
                comment = CommentDao.Find(commentId);
            }
            catch (InstanceNotFoundException<Comment> ex)
            {
                throw new InstanceNotFoundException<CommentDetails>(ex.Properties);
            }

            UserProfile user;
            try
            {
                user = UserDao.Find(comment.userId);
            }
            catch (InstanceNotFoundException<UserProfile> ex)
            {
                throw new InternalErrorException(ex);
            }

            return new CommentDetails(comment.commentId, comment.text, comment.userId, user.userLogin, comment.linkId, comment.date);
        }

        public ListBlock<CommentDetails> GetCommentsForLink(long linkId, int startIndex, int count)
        {
            if (!LinkDao.Exists(linkId))
            {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            ListBlock<Comment> comments;
            try
            {
                comments = CommentDao.ListForLink(linkId, startIndex, count);
            }
            catch (InstanceNotFoundException<Comment>)
            {
                return new ListBlock<CommentDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Comment>)
            {
                return new ListBlock<CommentDetails>(startIndex, false);
            }

            List<CommentDetails> details = new List<CommentDetails>();
            foreach (Comment comment in comments)
            {
                UserProfile user;
                try
                {
                    user = UserDao.Find(comment.userId);
                }
                catch (InstanceNotFoundException<UserProfile> ex)
                {
                    throw new InternalErrorException(ex);
                }

                details.Add(new CommentDetails(comment.commentId, comment.text, comment.userId, user.userLogin, comment.linkId, comment.date));
            }

            return new ListBlock<CommentDetails>(details, comments.Index, comments.HasMore);
        }

        public ListBlock<CommentDetails> GetCommentsForUser(long userId, int startIndex, int count)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            ListBlock<Comment> comments;
            try
            {
                comments = CommentDao.ListForUser(userId, startIndex, count);
            }
            catch (InstanceNotFoundException<Comment>)
            {
                return new ListBlock<CommentDetails>(startIndex, false);
            }
            catch (NoMoreItemsException<Comment>)
            {
                return new ListBlock<CommentDetails>(startIndex, false);
            }

            List<CommentDetails> details = new List<CommentDetails>();
            foreach (Comment comment in comments)
            {
                UserProfile user;
                try
                {
                    user = UserDao.Find(comment.userId);
                }
                catch (InstanceNotFoundException<UserProfile> ex)
                {
                    throw new InternalErrorException(ex);
                }

                details.Add(new CommentDetails(comment.commentId, comment.text, comment.userId, user.userLogin, comment.linkId, comment.date));
            }

            return new ListBlock<CommentDetails>(details, comments.Index, comments.HasMore);
        }

        public int CountCommentsForLink(long linkId)
        {
            if (!LinkDao.Exists(linkId))
            {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            return CommentDao.CountForLink(linkId);
        }

        public int CountCommentsForUser(long userId)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            return CommentDao.CountForUser(userId);
        }

        public long AddComment(long userId, long linkId, string text)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }
            if (!LinkDao.Exists(linkId))
            {
                throw new InstanceNotFoundException<LinkDetails>("linkId", linkId);
            }

            Comment comment = Comment.CreateComment(-1, userId, linkId, text, DateTime.Now);
            try
            {
                CommentDao.Create(comment);
            }
            catch (DuplicateInstanceException<Comment> ex)
            {
                throw new InternalErrorException(ex);
            }

            return comment.commentId;
        }
        
        public void UpdateComment(long userId, long commentId, string text)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            Comment comment;
            try
            {
                comment = CommentDao.Find(commentId);
            }
            catch (InstanceNotFoundException<Comment> ex)
            {
                throw new InstanceNotFoundException<CommentDetails>(ex.Properties);
            }

            if (comment.UserProfile.userId != userId)
            {
                throw new UserNotAuthorizedException<CommentDetails>(userId, "commentId", commentId);
            }

            comment.text = text;
            // comment.editDate = DateTime.Now; // This is not included in this iteration. Kept for future purposes

            try
            {
                CommentDao.Update(comment);
            }
            catch (InstanceNotFoundException<Comment> ex)
            {
                throw new InternalErrorException(ex);
            }
        }
        
        public void RemoveComment(long userId, long commentId)
        {
            if (!UserDao.Exists(userId))
            {
                throw new InstanceNotFoundException<UserProfileDetails>("userId", userId);
            }

            Comment comment;
            try
            {
                comment = CommentDao.Find(commentId);
            }
            catch (InstanceNotFoundException<Comment> ex)
            {
                throw new InstanceNotFoundException<CommentDetails>(ex.Properties);
            }

            if ((comment.UserProfile.userId != userId) && (comment.Link.UserProfile.userId != userId))
            {
                throw new UserNotAuthorizedException<CommentDetails>(userId, commentId);
            }

            try
            {
                CommentDao.Remove(comment.commentId);
            }
            catch (InstanceNotFoundException<Comment> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        #endregion

    }
}
