using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.WebMovies.Model.Services.LabelService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Es.Udc.DotNet.WebMovies.Web.Util;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Web
{
    public partial class WebMovies
        : MasterPage
    {

        public static readonly String USER_SESSION_ATTRIBUTE = "userSession";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (SessionManager.IsUserAuthenticated(Context))
            {
                long userId = SessionManager.GetUserSession(Context).UserProfileId;
                string login = SessionManager.GetUserSession(Context).Login;
                string name = SessionManager.GetUserSession(Context).FirstName;

                if (lblGreeting != null)
                {
                    lblGreeting.Text = GetLocalResourceObject("lblGreeting.Text").ToString() + " " + name + " (" + login + ")";
                }
                if (lnkAuthenticate != null)
                {
                    lnkAuthenticate.Visible = false;
                }
                if (lnkRegister != null)
                {
                    lnkRegister.Visible = false;
                }
                if (lblWelcome != null)
                {
                    lblWelcome.Visible = false;
                }

                if (lnkLinks != null)
                {
                    lnkLinks.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/ListLinks.aspx?userId=" + userId);
                }
                if (lnkFavorites != null)
                {
                    lnkFavorites.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Favorite/ListFavorites.aspx");
                }
                if (lnkComments != null)
                {
                    lnkComments.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Comment/ListComments.aspx?userId=" + userId);
                }
                if (lnkReported != null)
                {
                    IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                    ILinkService linkService = container.Resolve<ILinkService>();
                    int reportedLinksCount = linkService.CountReportedLinksForUser(userId, Settings.Default.WebMovies_demotedThreshold);
                    if (reportedLinksCount > 0)
                    {
                        lblReported.Text = reportedLinksCount.ToString();
                        lnkReported.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Link/ListLinks.aspx?reported=" + true.ToString());
                        lnkReported.Visible = true;
                    }
                }
            }
            else
            {
                if (lblGreeting != null)
                {
                    lblGreeting.Visible = false;
                }
                if (lnkLogout != null)
                {
                    lnkLogout.Visible = false;
                }

                if (lnkUpdate != null)
                {
                    lnkUpdate.Visible = false;
                }
                if (lblDashMenu1 != null)
                {
                    lblDashMenu1.Visible = false;
                }
                if (lnkLinks != null)
                {
                    lnkLinks.Visible = false;
                }
                if (lblDashMenu2 != null)
                {
                    lblDashMenu2.Visible = false;
                }
                if (lnkFavorites != null)
                {
                    lnkFavorites.Visible = false;
                }
                if (lblDashMenu3 != null)
                {
                    lblDashMenu3.Visible = false;
                }
                if (lnkComments != null)
                {
                    lnkComments.Visible = false;
                }
            }

            UpdateCloud();
        }

        private void UpdateCloud()
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            ILabelService labelService = container.Resolve<ILabelService>();

            DictionaryBlock<string, long> labelMap = labelService.GetMostValuedLabels(0, Settings.Default.WebMovies_labelsPerCloud);
            if (labelMap.Count == 0)
            {
                UpdateCloudFake();
                return;
            }
            double max = labelMap.Values.Max();
            double min = labelMap.Values.Min();

            SortedSet<ListItem> labels = new SortedSet<ListItem>(new AlphabeticalListItemTextComparer());
            foreach (string label in labelMap.Keys)
            {
                double rating = labelMap[label];
                int minSize = 30;
                if (max > min)
                {
                    minSize = (int) ((rating - min) / (max - min) * 20.0 + 20.0);
                }
                ListItem listItem = new ListItem(label, minSize.ToString());
                labels.Add(listItem);
            }

            CloudView.DataSource = labels;
            CloudView.DataBind();
        }

        private void UpdateCloudFake()
        {
            SortedSet<ListItem> labels = new SortedSet<ListItem>(new AlphabeticalListItemTextComparer());

            labels.Add(new ListItem("we", "21"));
            labels.Add(new ListItem("couldn't", "17"));
            labels.Add(new ListItem("find", "15"));
            labels.Add(new ListItem("global", "11"));
            labels.Add(new ListItem("labels", "29"));
            labels.Add(new ListItem("on", "15"));
            labels.Add(new ListItem("our", "30"));
            labels.Add(new ListItem("search", "15"));
            labels.Add(new ListItem("system", "26"));
            labels.Add(new ListItem("to", "20"));
            labels.Add(new ListItem("visualize", "10"));
            labels.Add(new ListItem("wandering", "12")); // It's alphabetically ordered!

            this.CloudView.DataSource = labels;
            this.CloudView.DataBind();
        }

    }
}
