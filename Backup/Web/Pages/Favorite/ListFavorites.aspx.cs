using System;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService;
using Es.Udc.DotNet.WebMovies.Model.Util;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using System.Data;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Favorite
{
    public partial class ListFavorites
        : SpecificCulturePage
    {

        private ObjectDataSource lvListFavoritesDataSource = new ObjectDataSource();

        protected void Page_Load(object sender, EventArgs e)
        {
            string orderedBy = Request.Params.Get("orderedBy");

            UserSession userSession = (UserSession)this.Context.Session["userSession"];
            long userId = userSession.UserProfileId;

            Type favoriteServiceType = typeof(IFavoriteService);

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            IUserService userService = container.Resolve<IUserService>();

            if (orderedBy == "rating")
            {
                lvListFavoritesDataSource.SelectMethod = favoriteServiceType.GetMethod("GetFavoritesForUser").Name;
            }
            else
            {
                lvListFavoritesDataSource.SelectMethod = favoriteServiceType.GetMethod("GetFavoritesForUserByRating").Name;
            }
            lvListFavoritesDataSource.SelectCountMethod = favoriteServiceType.GetMethod("CountFavoritesForUser").Name;
            lvListFavoritesDataSource.SelectParameters.Add("userId", DbType.Int64, userId.ToString());

            lvListFavoritesDataSource.ObjectCreating += this.LvListFavoritesDataSource_ObjectCreating;
            lvListFavoritesDataSource.TypeName = favoriteServiceType.FullName;
            lvListFavoritesDataSource.EnablePaging = true;
            lvListFavoritesDataSource.StartRowIndexParameterName = "startIndex";
            lvListFavoritesDataSource.MaximumRowsParameterName = "count";

            dpListFavorites.PageSize = Settings.Default.WebMovies_linksPerPage;

            lvListFavorites.DataSource = lvListFavoritesDataSource;
            lvListFavorites.DataBind();

            if (!IsPostBack)
            {
                string reorderUrl = Request.Url.AbsolutePath + "?";
                if (orderedBy == "rating")
                {
                    reorderUrl += "&orderedBy=date";
                    imgByWhat.ImageUrl = (string)GetLocalResourceObject("imgByDate.ImageUrl");
                    imgByWhat.AlternateText = (string)GetLocalResourceObject("imgByDate.AlternateText");
                }
                else
                {
                    reorderUrl += "&orderedBy=rating";
                    imgByWhat.ImageUrl = (string)GetLocalResourceObject("imgByRating.ImageUrl");
                    imgByWhat.AlternateText = (string)GetLocalResourceObject("imgByRating.AlternateText");
                }
                lnkOrderBy.NavigateUrl = reorderUrl;

                HyperLink lnkReturn = lvListFavorites.Controls[0].FindControl("lnkReturn") as HyperLink;
                if (lnkReturn != null)
                {
                    if (Request.UrlReferrer != null)
                    {
                        lnkReturn.NavigateUrl = Response.ApplyAppPathModifier(Request.UrlReferrer.AbsoluteUri);
                    }
                }
            }
        }

        public void LvListFavoritesDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            IFavoriteService favoriteService = new FavoriteService();
            favoriteService = (IFavoriteService)container.BuildUp(favoriteService.GetType(), favoriteService);

            e.ObjectInstance = favoriteService;
        }

        protected void LvListFavorites_PreRender(object sender, EventArgs e)
        {
            lvListFavorites.DataSource = lvListFavoritesDataSource;
            lvListFavorites.DataBind();
        }

        public string LinkName(long linkId)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();

            LinkDetails link = linkService.GetLink(linkId);

            return link.Name;
        }

        public long MovieId(long linkId)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();

            LinkDetails link = linkService.GetLink(linkId);

            return link.MovieId;
        }

        public string MovieTitle(long linkId)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();

            LinkDetails link = linkService.GetLink(linkId);

            return ApplicationManager.GetMovieTitle(link.MovieId);
        }

        public string Css(long linkId)
        {
            string css = "favorite";

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();

            LinkDetails link = linkService.GetLink(linkId);

            if (link.Rating <= Settings.Default.WebMovies_demotedThreshold)
            {
                css += " demoted";
            }
            else if (link.Rating >= Settings.Default.WebMovies_promotedThreshold)
            {
                css += " promoted";
            }

            return css;
        }

    }
}
