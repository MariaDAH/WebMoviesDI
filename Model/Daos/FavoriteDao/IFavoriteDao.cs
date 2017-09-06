using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.FavoriteDao
{
    public interface IFavoriteDao
        : IGenericDao<Favorite, long>
    {

        /// <summary>
        /// Finds the Favorite Links added by an user
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of user's favorite links</returns>
        /// <exception cref="InstanceNotFoundException&lt;Favorite&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Favorite&gt;"/>
        ListBlock<Favorite> ListForUser(long userId, int startIndex, int count);
        
        /// <summary>
        /// Finds the Favorite Links added by an user
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of user's favorite links</returns>
        /// <exception cref="InstanceNotFoundException&lt;Favorite&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Favorite&gt;"/>
        ListBlock<Favorite> ListForUserRated(long userId, int startIndex, int count);
        
        /// <summary>
        /// Finds the user's favorite links
        /// </summary>
        /// <param name="userId">linkId</param>
        /// <param name="linkId">movieId</param>
        /// <returns>Show an user's favorite link</returns>
        /// <exception cref="InstanceNotFoundException&lt;Favorite&gt;"/>
        /// <exception cref="DuplicateInstanceException&lt;Favorite&gt;"/>
        Favorite FindForUserAndLink(long userId, long linkId);

        /// <summary>
        /// Finds the user's favorite links
        /// </summary>
        /// <param name="userId">linkId</param>
        /// <param name="linkId">movieId</param>
        /// <returns>Show an user's favorite link</returns>
        bool ExistsForUserAndLink(long userId, long linkId);

        /// <summary>
        /// Counts the Favorite Links added by an user
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>The count of user's favorite links</returns>
        int CountForUser(long userId);

    }
}
