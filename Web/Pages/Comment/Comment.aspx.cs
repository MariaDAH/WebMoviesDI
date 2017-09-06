using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Services.CommentService;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Comment
{
    public partial class Comment
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            long commentId = Int64.Parse(Request.Params.Get("commentId"));

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ICommentService commentService = container.Resolve<ICommentService>();
            ILinkService linkService = container.Resolve<ILinkService>();

            CommentDetails comment = commentService.GetComment(commentId);
            LinkDetails link = linkService.GetLink(comment.LinkId);

            lblLink.Text = link.Name;
            lnkLink.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/Link.aspx?linkId=" + link.LinkId);

            lblAuthor.Text = comment.AuthorName;
            lnkAuthor.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Comment/ListComments.aspx?userId=" + comment.AuthorId);
            lblDate.Text = comment.Date.ToString();
            lblText.Text = comment.Text;

            if (SessionManager.IsUserAuthenticated(Context))
            {
                long userId = SessionManager.GetUserSession(Context).UserProfileId;

                if ((userId == comment.AuthorId) || (userId == link.UserId))
                {
                    pControl.Visible = true;

                    lnkEditComment.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Comment/EditComment.aspx?commentId=" + commentId);
                    lnkEditComment.Visible = (userId == comment.AuthorId);
                    lnkRemoveComment.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Comment/RemoveComment.aspx?commentId=" + commentId);
                }
            }
        }

    }
}
