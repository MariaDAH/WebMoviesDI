using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.WebMovies.Model.Daos.CommentDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;

namespace Es.Udc.DotNet.WebMovies.Model.Services.CommentService
{
    public interface ICommentService
    {

        IUserProfileDao UserDao { set; }

        ILinkDao LinkDao { set; }

        ICommentDao CommentDao { set; }

        /// <summary>
        /// Get the comments of a link
        /// </summary>
        /// <param name="linkId">Id of the link.</param>
        /// <returns>List of comments</returns>
        /// <exception cref="InstanceNotFoundException&lt;CommentDetails&gt;"/>
        [Transactional]
        CommentDetails GetComment(long commentId);

        /// <summary>
        /// Get the comments of a link
        /// </summary>
        /// <param name="linkId">Id of the link.</param>
        /// <param name="startIndex">Index of the first item to return.</param>
        /// <param name="count">Number of results to return.</param>
        /// <returns>List of comments</returns>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        [Transactional]
        ListBlock<CommentDetails> GetCommentsForLink(long linkId, int startIndex, int count);

        /// <summary>
        /// Get the comments of a link
        /// </summary>
        /// <param name="linkId">Id of the link.</param>
        /// <param name="startIndex">Index of the first item to return.</param>
        /// <param name="count">Number of results to return.</param>
        /// <returns>List of comments</returns>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        [Transactional]
        ListBlock<CommentDetails> GetCommentsForUser(long userId, int startIndex, int count);

        /// <summary>
        /// Counts the comments of a link
        /// </summary>
        /// <param name="linkId">Id of the link.</param>
        /// <returns>Number of comments</returns>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        [Transactional]
        int CountCommentsForLink(long linkId);

        /// <summary>
        /// Counts the comments of an user
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <returns>Number of comments</returns>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        [Transactional]
        int CountCommentsForUser(long userId);

        /// <summary>
        /// An user add a comment of a link
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name= "linkId">Link id</param>
        /// <param name="text"> Text of the Comment</param>
        /// <returns>Comment id</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        [Transactional]
        long AddComment(long userId, long linkId, string text);
        
        /// <summary>
        /// An user update on a comment about a link
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name= "linkId">Comment id</param>
        /// <param name="text"> Text of the Comment</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;CommentDetails&gt;"/>
        /// <exception cref="UserNotAuthorizedException&lt;CommentDetails&gt;"/>
        [Transactional]
        void UpdateComment(long userId, long commentId, string text);
        
        /// <summary>
        /// Deletion of a comment by its author
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name= "linkId">Comment id</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;CommentDetails&gt;"/>
        /// <exception cref="UserNotAuthorizedException&lt;CommentDetails&gt;"/>
        [Transactional]
        void RemoveComment(long userId, long commentId);

    }
}
