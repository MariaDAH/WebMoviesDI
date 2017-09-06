using System;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Favorite
{
    public partial class RemoveFavorite
        : SpecificCulturePage
    {

        private long LinkId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkId = Int64.Parse(Request.Params.Get("linkId"));

            if (!IsPostBack)
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                IFavoriteService favoriteService = container.Resolve<IFavoriteService>();
                ILinkService linkService = container.Resolve<ILinkService>();

                LinkDetails link = linkService.GetLink(LinkId);
                FavoriteDetails favorite = favoriteService.GetFavorite(userId, LinkId);

                lblLink.Text = "'" + link.Name + "'";
                lblName.Text = favorite.Name;
                lblDescription.Text = favorite.Description;

                if (Request.UrlReferrer != null)
                {
                    lnkReturn.NavigateUrl = Response.ApplyAppPathModifier(Request.UrlReferrer.AbsoluteUri);
                }
                else
                {
                    lnkReturn.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Favorite/ListFavorites.aspx");
                }
            }
        }

        public void Remove(object sender, EventArgs e)
        {
            try
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                IFavoriteService favoriteService = container.Resolve<IFavoriteService>();

                favoriteService.RemoveFromFavorites(userId, LinkId);

                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Favorite/ListFavorites.aspx"));
            }
            catch (InstanceNotFoundException<FavoriteDetails>)
            {
                lclError.Text = (string)GetLocalResourceObject("lclNotInFavorites.Text");
                pRemoveFavorite.Visible = false;
                pError.Visible = true;
            }
            catch (UserNotAuthorizedException<FavoriteDetails>)
            {
                lclError.Text = (string)GetLocalResourceObject("lclNotAuthorized.Text");
                pRemoveFavorite.Visible = false;
                pError.Visible = true;
            }
        }

    }
}
