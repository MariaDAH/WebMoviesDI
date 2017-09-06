using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.CommentService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;
using Es.Udc.DotNet.WebMovies.Web.Http.Util;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Comment
{
    public partial class AddComment
        : SpecificCulturePage
    {

        private long LinkId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkId = Int64.Parse(Request.Params.Get("linkId"));

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();

            LinkDetails link = linkService.GetLink(LinkId);

            if (!IsPostBack)
            {
                lblLink.Text = link.Name;
                lnkLink.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Link/Link.aspx?linkId=" + LinkId);
                lblMovie.Text = ApplicationManager.GetMovieTitle(link.MovieId);
                if (CookiesManager.GetPreferredSearchEngine(Context) == "webshop")
                {
                    lnkMovie.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Movie/Movie.aspx?movieId=" + link.MovieId);
                }
                else
                {
                    lnkMovie.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Movie/MovieXml.aspx?movieId=" + link.MovieId);
                }
            }
        }

        public void BtnAddCommentClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                ICommentService commentService = container.Resolve<ICommentService>();

                UserSession userSession = (UserSession)this.Context.Session["userSession"];

                commentService.AddComment(userSession.UserProfileId, LinkId, txtText.Text);

                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Comment/ListComments.aspx?linkId=" + LinkId));
            }
        }

    }
}
