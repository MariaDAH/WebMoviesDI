using System;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.MovieService;
using System.Xml.Xsl;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using System.Xml;
using Es.Udc.DotNet.WebMovies.Web.Http.Application;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Movie
{
    public partial class MovieXml
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            int movieId = Int32.Parse(Request.Params.Get("movieId"));

            string returnUrl = "/Pages/MainPage.aspx";
            Uri referrerUrl = Request.UrlReferrer;
            if (referrerUrl != null)
            {
                returnUrl = Response.ApplyAppPathModifier(referrerUrl.AbsoluteUri);
            }

            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            IMovieService movieService = container.Resolve<IMovieService>();
            XmlDocument xml = movieService.GetMovieXml(movieId);
            movieXml.DocumentContent = xml.InnerXml;

            movieXml.TransformSource = Server.MapPath("MovieXmlToAsp.xslt");

            XsltArgumentList arguments = new XsltArgumentList();
            arguments.AddParam("addLinkImage", String.Empty, GetLocalResourceObject("lnkAddLink.ImageUrl"));
            arguments.AddParam("addLinkText", String.Empty, GetLocalResourceObject("lnkAddLink.Text"));
            arguments.AddParam("movieImage", String.Empty, GetLocalResourceObject("imgMovie.ImageUrl"));
            arguments.AddParam("noProductText", String.Empty, GetLocalResourceObject("lclError.Text"));
            arguments.AddParam("purchaseImage", String.Empty, GetLocalResourceObject("imgPurchase.ImageUrl"));
            arguments.AddParam("purchaseText", String.Empty, GetLocalResourceObject("imgPurchase.AlternateText"));
            arguments.AddParam("purchaseUrl", String.Empty, Settings.Default.WebShop_purchaseUrl);
            arguments.AddParam("returnText", String.Empty, GetLocalResourceObject("lnkReturn.Text"));
            arguments.AddParam("returnUrl", String.Empty, returnUrl);
            arguments.AddParam("viewLinksImage", String.Empty, GetLocalResourceObject("imgViewLinks.ImageUrl"));
            arguments.AddParam("viewLinksText", String.Empty, GetLocalResourceObject("imgViewLinks.AlternateText"));
            movieXml.TransformArgumentList = arguments;

            if (xml.GetElementsByTagName("error")[0] != null)
            {
                lblTitle.Text = (string)GetGlobalResourceObject("Common", "error");
            }
        }

    }
}
