using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.CommentDao
{
    public interface ICommentDao
        : IGenericDao<Comment, long>
    {

        /// <summary>
        /// Finds the comments for a link
        /// </summary>
        /// <param name="linkId">Link id</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="count">Count</param>
        /// <returns>Show the comments for a link</returns>
        /// <exception cref="InstanceNotFoundException&lt;Comment&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Comment&gt;"/>
        ListBlock<Comment> ListForLink(long linkId, int startIndex, int count);

        /// <summary>
        /// Finds the comments for an user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="count">Count</param>
        /// <returns>Show the comments for an user</returns>
        /// <exception cref="InstanceNotFoundException&lt;Comment&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Comment&gt;"/>
        ListBlock<Comment> ListForUser(long userId, int startIndex, int count);

        /// <summary>
        /// Return the number of comments for a link
        /// </summary>
        /// <param name="linkId">Link id</param>
        /// <returns>Show the comments for a link</returns>
        int CountForLink(long linkId);

        /// <summary>
        /// Return the number of comments for an user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Show the comments for an user</returns>
        int CountForUser(long userId);

        /// <summary>
        /// Finds the user's comments for a link
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="commentId">Comment id</param>
        /// <returns>Show the comment  for an user</returns>
        /// <exception cref="InstanceNotFoundException&lt;Comment&gt;"/>
        /// <exception cref="DuplicateInstanceException&lt;Comment&gt;"/>
        Comment FindForUserAndComment(long userId, long commentId);

    }
}
