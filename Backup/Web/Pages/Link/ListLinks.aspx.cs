using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.WebMovies.Model.Services.LabelService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Util;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Es.Udc.DotNet.WebMovies.Web.Util;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;
using System.Data;
using Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService;
using Es.Udc.DotNet.WebMovies.Model.Services.RatingService;
using Es.Udc.DotNet.WebMovies.Web.Http.Util;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Link
{
    public partial class ListLinks
        : SpecificCulturePage
    {

        private ObjectDataSource lvListLinksDataSource = new ObjectDataSource();

        protected void Page_Load(object sender, EventArgs e)
        {
            string orderedBy = Request.Params.Get("orderedBy");
            string userIdString = Request.Params.Get("userId");
            string movieIdString = Request.Params.Get("movieId");
            string labelString = Request.Params.Get("label");
            string reportedString = Request.Params.Get("reported");

            Type linkServiceType = typeof(ILinkService);

            if ((userIdString == null) && (movieIdString == null) && (labelString == null) && (reportedString != null) && (reportedString.ToLower() == "true"))
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                IUserService userService = container.Resolve<IUserService>();

                if (!IsPostBack)
                {
                    lclLinks.Text = (string)GetLocalResourceObject("lclReportedLinks.Text");
                    lclFor.Visible = true;
                    lclFor.Text += " " + (string)GetLocalResourceObject("lblUser.Text");
                    lnkForWhat.Visible = true;
                    lblForWhat.Text = userService.GetUserProfile(userId).LoginName;
                    imgForWhat.ImageUrl = (string)GetLocalResourceObject("imgReported.ImageUrl");
                    imgForWhat.AlternateText = (string)GetLocalResourceObject("imgReported.AlternateText");
                    lnkForWhat.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Link/ListLinks.aspx?userId=" + userId.ToString());
                }

                lvListLinksDataSource.SelectMethod = linkServiceType.GetMethod("GetReportedLinksForUser").Name;
                lvListLinksDataSource.SelectCountMethod = linkServiceType.GetMethod("CountReportedLinksForUser").Name;
                lvListLinksDataSource.SelectParameters.Add("userId", DbType.Int64, userId.ToString());
                lvListLinksDataSource.SelectParameters.Add("threshold", DbType.Int32, Settings.Default.WebMovies_demotedThreshold.ToString());
                lnkOrderBy.Visible = false;
            }
            else if (((userIdString != null) && (movieIdString == null) && (labelString == null) && (reportedString == null)) || ((userIdString == null) && (movieIdString == null) && (labelString == null) && (reportedString == null)))
            {
                if (userIdString == null)
                {
                    UserSession userSession = (UserSession)this.Context.Session["userSession"];
                    userIdString = userSession.UserProfileId.ToString();
                }
                long userId = Int64.Parse(userIdString);

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                IUserService userService = container.Resolve<IUserService>();

                if (!IsPostBack)
                {
                    lclFor.Visible = true;
                    lclFor.Text += " " + (string)GetLocalResourceObject("lblUser.Text");
                    lnkForWhat.Visible = true;
                    lblForWhat.Text = userService.GetUserProfile(userId).LoginName;
                    imgForWhat.ImageUrl = (string)GetLocalResourceObject("imgUser.ImageUrl");
                    imgForWhat.AlternateText = (string)GetLocalResourceObject("imgUser.AlternateText");
                    lnkForWhat.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Link/ListLinks.aspx?userId=" + userIdString);
                }

                if (orderedBy == "rating")
                {
                    lvListLinksDataSource.SelectMethod = linkServiceType.GetMethod("GetMostValuedLinksForUser").Name;
                }
                else
                {
                    lvListLinksDataSource.SelectMethod = linkServiceType.GetMethod("GetLinksForUser").Name;
                }
                lvListLinksDataSource.SelectCountMethod = linkServiceType.GetMethod("CountLinksForUser").Name;
                lvListLinksDataSource.SelectParameters.Add("userId", DbType.Int64, userIdString);
            }
            else if ((userIdString == null) && (movieIdString != null) && (labelString == null) && (reportedString == null))
            {
                long movieId = Int64.Parse(movieIdString);

                if (!IsPostBack)
                {
                    lclFor.Visible = true;
                    lclFor.Text += " " + (string)GetLocalResourceObject("lblMovie.Text");
                    lnkForWhat.Visible = true;
                    lblForWhat.Text = "\"" + ApplicationManager.GetMovieTitle(movieId) + "\"";
                    imgForWhat.ImageUrl = (string)GetLocalResourceObject("imgMovie.ImageUrl");
                    imgForWhat.AlternateText = (string)GetLocalResourceObject("imgMovie.AlternateText");
                    if (CookiesManager.GetPreferredSearchEngine(Context) == "webshop")
                    {
                        lnkForWhat.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Movie/Movie.aspx?movieId=" + movieIdString);
                    }
                    else
                    {
                        lnkForWhat.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Movie/MovieXml.aspx?movieId=" + movieIdString);
                    }
                }

                if (orderedBy == "rating")
                {
                    lvListLinksDataSource.SelectMethod = linkServiceType.GetMethod("GetMostValuedLinksForMovie").Name;
                }
                else
                {
                    lvListLinksDataSource.SelectMethod = linkServiceType.GetMethod("GetLinksForMovie").Name;
                }
                lvListLinksDataSource.SelectCountMethod = linkServiceType.GetMethod("CountLinksForMovie").Name;
                lvListLinksDataSource.SelectParameters.Add("movieId", DbType.Int64, movieIdString);

                lnkAddLink.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/AddLink.aspx?movieId=" + movieIdString);
                lnkAddLink.Visible = true;
            }
            else if ((userIdString == null) && (movieIdString == null) && (labelString != null) && (reportedString == null))
            {
                if (!IsPostBack)
                {
                    lclFor.Visible = true;
                    lclFor.Text += " " + (string)GetLocalResourceObject("lblLabel.Text");
                    lnkForWhat.Visible = true;
                    lblForWhat.Text = "'" + labelString + "'";
                    imgForWhat.ImageUrl = (string)GetLocalResourceObject("imgLabel.ImageUrl");
                    imgForWhat.AlternateText = (string)GetLocalResourceObject("imgLabel.AlternateText");
                    lnkForWhat.NavigateUrl = Response.ApplyAppPathModifier("/Pages/Link/ListLinks.aspx?label=" + labelString);
                }

                if (orderedBy == "rating")
                {
                    lvListLinksDataSource.SelectMethod = linkServiceType.GetMethod("GetMostValuedLinksForLabel").Name;
                }
                else
                {
                    lvListLinksDataSource.SelectMethod = linkServiceType.GetMethod("GetLinksForLabel").Name;
                }
                lvListLinksDataSource.SelectCountMethod = linkServiceType.GetMethod("CountLinksForLabel").Name;
                lvListLinksDataSource.SelectParameters.Add("label", DbType.String, labelString);
            }
            else
            {
                throw new ArgumentException("Wrong URL parameters");
            }

            lvListLinksDataSource.ObjectCreating += this.LvListLinksDataSource_ObjectCreating;
            lvListLinksDataSource.TypeName = linkServiceType.FullName;
            lvListLinksDataSource.EnablePaging = true;
            lvListLinksDataSource.StartRowIndexParameterName = "startIndex";
            lvListLinksDataSource.MaximumRowsParameterName = "count";

            dpListLinks.PageSize = Settings.Default.WebMovies_linksPerPage;

            lvListLinks.DataSource = lvListLinksDataSource;
            lvListLinks.DataBind();

            if (!IsPostBack)
            {
                string reorderUrl = Request.Url.AbsolutePath + "?";
                if (userIdString != null)
                {
                    reorderUrl += "userId=" + userIdString;
                }
                if (movieIdString != null)
                {
                    reorderUrl += "movieId=" + movieIdString;
                }
                if (labelString != null)
                {
                    reorderUrl += "label=" + labelString;
                }
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

                HyperLink lnkReturn = lvListLinks.Controls[0].FindControl("lnkReturn") as HyperLink;
                if (lnkReturn != null)
                {
                    if (Request.UrlReferrer != null)
                    {
                        lnkReturn.NavigateUrl = Response.ApplyAppPathModifier(Request.UrlReferrer.AbsoluteUri);
                    }
                }
            }
        }

        public void LvListLinksDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            ILinkService linkService = new LinkService();
            linkService = (ILinkService)container.BuildUp(linkService.GetType(), linkService);

            e.ObjectInstance = linkService;
        }

        protected void LvListLinks_PreRender(object sender, EventArgs e)
        {
            lvListLinks.DataSource = lvListLinksDataSource;
            lvListLinks.DataBind();
        }

        public string MovieTitle(long movieId)
        {
            return ApplicationManager.GetMovieTitle(movieId);
        }

        public string Css(long linkId)
        {
            string css = "link";

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

        public string Labels(long linkId)
        {
            try
            {
                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

                ILabelService labelService = container.Resolve<ILabelService>();

                DictionaryBlock<string, long> labelMap = labelService.GetLabelsForLink(linkId, 0, int.MaxValue - 1);
                double max = labelMap.Values.Max();
                double min = labelMap.Values.Min();

                SortedSet<ListItem> labels = new SortedSet<ListItem>(new AlphabeticalListItemTextComparer());
                foreach (string label in labelMap.Keys)
                {
                    double rating = labelMap[label];
                    int minSize = 15;
                    if (max > min)
                    {
                        minSize = (int)((rating - min) / (max - min) * 10.0 + 10.0);
                    }
                    ListItem listItem = new ListItem(label, minSize.ToString());
                    labels.Add(listItem);
                }

                string labelsString = "";
                foreach (ListItem listItem in labels)
                {
                    labelsString += "<a href=\"/Pages/Link/ListLinks.aspx?label=" + listItem.Text + "\" style=\"font-size: " + listItem.Value + "px;\">" + listItem.Text + "</a>";
                }

                return labelsString;
            }
            catch
            {
                return "&nbsp;";
            }
        }

        public bool ReportReadVisible(long linkId)
        {
            if (SessionManager.IsUserAuthenticated(Context))
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                ILinkService linkService = container.Resolve<ILinkService>();
                LinkDetails link = linkService.GetLink(linkId);

                return ((link.UserId == userId) && (link.Rating <= Settings.Default.WebMovies_demotedThreshold) && (!link.ReportRead));
            }
            else
            {
                return false;
            }
        }

        public string Edit(long linkId)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();
            long linkUserId = linkService.GetLink(linkId).UserId;

            if (SessionManager.IsUserAuthenticated(Context))
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                if (linkUserId == userId)
                {
                    return "<a href=\"/Pages/Link/EditLink.aspx?linkId=" + linkId + "\"><img src=\"" + GetLocalResourceObject("lnkEditLink.ImageUrl") + "\" alt=\"" + GetLocalResourceObject("lnkEditLink.AlternateText") + "\" /></a>";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public string Remove(long linkId)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();
            long linkUserId = linkService.GetLink(linkId).UserId;

            if (SessionManager.IsUserAuthenticated(Context))
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                if (linkUserId == userId)
                {
                    return "<a href=\"/Pages/Link/RemoveLink.aspx?linkId=" + linkId + "\"><img src=\"" + GetLocalResourceObject("lnkRemoveLink.ImageUrl") + "\" alt=\"" + GetLocalResourceObject("lnkRemoveLink.AlternateText") + "\" /></a>";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public string RateUpCss(long linkId)
        {
            if (SessionManager.IsUserAuthenticated(Context))
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                IRatingService ratingService = container.Resolve<IRatingService>();

                if (ratingService.GetRating(userId, linkId) > 0)
                {
                    return "selected";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public string RateDownCss(long linkId)
        {
            if (SessionManager.IsUserAuthenticated(Context))
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                IRatingService ratingService = container.Resolve<IRatingService>();

                if (ratingService.GetRating(userId, linkId) < 0)
                {
                    return "selected";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public bool RateEnabled(long linkId)
        {
            if (SessionManager.IsUserAuthenticated(Context))
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                ILinkService linkService = container.Resolve<ILinkService>();

                LinkDetails link = linkService.GetLink(linkId);

                if (link.UserId == userId)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public string Favorite(long linkId)
        {
            if (SessionManager.IsUserAuthenticated(Context))
            {
                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                ILinkService linkService = container.Resolve<ILinkService>();
                IFavoriteService favoriteService = container.Resolve<IFavoriteService>();

                LinkDetails link = linkService.GetLink(linkId);

                string href = " href=\"/Pages/Favorite/AddFavorite.aspx?linkId=" + linkId + "\"";
                string image = (string)GetLocalResourceObject("lnkFavorite.ImageURL");
                string text = (string)GetLocalResourceObject("lnkFavorite.Text");
                string disabled = "";
                if (favoriteService.HasInFavorites(userId, linkId))
                {
                    href = " href=\"/Pages/Favorite/RemoveFavorite.aspx?linkId=" + linkId + "\"";
                    image = (string)GetLocalResourceObject("lnkUnfavorite.ImageURL");
                    text = (string)GetLocalResourceObject("lnkUnfavorite.Text");
                }
                if (link.UserId == userId)
                {
                    href = "";
                    disabled = " disabled=\"disabled\"";
                }

                return "<a" + href + disabled + "><img src=\"" + image + "\" alt=\"" + text + "\" /></a>";
            }
            else
            {
                return "<a href=\"/Pages/Favorite/AddFavorite.aspx?linkId=" + linkId + "\"><img src=\"" + GetLocalResourceObject("lnkFavorite.ImageURL") + "\" alt=\"" + GetLocalResourceObject("lnkFavorite.Text") + "\" /></a>";
            }
        }

        public void RateUp(object sender, CommandEventArgs e)
        {
            if (SessionManager.IsUserAuthenticated(Context))
            {
                long linkId = Int64.Parse(e.CommandArgument.ToString());

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                IRatingService ratingService = container.Resolve<IRatingService>();

                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                int ratedValue = ratingService.GetRating(userId, linkId);
                if (ratedValue <= 0)
                {
                    ratingService.Rate(userId, linkId, 1);
                }
                else
                {
                    ratingService.Rate(userId, linkId, 0);
                }

                //Server.Transfer(Request.Url.AbsolutePath);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/User/Authentication.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsolutePath)));
            }
        }

        public void RateDown(object sender, CommandEventArgs e)
        {
            if (SessionManager.IsUserAuthenticated(Context))
            {
                long linkId = Int64.Parse(e.CommandArgument.ToString());

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                IRatingService ratingService = container.Resolve<IRatingService>();

                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                int ratedValue = ratingService.GetRating(userId, linkId);
                if (ratedValue >= 0)
                {
                    ratingService.Rate(userId, linkId, -1);
                }
                else
                {
                    ratingService.Rate(userId, linkId, 0);
                }

                //Server.Transfer(Request.Url.AbsolutePath);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/User/Authentication.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsolutePath)));
            }
        }

        public void ReportRead(object sender, CommandEventArgs e)
        {
            long linkId = Int64.Parse(e.CommandArgument.ToString());

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();

            UserSession userSession = (UserSession)this.Context.Session["userSession"];
            long userId = userSession.UserProfileId;

            linkService.SetReportedLinkAsRead(userId, linkId);

            //Server.Transfer(Request.Url.AbsolutePath);
            Response.Redirect(Request.Url.AbsoluteUri);
        }

    }
}
