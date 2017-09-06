using System;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Favorite
{
    public partial class AddFavorite
        : SpecificCulturePage
    {

        public long LinkId { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkId = Int64.Parse(Request.Params.Get("linkId"));

            if (!IsPostBack)
            {
                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                ILinkService linkService = container.Resolve<ILinkService>();

                LinkDetails link = linkService.GetLink(LinkId);

                lclAdd.Text += " " + (string)GetLocalResourceObject("lblLink.Text");
                lnkAddWhat.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/Link.aspx?linkId=" + LinkId);
                lblAddWhat.Text = "'" + link.Name + "'";
                imgAddWhat.ImageUrl = (string)GetLocalResourceObject("imgLink.ImageUrl");
                imgAddWhat.AlternateText = (string)GetLocalResourceObject("imgLink.AlternateText");
            }
        }

        protected void btnAddFavorite_Click(object sender, EventArgs e)
        {
            if (Page.IsValid) 
            {
                try
                {
                    IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                    ILinkService linkService = container.Resolve<ILinkService>();
                    IFavoriteService favoriteService = container.Resolve<IFavoriteService>();

                    UserSession userSession = (UserSession)this.Context.Session["userSession"];
                    long userId = userSession.UserProfileId;

                    LinkDetails link = linkService.GetLink(LinkId);

                    favoriteService.AddToFavorites(userId, LinkId, txtName.Text, txtDescription.Text);

                    Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Favorite/ListFavorites.aspx"));
                }
                catch (DuplicateInstanceException<FavoriteDetails>)
                {
                    pError.Visible = true;
                    return;
                }
            }
        }

    }
}
