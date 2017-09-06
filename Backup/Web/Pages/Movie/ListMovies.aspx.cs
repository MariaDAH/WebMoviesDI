using System;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.MovieService;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Movie
{
    public partial class ListMovies
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string keywordsUrl = Request.Params.Get("keywords");
            string keywords = HttpUtility.UrlDecode(keywordsUrl);

            int startIndex = 0;
            try
            {
                startIndex = Int32.Parse(Request.Params.Get("startIndex"));
            }
            catch (ArgumentNullException) { }

            int count = Settings.Default.WebMovies_linksPerPage;

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            IMovieService movieService = container.Resolve<IMovieService>();

            lclSearch.Text = GetLocalResourceObject("lclSearch.Text") + ": \"" + keywords + "\"";

            ListBlock<MovieDetails> movies = movieService.SearchMovies(keywords, startIndex, count);

            lvListMovies.DataSource = movies;
            lvListMovies.DataBind();

            int previousIndex = 0;
            if (count <= startIndex)
            {
                previousIndex += startIndex - count;
            }
            lnkPrevious.NavigateUrl = Response.ApplyAppPathModifier("ListMovies.aspx?keywords=" + keywordsUrl + "&startIndex=" + previousIndex);
            lnkPrevious.Visible = startIndex > 0;

            lnkNext.NavigateUrl = Response.ApplyAppPathModifier("ListMovies.aspx?keywords=" + keywordsUrl + "&startIndex=" + (startIndex + count));
            lnkNext.Visible = movies.HasMore;
        }

        public string Currency(double value)
        {
            return value.ToString("C");
        }

        public string ViewLinksUrl(long movieId)
        {
            return "/Pages/Link/ListLinks.aspx?movieId=" + movieId;
        }

        public string AddLinkUrl(long movieId)
        {
            return "/Pages/Link/AddLink.aspx?movieId=" + movieId;
        }

        public string MovieTitle(long movieId, string title)
        {
            return ApplicationManager.CacheMovieTitle(movieId, title);
        }

    }
}
