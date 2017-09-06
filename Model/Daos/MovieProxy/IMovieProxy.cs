using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.MovieProxy
{
    public interface IMovieProxy
    {

        /// <summary>
        /// Searches for movies that contain the given keywords in their titles.
        /// </summary>
        /// <param name="keywords">Keywords sepparated by blank spaces, all of which may be found in the movies to be found</param>
        /// <param name="startIndex">Index of the first item to be returned</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>A string containing the XML that defines the result of the search</returns>
        string FindMovies(string keywords, int startIndex, int count);

        /// <summary>
        /// Gets the details of a movie
        /// </summary>
        /// <param name="movieId">ID of the movie request</param>
        /// <returns>A string containing the XML that defines the result of the search</returns>
        string DetailMovie(long movieId);

    }
}
