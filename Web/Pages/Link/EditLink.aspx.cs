using System;
using System.Collections.Generic;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.LabelService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Properties;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Link
{
    public partial class EditLink
        : SpecificCulturePage
    {

        private long LinkId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkId = Int64.Parse(Request.Params.Get("linkId"));

            atvName.Visible = false;

            if (!IsPostBack)
            {
                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                ILinkService linkService = container.Resolve<ILinkService>();
                ILabelService labelService = container.Resolve<ILabelService>();

                LinkDetails link = linkService.GetLink(LinkId);

                lclEditLink.Text = GetLocalResourceObject("lclEditLink.Text") + " " + link.Name;
                string movieTitle = ApplicationManager.GetMovieTitle(link.MovieId);
                if (movieTitle != null)
                {
                    lclEditLink.Text = GetLocalResourceObject("lclEditLink.Text") + " " + link.Name + " " + GetLocalResourceObject("lclFor.Text") + " \"" + movieTitle + "\"";
                }
                lblUrl.Text = link.Url;
                txtName.Text = link.Name;
                txtDescription.Text = link.Description;
                string labelsString = "";
                foreach (string label in labelService.GetLabelsForLink(LinkId, 0, int.MaxValue - 1).Keys)
                {
                    labelsString += label + " ";
                }
                txtLabels.Text = labelsString;

                if (link.Rating <= Settings.Default.WebMovies_demotedThreshold)
                {
                    pEditLink.CssClass += " demoted";
                }
                else if (link.Rating >= Settings.Default.WebMovies_promotedThreshold)
                {
                    pEditLink.CssClass += " promoted";
                }
            }
        }

        protected void BtnEditLink_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                ILinkService linkService = container.Resolve<ILinkService>();
                ILabelService labelService = container.Resolve<ILabelService>();

                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                try
                {
                    linkService.UpdateLink(userId, LinkId, txtName.Text, txtDescription.Text);
                }
                catch (DuplicateInstanceException<LinkDetails> x)
                {
                    if (x.Properties[1].Name == "name")
                    {
                        atvName.Visible = true;
                    }
                    return;
                }

                labelService.SetLabelsForLink(userId, LinkId, new List<string>(txtLabels.Text.Split(' ')));

                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Link/Link.aspx?linkId=" + LinkId));
            }
        }

    }
}
