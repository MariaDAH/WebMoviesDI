using System;
using System.Collections.Generic;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.LabelService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Link
{
    public partial class AddLink
        : SpecificCulturePage
    {

        private long MovieId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            MovieId = Int64.Parse(Request.Params.Get("movieId"));

            atvUrl.Visible = false;
            atvName.Visible = false;

            if (!IsPostBack)
            {
                string movieTitle = ApplicationManager.GetMovieTitle(MovieId);
                if (movieTitle != null)
                {
                    lclAddLink.Text = GetLocalResourceObject("lclAddLinkFor.Text") + " \"" + movieTitle + "\"";
                }
            }
        }

        protected void BtnAddLink_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
                ILinkService linkService = container.Resolve<ILinkService>();
                ILabelService labelService = container.Resolve<ILabelService>();

                UserSession userSession = (UserSession)this.Context.Session["userSession"];
                long userId = userSession.UserProfileId;

                long linkId;
                try
                {
                    linkId = linkService.AddLink(userId, MovieId, txtName.Text, txtDescription.Text, txtUrl.Text);
                }
                catch (DuplicateInstanceException<LinkDetails> ex)
                {
                    if (ex.Properties[1].Name == "url")
                    {
                        atvUrl.Visible = true;
                    }
                    else if (ex.Properties[1].Name == "name")
                    {
                        atvName.Visible = true;
                    }
                    return;
                }
                labelService.SetLabelsForLink(userId, linkId, new List<string>(txtLabels.Text.Split(' ')));

                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Link/Link.aspx?linkId=" + linkId));
            }
        }

    }
}
