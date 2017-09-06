using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao
{
    public interface ILinkDao
        : IGenericDao<Link, long>
    {

        /// <summary>
        /// Finds the Links added by an user
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of links added by the user</returns>
        /// <exception cref="InstanceNotFoundException&lt;Link&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Link&gt;"/>
        ListBlock<Link> ListForUser(long userId, int startIndex, int count);

        /// <summary>
        /// Finds the Links added by an user
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of links added by the user</returns>
        /// <exception cref="InstanceNotFoundException&lt;Link&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Link&gt;"/>
        ListBlock<Link> ListForUserRated(long userId, int startIndex, int count);

        /// <summary>
        /// Finds the Links added by an user under a threshold and not read
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="threshold">threshold</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of links added by the user</returns>
        /// <exception cref="InstanceNotFoundException&lt;Link&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Link&gt;"/>
        ListBlock<Link> ListForUserReported(long userId, int threshold, int startIndex, int count);

        /// <summary>
        /// Finds the Links of a movie.
        /// </summary>
        /// <param name="linkId">movieId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of links for this movie</returns>
        /// <exception cref="InstanceNotFoundException&lt;Link&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Link&gt;"/>
        ListBlock<Link> ListForMovie(long movieId, int startIndex, int count);

        /// <summary>
        /// Finds the Links of a movie ordered by rating.
        /// </summary>
        /// <param name="linkId">movieId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of links for this movie</returns>
        /// <exception cref="InstanceNotFoundException&lt;Link&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Link&gt;"/>
        ListBlock<Link> ListForMovieRated(long movieId, int startIndex, int count);

        /// <summary>
        /// Finds the Links of a movie.
        /// </summary>
        /// <param name="linkId">movieId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of links for this movie</returns>
        /// <exception cref="InstanceNotFoundException&lt;Link&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Link&gt;"/>
        ListBlock<Link> ListForLabel(string label, int startIndex, int count);

        /// <summary>
        /// Finds the Links of a movie ordered by rating.
        /// </summary>
        /// <param name="linkId">movieId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of links for this movie</returns>
        /// <exception cref="InstanceNotFoundException&lt;Link&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Link&gt;"/>
        ListBlock<Link> ListForLabelRated(string label, int startIndex, int count);

        /// <summary>
        /// Finds the Links added by an user for a movie
        /// </summary>
        /// <param name="linkId">linkId</param>
        /// <param name="movieId">movieId</param>
        /// <returns>The List of links for this movie added by the user</returns>
        /// <exception cref="InstanceNotFoundException&lt;Link&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Link&gt;"/>
        ListBlock<Link> ListForUserAndMovie(long userId, long movieId, int startIndex, int count);
        
        /// <summary>
        /// Finds the Links joined to a label
        /// </summary>
        /// <param name="labelId">labelId</param>
        /// <param name="startIndex">startIndex</param>
        /// <param name="count">count</param>
        /// <returns>The List of links for a label</returns>
        /// <exception cref="InstanceNotFoundException&lt;Link&gt;"/>
        /// <exception cref="NoMoreItemsException&lt;Link&gt;"/>
        ListBlock<Link> ListForLabel(long labelId, int startIndex, int count);
        
        /// <summary>
        /// Checks if exists an entry with that name for a given movie.
        /// </summary>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="name">Name of the link.</param>
        /// <returns>The existence of an name for a movie already.</returns>
        bool ExistsForMovieAndName(long movieId, string name);
        
        /// <summary>
        /// Checks if exists an entry with that url for a given movie.
        /// </summary>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="url">URL of the link.</param>
        /// <returns>The existence of an URL for a movie already.</returns>
        bool ExistsForMovieAndUrl(long movieId, string url);
        
        /// <summary>
        /// Checks if exists an entry with that movie for a given link.
        /// </summary>
        /// <param name="movieId">ID of the movie.</param>
        /// <param name="link">Id of the link.</param>
        /// <returns>The existence of a link for a movie already.</returns>
        bool ExistsForMovieAndLink(long movieId, long linkId);
        
        /// <summary>
        /// Checks if exists an entry with that user for a given link.
        /// </summary>
        /// <param name="movieId">ID of the user.</param>
        /// <param name="link">Id of the link.</param>
        /// <returns>The existence of an user for a link already.</returns>
        bool ExistsForUserAndLink(long userId, long linkId);

        /// <summary>
        /// Counts the number of links for a given movie ID.
        /// </summary>
        /// <param name="movieId">ID of the movie.</param>
        /// <returns>The number of linka for the movie.</returns>
        int CountForMovie(long movieId);

        /// <summary>
        /// Counts the number of links for a given movie ID.
        /// </summary>
        /// <param name="movieId">ID of the movie.</param>
        /// <returns>The number of linka for the movie.</returns>
        int CountForUser(long userId);

        /// <summary>
        /// Counts the number of links for a given movie ID.
        /// </summary>
        /// <param name="movieId">ID of the movie.</param>
        /// <returns>The number of linka for the movie.</returns>
        int CountForUserReported(long userId, int threshold);

        /// <summary>
        /// Counts the number of links for a given movie ID.
        /// </summary>
        /// <param name="movieId">ID of the movie.</param>
        /// <returns>The number of linka for the movie.</returns>
        int CountForLabel(string label);

    }
}
