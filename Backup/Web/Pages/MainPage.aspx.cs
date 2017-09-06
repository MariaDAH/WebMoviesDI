using System;
using System.Web;
using System.Web.UI;
using Es.Udc.DotNet.WebMovies.Model.Services.MovieService;
using Es.Udc.DotNet.WebMovies.Model.Util;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.WebMovies.Web.Http.Util;

namespace Es.Udc.DotNet.WebMovies.Web.Pages
{
    public partial class MainPage
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String preferredSearchEngine = CookiesManager.GetPreferredSearchEngine(Context);

                if (preferredSearchEngine != null)
                {
                    ddlSearchEngine.SelectedValue = preferredSearchEngine;
                }
            }
        }

        protected void BtnSearchButtonClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string keywords = txtSearchKeywords.Text;
                string engine = ddlSearchEngine.SelectedValue;

                CookiesManager.LeavePreferredSearchEngineCookie(Context, engine);

                SearchMovies(keywords, engine);
            }
        }

        private void SearchMovies(string keywords, string engine)
        {
            if (engine == "webshop") // Búsqueda con la práctica optativa
            {
                Response.Redirect(Response.ApplyAppPathModifier("Movie/ListMovies.aspx"
                    + "?keywords=" + HttpUtility.UrlEncode(keywords)));
            }
            else // Búsqueda con la práctica normal (tranformación XSLT)
            {
                Response.Redirect(Response.ApplyAppPathModifier("Movie/ListMoviesXml.aspx"
                    + "?keywords=" + HttpUtility.UrlEncode(keywords)));
            }
        }

    }
}
