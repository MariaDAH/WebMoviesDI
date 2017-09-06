using System;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Favorite
{
    public partial class EditFavorite
        : SpecificCulturePage
    {

        public long LinkId { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkId = Int64.Parse(Request.Params.Get("linkId"));

            if (!IsPostBack)
            {
                try
                {
                    UserSession userSession = (UserSession)this.Context.Session["userSession"];
                    long userId = userSession.UserProfileId;

                    IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                    ILinkService linkService = container.Resolve<ILinkService>();
                    IFavoriteService favoriteService = container.Resolve<IFavoriteService>();

                    LinkDetails link = linkService.GetLink(LinkId);

                    lclEdit.Text += " " + (string)GetLocalResourceObject("lblLink.Text");
                    lnkEditWhat.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/Link.aspx?linkId=" + LinkId);
                    lblEditWhat.Text = "'" + link.Name + "'";
                    imgEditWhat.ImageUrl = (string)GetLocalResourceObject("imgLink.ImageUrl");
                    imgEditWhat.AlternateText = (string)GetLocalResourceObject("imgLink.AlternateText");

                    FavoriteDetails favorite = favoriteService.GetFavorite(userId, LinkId);

                    txtName.Text = favorite.Name;
                    txtDescription.Text = favorite.Description;
                }
                catch (InstanceNotFoundException<FavoriteDetails>)
                {
                    pFavorite.Visible = false;
                    pError.Visible = true;
                    return;
                }
            }
        }

        protected void btnEditFavorite_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                    IFavoriteService favoriteService = container.Resolve<IFavoriteService>();

                    UserSession userSession = (UserSession)this.Context.Session["userSession"];
                    long userId = userSession.UserProfileId;

                    favoriteService.UpdateFavorite(userId, LinkId, txtName.Text, txtDescription.Text);

                    Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Favorite/ListFavorites.aspx"));
                }
                catch (InstanceNotFoundException<FavoriteDetails>)
                {
                    pError.Visible = true;
                    return;
                }
            }
        }

    }
}
