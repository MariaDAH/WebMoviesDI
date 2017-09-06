using System;
using System.Web;
using System.Xml.Xsl;
using Es.Udc.DotNet.WebMovies.Model.Services.MovieService;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Microsoft.Practices.Unity;
using System.Xml;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Movie
{
    public partial class ListMoviesXml
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string keywordsUrl = Request.Params.Get("keywords");
            string keywords = HttpUtility.UrlDecode(keywordsUrl);

            int startIndex = 0;
            try
            {
                startIndex = Int32.Parse(Request.Params.Get("startIndex"));
            } catch (ArgumentNullException) { }

            int count = Settings.Default.WebMovies_linksPerPage;

            lclSearch.Text = GetLocalResourceObject("lclSearch.Text") + ": \"" + keywords + "\"";

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            IMovieService movieService = container.Resolve<IMovieService>();

            XmlDocument xml = movieService.SearchMoviesXml(keywords, startIndex, Settings.Default.WebMovies_linksPerPage);
            movieListXml.DocumentContent = xml.InnerXml;

            movieListXml.TransformSource = Server.MapPath("ListMoviesXmlToAsp.xslt");

            XsltArgumentList arguments = new XsltArgumentList();
            arguments.AddParam("addLinkImage", String.Empty, GetLocalResourceObject("lnkAddLink.ImageUrl"));
            arguments.AddParam("addLinkText", String.Empty, GetLocalResourceObject("lnkAddLink.Text"));
            arguments.AddParam("noProductsText", String.Empty, GetLocalResourceObject("lclNoResults.Text"));
            arguments.AddParam("viewLinksImage", String.Empty, GetLocalResourceObject("lnkViewLinks.ImageUrl"));
            arguments.AddParam("viewLinksText", String.Empty, GetLocalResourceObject("lnkViewLinks.Text"));
            movieListXml.TransformArgumentList = arguments;

            int previousIndex = 0;
            if (count <= startIndex)
            {
                previousIndex += startIndex - count;
            }
            lnkPrevious.NavigateUrl = Response.ApplyAppPathModifier("ListMoviesXml.aspx?keywords=" + keywordsUrl + "&startIndex=" + previousIndex);
            lnkPrevious.Visible = startIndex > 0;

            lnkNext.NavigateUrl = Response.ApplyAppPathModifier("ListMoviesXml.aspx?keywords=" + keywordsUrl + "&startIndex=" + (startIndex + count));
            lnkNext.Visible = false;
            try {
                lnkNext.Visible = xml.GetElementsByTagName("more")[0].InnerText.Trim() == "true";
            }
            catch (NullReferenceException) { }
        }

    }
}
