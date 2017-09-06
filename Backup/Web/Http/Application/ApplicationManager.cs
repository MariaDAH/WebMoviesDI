using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.MovieService;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Web.Http.Application
{
    public class ApplicationManager
    {

        public static readonly String MOVIE_TITLES_ATTRIBUTE = "movieTitles";

        private static IMovieService movieService;

        public IMovieService MovieService
        {
            set { movieService = value; }
        }

        static ApplicationManager()
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            movieService = container.Resolve<IMovieService>();

            HttpContext.Current.Application.Add(MOVIE_TITLES_ATTRIBUTE, new Dictionary<long, string>());
        }

        public static string GetMovieTitle(long movieId)
        {
            IDictionary<long, string> movieTitles = (IDictionary<long, string>)HttpContext.Current.Application[MOVIE_TITLES_ATTRIBUTE];

            string title = null;

            try
            {
                title = movieTitles[movieId];
            }
            catch
            {
                title = movieService.GetMovie(movieId).Title;

                if (title != null)
                {
                    movieTitles.Add(movieId, title);
                }
            }

            return title;
        }

        public static void SetMovieTitle(long movieId, string movieTitle)
        {
            IDictionary<long, string> movieTitles = (IDictionary<long, string>)HttpContext.Current.Application[MOVIE_TITLES_ATTRIBUTE];

            if (movieTitle != null)
            {
                movieTitles.Add(movieId, movieTitle);
            }
        }

        public static string CacheMovieTitle(long movieId, string movieTitle)
        {
            IDictionary<long, string> movieTitles = (IDictionary<long, string>)HttpContext.Current.Application[MOVIE_TITLES_ATTRIBUTE];

            if ((!movieTitles.ContainsKey(movieId)) && (movieTitle != null))
            {
                movieTitles.Add(movieId, movieTitle);
            }

            return movieTitle;
        }

    }
}