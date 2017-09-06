using System;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.MovieService;
using System.Web;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Movie
{
    public partial class Movie
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            long movieId = Int64.Parse(Request.Params.Get("movieId"));

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            IMovieService movieService = container.Resolve<IMovieService>();

            MovieDetails movie = null;
            try
            {
                movie = movieService.GetMovie(movieId);
            }
            catch (InstanceNotFoundException<MovieDetails>)
            {
                lblTitle.Text = (string)GetGlobalResourceObject("Common", "error");

                Uri referrerUrl = Request.UrlReferrer;
                if (referrerUrl != null)
                {
                    lnkReturn.NavigateUrl = Response.ApplyAppPathModifier(referrerUrl.AbsoluteUri);
                }

                pMovie.Visible = false;
                pError.Visible = true;
                return;
            }

            lblTitle.Text = MovieTitle(movieId, movie.Title);
            lblPrice.Text = movie.Price.ToString("C");
            lblDescription.Text = movie.Description;
            lblLinkCount.Text = movie.LinkCount.ToString();
            lnkPurchase.NavigateUrl = Response.ApplyAppPathModifier(Settings.Default.WebShop_purchaseUrl + movieId);
            lnkAddLink.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Link/AddLink.aspx?movieId=" + movieId);
            lnkViewLinks.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Link/ListLinks.aspx?movieId=" + movieId);
        }

        public string MovieTitle(long movieId, string title)
        {
            return ApplicationManager.CacheMovieTitle(movieId, title);
        }

    }
}
