using System;
using System.Xml;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.WebMovies.Model.Daos.MovieProxy;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;

namespace Es.Udc.DotNet.WebMovies.Model.Services.MovieService
{
    public interface IMovieService
    {

        IMovieProxy MovieProxy { set; }

        /// <summary>
        /// Finds the details of a movie
        /// </summary>
        /// <param name="movieId">ID of the movie to be found</param>
        /// <returns>Details of the movie</returns>
        /// <exception cref="InstanceNotFoundException&lt;MovieDetails&gt;"/>
        [Transactional]
        MovieDetails GetMovie(long movieId);

        /// <summary>
        /// Finds the details of a movie in an XML.
        /// </summary>
        /// <param name="movieId">ID of the movie to be found</param>
        /// <returns>An XML document containing the details of the movie, containing an error tag if no movie is found</returns>
        [Transactional]
        XmlDocument GetMovieXml(long movieId);

        /// <summary>
        /// Searches for movies which contain all of the given keywords in their names
        /// </summary>
        /// <param name="keywords">Keywords sepparated by blank spaces, all of which may be found in the movies to be found</param>
        /// <param name="startIndex">Index of the first item to be returned</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>An list block of details of the movies found matching the criteria</returns>
        [Transactional]
        ListBlock<MovieDetails> SearchMovies(String keywords, int startIndex, int count);

        /// <summary>
        /// Searches for movies which contain all of the given keywords in their names
        /// </summary>
        /// <param name="keywords">Keywords sepparated by blank spaces, all of which may be found in the movies to be found</param>
        /// <param name="startIndex">Index of the first item to be returned</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>An XML document that defines the result of the search, containing an error tag if no movie is found</returns>
        [Transactional]
        XmlDocument SearchMoviesXml(String keywords, int startIndex, int count);

    }
}
