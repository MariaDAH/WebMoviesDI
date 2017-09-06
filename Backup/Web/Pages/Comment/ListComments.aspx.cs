using System;
using System.Web;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.WebMovies.Model.Services.CommentService;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Comment
{
    public partial class ListComments
        : SpecificCulturePage
    {

        private ObjectDataSource lvListCommentsDataSource = new ObjectDataSource();

        protected void Page_Load(object sender, EventArgs e)
        {
            string userIdString = Request.Params.Get("userId");
            string linkIdString = Request.Params.Get("linkId");

            Type commentServiceType = typeof(ICommentService);

            if (((userIdString != null) && (linkIdString == null)) || ((userIdString == null) && (linkIdString == null)))
            {
                if (userIdString == null)
                {
                    UserSession userSession = (UserSession)this.Context.Session["userSession"];
                    userIdString = userSession.UserProfileId.ToString();
                }
                long userId = Int64.Parse(userIdString);

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                IUserService userService = container.Resolve<IUserService>();

                UserProfileDetails user = userService.GetUserProfile(userId);

                lvListCommentsDataSource.SelectMethod = commentServiceType.GetMethod("GetCommentsForUser").Name;
                lvListCommentsDataSource.SelectCountMethod = commentServiceType.GetMethod("CountCommentsForUser").Name;
                lvListCommentsDataSource.SelectParameters.Add("userId", userIdString);

                if (!IsPostBack)
                {
                    lnkForWhat.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/ListLinks.aspx?userId=" + userIdString);
                    imgForWhat.ImageUrl = (string)GetLocalResourceObject("imgAuthor.ImageUrl");
                    imgForWhat.AlternateText = (string)GetLocalResourceObject("imgAuthor.AlternateText");
                    lclFor.Text += " " + (string)GetLocalResourceObject("lblAuthor.Text");
                    lblForWhat.Text = user.LoginName;
                }
            }
            else if ((userIdString == null) && (linkIdString != null))
            {
                long linkId = Int64.Parse(linkIdString);

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                ILinkService linkService = container.Resolve<ILinkService>();

                LinkDetails link = linkService.GetLink(linkId);

                lvListCommentsDataSource.SelectMethod = commentServiceType.GetMethod("GetCommentsForLink").Name;
                lvListCommentsDataSource.SelectCountMethod = commentServiceType.GetMethod("CountCommentsForLink").Name;
                lvListCommentsDataSource.SelectParameters.Add("linkId", linkIdString);

                if (!IsPostBack)
                {
                    lnkForWhat.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/Link.aspx?linkId=" + linkIdString);
                    imgForWhat.ImageUrl = (string)GetLocalResourceObject("imgLink.ImageUrl");
                    imgForWhat.AlternateText = (string)GetLocalResourceObject("imgLink.AlternateText");
                    lclFor.Text += " " + (string)GetLocalResourceObject("lblLink.Text");
                    lblForWhat.Text = "'" + link.Name + "'";
                    lnkAddComment.Visible = true;
                    lnkAddComment.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Comment/AddComment.aspx?linkId=" + linkIdString);
                }
            }
            else
            {
                throw new ArgumentException("Wrong URL parameters");
            }

            lvListCommentsDataSource.ObjectCreating += this.LvListCommentsDataSource_ObjectCreating;
            lvListCommentsDataSource.TypeName = commentServiceType.FullName;
            lvListCommentsDataSource.EnablePaging = true;
            lvListCommentsDataSource.StartRowIndexParameterName = "startIndex";
            lvListCommentsDataSource.MaximumRowsParameterName = "count";

            dpListComments.PageSize = Settings.Default.WebMovies_commentsPerPage;

            lvListComments.DataSource = lvListCommentsDataSource;
            lvListComments.DataBind();
        }

        public void LvListCommentsDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            ICommentService commentService = new CommentService();
            commentService = (ICommentService)container.BuildUp(commentService.GetType(), commentService);

            e.ObjectInstance = commentService;
        }

        protected void LvListComments_PreRender(object sender, EventArgs e)
        {
            lvListComments.DataSource = lvListCommentsDataSource;
            lvListComments.DataBind();
        }

        public string Edit(long commentId)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ICommentService commentService = container.Resolve<ICommentService>();
            long commentUserId = commentService.GetComment(commentId).AuthorId;

            UserSession userSession = (UserSession)this.Context.Session["userSession"];
            long userId = userSession.UserProfileId;

            if (commentUserId == userId)
            {
                return "<a href=\"/Pages/Comment/EditComment.aspx?commentId=" + commentId + "\"><img src=\"" + GetLocalResourceObject("lnkEditComment.ImageUrl") + "\" alt=\"" + GetLocalResourceObject("lnkEditComment.AlternateText") + "\" /></a>";
            }
            else
            {
                return "";
            }
        }

        public string Remove(long commentId)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ICommentService commentService = container.Resolve<ICommentService>();
            ILinkService linkService = container.Resolve<ILinkService>();

            CommentDetails comment = commentService.GetComment(commentId);
            long commentLinkAuthorId = comment.AuthorId;

            LinkDetails link = linkService.GetLink(comment.LinkId);

            UserSession userSession = (UserSession)this.Context.Session["userSession"];
            long userId = userSession.UserProfileId;

            if ((comment.AuthorId == userId) || (link.UserId == userId))
            {
                return "<a href=\"/Pages/Comment/RemoveComment.aspx?commentId=" + commentId + "\"><img src=\"" + GetLocalResourceObject("lnkRemoveComment.ImageUrl") + "\" alt=\"" + GetLocalResourceObject("lnkRemoveComment.AlternateText") + "\" /></a>";
            }
            else
            {
                return "";
            }
        }

    }
}
