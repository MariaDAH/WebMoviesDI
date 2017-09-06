using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.WebMovies.Model.Daos.FavoriteDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;

namespace Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService
{
    public interface IFavoriteService
    {

        IUserProfileDao UserProfileDao { set; }

        ILinkDao LinkDao { set; }

        IFavoriteDao FavoriteDao { set; }

        /// <summary>
        /// Determines whether the user has a link in favorites or not.
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="linkId">ID of the link</param>
        /// <returns>The presence of the link in favorite list</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        [Transactional]
        bool HasInFavorites(long userId, long linkId);

        /// <summary>
        /// Gets the info for a favorite link.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="linkId">ID of the link.</param>
        /// <returns>Details of the favorite.</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;FavoriteDetails&gt;"/>
        FavoriteDetails GetFavorite(long userId, long linkId);

        /// <summary>
        /// Lists the favorite links for an user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="startIndex">Index of the first value to return.</param>
        /// <param name="count">Number of values to return.</param>
        /// <returns>Returns a list of favorite links for that user limited to the parameters given.</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        [Transactional]
        ListBlock<FavoriteDetails> GetFavoritesForUser(long userId, int startIndex, int count);

        /// <summary>
        /// Lists the favorite links for an user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="startIndex">Index of the first value to return.</param>
        /// <param name="count">Number of values to return.</param>
        /// <returns>Returns a list of favorite links for that user limited to the parameters given.</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        [Transactional]
        ListBlock<FavoriteDetails> GetFavoritesForUserByRating(long userId, int startIndex, int count);

        /// <summary>
        /// Counts the favorite links for an user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="startIndex">Index of the first value to return.</param>
        /// <param name="count">Number of values to return.</param>
        /// <returns>Returns the count of favorite links for that user limited to the parameters given.</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        [Transactional]
        int CountFavoritesForUser(long userId);

        /// <summary>
        /// Sets a link as favorite for an user
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="linkId">ID of the link.</param>
        /// <param name="name">Name of the favorite to show in the user page.</param>
        /// <param name="description">Description of the favorite to show in the user page.</param>
        /// <param name="movieTitle">Title of the movie referred by the link.</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        /// <exception cref="DuplicateInstanceException&lt;FavoriteDetails&gt;"/>
        [Transactional]
        long AddToFavorites(long userId, long linkId, string name, string description);
        
        /// <summary>
        /// Updates the data for a favorite link
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="favoriteId">ID of the link.</param>
        /// <param name="name">Name of the favorite to show in the user page.</param>
        /// <param name="description">Description of the favorite to show in the user page.</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;FavoriteDetails&gt;"/>
        [Transactional]
        void UpdateFavorite(long userId, long linkId, string name, string description);

        /// <summary>
        /// Removes the set as a favorite for a link
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="favoriteId">ID of the link.</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;LinkDetails&gt;"/>
        /// <exception cref="InstanceNotFoundException&lt;FavoriteDetails&gt;"/>
        [Transactional]
        void RemoveFromFavorites(long userId, long linkId);

    }
}
