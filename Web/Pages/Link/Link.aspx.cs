using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService;
using Es.Udc.DotNet.WebMovies.Model.Services.LabelService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Services.RatingService;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Es.Udc.DotNet.WebMovies.Web.Http.Util;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Es.Udc.DotNet.WebMovies.Web.Util;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Link
{
    public partial class Link
        : SpecificCulturePage
    {

        private long LinkId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkId = Int64.Parse(Request.Params.Get("linkId"));

            if (!IsPostBack)
            {
                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

                ILinkService linkService = container.Resolve<ILinkService>();
                IRatingService ratingService = container.Resolve<IRatingService>();
                IFavoriteService favoriteService = container.Resolve<IFavoriteService>();

                LinkDetails link = linkService.GetLink(LinkId);

                string searchEngine = CookiesManager.GetPreferredSearchEngine(Context);

                lblName.Text = link.Name;

                lblUrl.Text = link.Url;
                lnkUrl.NavigateUrl = "http://" + link.Url;

                lblAuthor.Text = link.UserName;
                lnkAuthor.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/ListLinks.aspx?userId=" + link.UserId);

                lblMovie.Text = ApplicationManager.GetMovieTitle(link.MovieId);
                if (searchEngine == "webshop")
                {
                    lnkMovie.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Movie/Movie.aspx?movieId=" + link.MovieId);
                }
                else
                {
                    lnkMovie.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Movie/MovieXml.aspx?movieId=" + link.MovieId);
                }

                lblDate.Text = link.Date.ToString();

                lblDescription.Text = link.Description;

                lblRating.Text = link.Rating.ToString();

                lnkFavorite.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Favorite/AddFavorite.aspx?linkId=" + LinkId);

                lnkEditLink.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/EditLink.aspx?linkId=" + LinkId);
                lnkRemoveLink.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/RemoveLink.aspx?linkId=" + LinkId);

                lnkComments.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Comment/ListComments.aspx?linkId=" + LinkId);
                lnkAddComment.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Comment/AddComment.aspx?linkId=" + LinkId);

                if (SessionManager.IsUserAuthenticated(Context))
                {
                    UserSession userSession = (UserSession)this.Context.Session["userSession"];
                    long userId = userSession.UserProfileId;

                    lnkRateUp.Enabled = (link.UserId != userId);
                    lnkRateDown.Enabled = (link.UserId != userId);
                    int ratedValue = ratingService.GetRating(userId, LinkId);
                    if (ratedValue > 0)
                    {
                        lnkRateUp.CssClass += " selected";
                    }
                    else if (ratedValue < 0)
                    {
                        lnkRateDown.CssClass += " selected";
                    }

                    lnkFavorite.Enabled = (link.UserId != userId);
                    if (favoriteService.HasInFavorites(userId, LinkId))
                    {
                        lnkFavorite.ImageUrl = (string)GetLocalResourceObject("lnkUnfavorite.ImageUrl");
                        lnkFavorite.Text = (string)GetLocalResourceObject("lnkUnfavorite.Text");
                        lnkFavorite.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Favorite/RemoveFavorite.aspx?linkId=" + LinkId);
                    }

                    lnkEditLink.Visible = (link.UserId == userId);
                    lnkRemoveLink.Visible = (link.UserId == userId);

                    lnkReported.Visible = (link.UserId == userId) && (link.Rating <= Settings.Default.WebMovies_demotedThreshold) && (!link.ReportRead);
                }

                if (link.Rating <= Settings.Default.WebMovies_demotedThreshold)
                {
                    pLink.CssClass += " demoted";
                }
                else if (link.Rating >= Settings.Default.WebMovies_promotedThreshold)
                {
                    pLink.CssClass += " promoted";
                }

                UpdateLabels();
            }
        }

        private void UpdateLabels()
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            ILabelService labelService = container.Resolve<ILabelService>();

            DictionaryBlock<string, long> labelMap = labelService.GetLabelsForLink(LinkId, 0, int.MaxValue - 1);
            if (labelMap.Count > 0)
            {
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

                lvLabels.DataSource = labels;
                lvLabels.DataBind();
            }
        }

        public void RateUp(object sender, EventArgs e)
        {
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect("");
            }

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            IRatingService ratingService = container.Resolve<IRatingService>();

            UserSession userSession = (UserSession)this.Context.Session["userSession"];
            long userId = userSession.UserProfileId;

            int ratedValue = ratingService.GetRating(userId, LinkId);
            if (ratedValue <= 0)
            {
                ratingService.Rate(userId, LinkId, 1);
            }
            else
            {
                ratingService.Rate(userId, LinkId, 0);
            }

            //Server.Transfer(Request.Url.AbsolutePath);
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        public void RateDown(object sender, EventArgs e)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            IRatingService ratingService = container.Resolve<IRatingService>();

            UserSession userSession = (UserSession)this.Context.Session["userSession"];
            long userId = userSession.UserProfileId;

            int ratedValue = ratingService.GetRating(userId, LinkId);
            if (ratedValue >= 0)
            {
                ratingService.Rate(userId, LinkId, -1);
            }
            else
            {
                ratingService.Rate(userId, LinkId, 0);
            }

            //Server.Transfer(Request.Url.AbsolutePath);
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        public void ReportRead(object sender, EventArgs e)
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            ILinkService linkService = container.Resolve<ILinkService>();

            UserSession userSession = (UserSession)this.Context.Session["userSession"];
            long userId = userSession.UserProfileId;

            linkService.SetReportedLinkAsRead(userId, LinkId);

            //Server.Transfer(Request.Url.AbsolutePath);
            Response.Redirect(Request.Url.AbsoluteUri);
        }

    }
}
