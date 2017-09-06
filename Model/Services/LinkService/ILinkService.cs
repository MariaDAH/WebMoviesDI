using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Daos.RatingDao;

namespace Es.Udc.DotNet.WebMovies.Model.Services.LinkService
{
    public interface ILinkService
    {

        IUserProfileDao UserDao { set; }

        ILinkDao LinkDao { set; }

        IRatingDao RatingDao { set; }

        /// <summary>
        /// Finds the details of a link.
        /// </summary>
        /// <param name="linkId">Id of the link to be found.</param>
        /// <returns>Link details</returns>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        [Transactional]
        LinkDetails GetLink(long linkId);
        
        /// <summary>
        /// Lists movie Links (ordered by date)
        /// </summary>
        /// <param name="movieId">Id of the movie.</param>
        /// <param name="startIndex">Start Index</param>
        /// <param name="count"> Count</param>
        /// <returns>LinkDetails List</returns>
        [Transactional]
        ListBlock<LinkDetails> GetLinksForMovie(long movieId, int startIndex, int count);
        
        /// <summary>
        /// Lists movie Links (ordered by rating)
        /// </summary>
        /// <param name="movieId">Id of the movie.</param>
        /// <param name="startIndex">Start Index</param>
        /// <param name="count"> Count</param>
        /// <returns>LinkDetails List</returns>
        [Transactional]
        ListBlock<LinkDetails> GetMostValuedLinksForMovie(long movieId, int startIndex, int count);

        /// <summary>
        /// Counts the number of links for a given movie.
        /// </summary>
        /// <param name="linkId">ID of the movie.</param>
        /// <returns>The number of links for a movie.</returns>
        [Transactional]
        int CountLinksForMovie(long movieId);

        /// <summary>
        /// Lists movie Links (ordered by date)
        /// </summary>
        /// <param name="movieId">Id of the movie.</param>
        /// <param name="startIndex">Start Index</param>
        /// <param name="count"> Count</param>
        /// <returns>LinkDetails List</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        [Transactional]
        ListBlock<LinkDetails> GetLinksForUser(long userId, int startIndex, int count);

        /// <summary>
        /// Lists user Links (ordered by rating)
        /// </summary>
        /// <param name="movieId">Id of the user.</param>
        /// <param name="startIndex">Start Index</param>
        /// <param name="count"> Count</param>
        /// <returns>LinkDetails List</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        [Transactional]
        ListBlock<LinkDetails> GetMostValuedLinksForUser(long userId, int startIndex, int count);

        /// <summary>
        /// Counts the number of links for a given user.
        /// </summary>
        /// <param name="linkId">ID of the user.</param>
        /// <returns>The number of links for a user.</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        [Transactional]
        int CountLinksForUser(long userId);

        /// <summary>
        /// Lists movie links (ordered by rating) that are under the indicated threshold and not maked as read
        /// </summary>
        /// <param name="userId">Id of the movie.</param>
        /// <param name="threshold">Threshold.</param>
        /// <param name="startIndex">Start Index</param>
        /// <param name="count">Count</param>
        /// <returns>LinkDetails List</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        [Transactional]
        ListBlock<LinkDetails> GetReportedLinksForUser(long userId, int threshold, int startIndex, int count);

        /// <summary>
        /// Counts the number of reported and not read links for a given user.
        /// </summary>
        /// <param name="linkId">ID of the user.</param>
        /// <param name="threshold">Threshold.</param>
        /// <returns>The number of reported links for a user.</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        [Transactional]
        int CountReportedLinksForUser(long userId, int threshold);

        /// <summary>
        /// Sets a reported link as read
        /// </summary>
        /// <param name="movieId">Id of the user.</param>
        /// <param name="movieId">Id of the link.</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;" />
        /// <exception cref="UserNotAuthorized&lt;LinkDetails&gt;" />
        [Transactional]
        void SetReportedLinkAsRead(long userId, long linkId);

        /// <summary>
        /// Lists movie Links (ordered by date)
        /// </summary>
        /// <param name="movieId">Id of the movie.</param>
        /// <param name="startIndex">Start Index</param>
        /// <param name="count"> Count</param>
        /// <returns>LinkDetails List</returns>
        [Transactional]
        ListBlock<LinkDetails> GetLinksForLabel(string label, int startIndex, int count);

        /// <summary>
        /// Lists movie Links (ordered by rating)
        /// </summary>
        /// <param name="movieId">Id of the movie.</param>
        /// <param name="startIndex">Start Index</param>
        /// <param name="count"> Count</param>
        /// <returns>LinkDetails List</returns>
        [Transactional]
        ListBlock<LinkDetails> GetMostValuedLinksForLabel(string label, int startIndex, int count);

        /// <summary>
        /// Counts the number of links for a given movie.
        /// </summary>
        /// <param name="linkId">ID of the movie.</param>
        /// <returns>The number of links for a movie.</returns>
        [Transactional]
        int CountLinksForLabel(string label);

        /// <summary>
        /// Adds a new link for a movie.
        /// </summary>
        /// <param name="usrId">User adding the link.</param>
        /// <param name="movieId">Movie whose link is being added.</param>
        /// <param name="name">Name given to the link.</param>
        /// <param name="description">Description of the link.</param>
        /// <param name="url">URL for the link.</param>
        /// <returns>linkId</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="DuplicateInstanceException&lt;LinkDetails&gt;" />
        [Transactional]
        long AddLink(long userId, long movieId, string name, string description, string url);
        
        /// <summary>
        /// Updates an existent link for a movie.
        /// </summary>
        /// <param name="linkId">ID of the link to be added.</param>
        /// <param name="name">Name given to the link.</param>
        /// <param name="description">Description of the link.</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;" />
        /// <exception cref="DuplicateInstanceException&lt;LinkDetails&gt;" />
        /// <exception cref="UserNotAuthorizedException&lt;LinkDetails&gt;" />
        [Transactional]
        void UpdateLink(long userId, long linkId, string name, string description);
        
        /// <summary>
        /// Removes an existent link of an userId.
        /// </summary>
        /// <param name="linkId">ID of the link to be removed.</param>
        /// <param name="name">ID of the user.</param>
        /// <param name="startIndex">Start Index</param>
        /// <param name="count"> Count</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;" />
        /// <exception cref="UserNotAuthorizedException&lt;LinkDetails&gt;" />
        [Transactional]
        void RemoveLink(long userId, long linkId);
        
    }
}
