using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Net;
using Es.Udc.DotNet.WebMovies.Model.MovieService;
using System.Xml.XPath;
using Es.Udc.DotNet.WebMovies.Web.Properties;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using System.Xml.Xsl;


namespace Es.Udc.DotNet.WebMovies.Web.Pages.Movie
{
    public partial class ShowProductWithoutDetails : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /* Hide the links */
            lnkPrevious.Visible = false;
            lnkNext.Visible = false;
            lblNoMovies.Visible = false;

            // Parameters 

            //Get keywords
            String keywords = Request.Params.Get("keywords");

            // Get Start Index 
            int startIndex, count;
            try
            {
                startIndex = Int32.Parse(Request.Params.Get("startIndex"));
            }
            catch (ArgumentNullException)
            {
                startIndex = 0;
            }

            //Get Count 

            try
            {
                count = Int32.Parse(Request.Params.Get("count"));
            }
            catch (ArgumentNullException)
            {
                count = Settings.Default.WebMovies_moviesPerPage;
            }

            /* Get movieTitle */
            String movieTitle;
            if ((movieTitle = Request.Params.Get("name")) == null)
                movieTitle = ((string)Context.Items["movieTitle"]);

            String[] nameStrings = movieTitle.Split(' ');

            // Se pasan al xslt los datos de localizacion como parametros
            XsltArgumentList arguments = new XsltArgumentList();
            arguments.AddParam("addLink", "", GetLocalResourceObject("addLink"));
            arguments.AddParam("showLinks", "", GetLocalResourceObject("showLinks"));
            arguments.AddParam("addFavorite", "", GetLocalResourceObject("addFavorite"));
            aspXmlMovies.TransformArgumentList = arguments;

            //Get xmlMovie 
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            IMovieService productsService = container.Resolve<IMovieService>();
            XmlDocument xmlDocument = productsService.FindMovie(keywords, startIndex, count);


            XmlElement xmlElement = xmlDocument.GetElementById("more");


            // en caso de que no haya movies, mostramos un mensaje
            if (xmlDocument.GetElementsByTagName("product").Item(0) == null)
            {
                lblNoMovies.Visible = true;
            }

            //Create Navigator
            XPathNavigator navigator = xmlDocument.CreateNavigator();

            //Transformcacion xslt
            aspXmlMovies.XPathNavigator = navigator;
            aspXmlMovies.TransformSource = Server.MapPath("Movies.xslt");


            /* "Previous" link */
            if ((startIndex - count) >= 0)
            {
                String url = String.Format(
                    Settings.Default.WebMovies_applicationURL +
                    "~/Pages/Product/ShowMovieWithoutDetails.aspx?movieTitle={0}" +
                    "&startIndex={1}&count={2}", movieTitle, startIndex - count, count);

                this.lnkPrevious.NavigateUrl =
                    Response.ApplyAppPathModifier(url);
                this.lnkPrevious.Visible = true;
            }


            /* "Next" link */
            XmlNode xmlNode = xmlDocument.SelectSingleNode("/response/more");

            if (xmlNode != null)
            {
                String url = String.Format(
                    Settings.Default.WebMovies_applicationURL +
                    "~/Pages/Product/ShowMovieWithoutDetails.aspx?movieTitle={0}" +
                    "&startIndex={1}&count={2}", movieTitle, startIndex + count, count);

                this.lnkNext.NavigateUrl = Response.ApplyAppPathModifier(url);
                this.lnkNext.Visible = true;
            }
        }
    }

}
    
