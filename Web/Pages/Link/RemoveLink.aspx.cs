using System;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Util;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Es.Udc.DotNet.WebMovies.Model.Services.LabelService;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Link
{
    public partial class RemoveLink
        : SpecificCulturePage
    {

        private long MovieId;

        private long LinkId;

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkId = Int64.Parse(Request.Params.Get("linkId"));

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();

            LinkDetails link = linkService.GetLink(LinkId);

            MovieId = link.MovieId;

            if (!IsPostBack)
            {
                lblLinkName.Text = link.Name;
                lnkLinkName.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/Link.aspx?linkId=" + LinkId);

                if (Request.UrlReferrer != null)
                {
                    lnkReturn.NavigateUrl = Response.ApplyAppPathModifier(Request.UrlReferrer.AbsoluteUri);
                }
                else
                {
                    lnkReturn.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/Link.aspx?linkId=" + LinkId);
                }

                if (link.Rating <= Settings.Default.WebMovies_demotedThreshold)
                {
                    pRemoveLink.CssClass += " demoted";
                }
                else if (link.Rating >= Settings.Default.WebMovies_promotedThreshold)
                {
                    pRemoveLink.CssClass += " promoted";
                }
            }
        }

        public void Remove(object sender, EventArgs e)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();
            ILabelService labelService = container.Resolve<ILabelService>();

            UserSession userSession = (UserSession)this.Context.Session["userSession"];
            long userId = userSession.UserProfileId;

            try
            {
                labelService.RemoveLabelsForLink(userId, LinkId);
                linkService.RemoveLink(userId, LinkId);
            }
            catch (UserNotAuthorizedException<LinkDetails>)
            {
                pError.Visible = true;
            }

            if (CookiesManager.GetPreferredSearchEngine(Context) == "webshop")
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Movie/Movie.aspx?movieId=" + MovieId));
            }
            else
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Movie/MovieXml.aspx?movieId=" + MovieId));
            }
        }

    }
}
