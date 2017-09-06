using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.CommentDao
{
    class CommentDaoEntityFramework
        : GenericDaoEntityFramework<Comment, long>, ICommentDao
    {

        public CommentDaoEntityFramework() { }

        #region ICommentDao Members

        public ListBlock<Comment> ListForLink(long linkId, int startIndex, int count)
        {
            ObjectSet<Comment> comments = Context.CreateObjectSet<Comment>();

            List<Comment> result = (from c in comments
                                    where c.linkId == linkId
                                    orderby c.date descending
                                    select c).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Comment>("linkId", linkId);
                }
                else
                {
                    throw new NoMoreItemsException<Comment>("linkId", linkId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Comment>(result, startIndex, hasMore);
        }

        public ListBlock<Comment> ListForUser(long userId, int startIndex, int count)
        {
            ObjectSet<Comment> comments = Context.CreateObjectSet<Comment>();

            List<Comment> result = (from c in comments
                                    where c.userId == userId
                                    orderby c.date descending
                                    select c).Skip(startIndex).Take(count + 1).ToList();

            if (result.Count == 0)
            {
                if (startIndex == 0)
                {
                    throw new InstanceNotFoundException<Comment>("userId", userId);
                }
                else
                {
                    throw new NoMoreItemsException<Comment>("userId", userId);
                }
            }

            bool hasMore = (result.Count == count + 1);
            if (hasMore)
            {
                result.RemoveAt(count);
            }

            return new ListBlock<Comment>(result, startIndex, hasMore);
        }

        public int CountForLink(long linkId)
        {
            ObjectSet<Comment> comments = Context.CreateObjectSet<Comment>();

            var result = from c in comments
                         where c.linkId == linkId
                         select c;

            return result.Count();
        }

        public int CountForUser(long userId)
        {
            ObjectSet<Comment> comments = Context.CreateObjectSet<Comment>();

            var result = from c in comments
                         where c.userId == userId
                         select c;

            return result.Count();
        }

        public Comment FindForUserAndComment(long userId, long commentId)
        {
            ObjectSet<Comment> comments = Context.CreateObjectSet<Comment>();

            var result = from c in comments.Include("UserProfile")
                         where (c.UserProfile.userId == userId) && (c.commentId == commentId)
                         orderby c.date descending
                         select c;

            if (result.Count() == 0)
            {
                throw new InstanceNotFoundException<Comment>("userId", userId, "commentId", commentId);
            }
            else if (result.Count() > 1)
            {
                throw new DuplicateInstanceException<Comment>("userId", userId, "commentId", commentId);
            }

            return result.First<Comment>();
        }

        #endregion

    }
}
