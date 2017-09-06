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
using Es.Udc.DotNet.WebMovies.Web.Http.Util;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Comment
{
    public partial class RemoveComment
        : SpecificCulturePage
    {

        private long MovieId { get; set; }

        private long CommentId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            CommentId = Int64.Parse(Request.Params.Get("commentId"));

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ICommentService commentService = container.Resolve<ICommentService>();
            ILinkService linkService = container.Resolve<ILinkService>();

            CommentDetails comment = commentService.GetComment(CommentId);
            LinkDetails link = linkService.GetLink(comment.LinkId);

            MovieId = link.MovieId;

            lblLink.Text = link.Name;
            lnkLink.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/Link.aspx?linkId=" + link.LinkId);

            lblAuthor.Text = comment.AuthorName;
            lnkAuthor.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Comment/ListComments.aspx?userId=" + comment.AuthorId);
            lblDate.Text = comment.Date.ToString();
            lblText.Text = comment.Text;
        }

        public void Remove(object sender, EventArgs e)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ICommentService commentService = container.Resolve<ICommentService>();

            UserSession userSession = (UserSession)this.Context.Session["userSession"];
            long userId = userSession.UserProfileId;

            try
            {
                commentService.RemoveComment(userId, CommentId);
            }
            catch (UserNotAuthorizedException<CommentDetails>)
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
