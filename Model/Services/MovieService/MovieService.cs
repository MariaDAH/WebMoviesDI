using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.MovieProxy;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Model.Services.MovieService
{
    public class MovieService
        : IMovieService
    {

        private static string PRODUCT_NOT_FOUND_ERROR = "Product not found";
        private static string INCORRECT_PARAMETERS_ERROR = "Incorrect parameters";
        private static string PRODUCTS_NOT_FOUND_ERROR = "Products not found";

        [Dependency]
        public IMovieProxy MovieProxy { private get; set; }

        [Dependency]
        public IUserProfileDao UserProfileDao { private get; set; }

        [Dependency]
        public ILinkDao LinkDao { private get; set; }

        #region IMovieService Members

        public MovieDetails GetMovie(long movieId)
        {
            string xmlString;
            try
            {
                xmlString = MovieProxy.DetailMovie(movieId);
            }
            catch (Exception ex)
            {
                throw new NetworkException(ex);
            }

            XDocument xmlDocument = XDocument.Parse(xmlString, LoadOptions.None);

            XElement errorElement = xmlDocument.Element("xml").Element("error");
            if (errorElement != null)
            {
                XElement textElement = errorElement.Element("text");
                if (textElement != null)
                {
                    string textValue = textElement.Value.Trim();
                    if (textValue == PRODUCT_NOT_FOUND_ERROR)
                    {
                        throw new InstanceNotFoundException<MovieDetails>("movieId", movieId);
                    }
                    else
                    {
                        throw new XmlErrorException("Unknown error message");
                    }
                }

                throw new XmlErrorException("No <text> tag found for <error>.");
            }

            XElement categoryElement = xmlDocument.Element("xml").Element("product").Element("category").Element("id");
            if (categoryElement.Value.Trim() != Properties.Settings.Default.WebShop_moviesCategoryId)
            {
                throw new InstanceNotFoundException<MovieDetails>("movieId", movieId);
            }

            CultureInfo culture = new CultureInfo("en-US");
            var result = (from ev in xmlDocument.Element("xml").Elements("product")
                          select new MovieDetails
                          {
                              MovieId = Int64.Parse(ev.Element("id").Value.Trim(), culture),
                              Title = ev.Element("name").Value.Trim(),
                              Description = ev.Element("description").Value.Trim(),
                              Price = Double.Parse(ev.Element("price").Value.Trim(), culture),
                              LinkCount = LinkDao.CountForMovie(Int64.Parse(ev.Element("id").Value.Trim(), culture))
                          }).ToList();

            if (result.Count != 1)
            {
                throw new XmlErrorException("More than one <product> tags were found.");
            }

            return result.First();
        }

        public XmlDocument GetMovieXml(long movieId)
        {
            XmlDocument xml = new XmlDocument();

            string xmlString;
            try
            {
                xmlString = MovieProxy.DetailMovie(movieId);
            }
            catch (Exception ex)
            {
                throw new NetworkException(ex);
            }

            try
            {
                XDocument xmlDocument = XDocument.Parse(xmlString, LoadOptions.None);
                XElement categoryElement = xmlDocument.Element("xml").Element("product").Element("category").Element("id");
                if (categoryElement.Value.Trim() != Properties.Settings.Default.WebShop_moviesCategoryId)
                {
                    xml.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<xml><error><text>Products not found</text></error></xml>");
                    return xml;
                }
            }
            catch (NullReferenceException) { }

            xml.LoadXml(xmlString);

            return xml;
        }

        public ListBlock<MovieDetails> SearchMovies(string keywords, int startIndex, int count)
        {
            string xmlString;
            try
            {
                xmlString = MovieProxy.FindMovies(keywords, startIndex, count);
            }
            catch (Exception ex)
            {
                throw new NetworkException(ex);
            }

            XDocument xmlDocument = XDocument.Parse(xmlString, LoadOptions.None);

            XElement errorElement = xmlDocument.Element("xml").Element("error");
            if (errorElement != null)
            {
                XElement textElement = errorElement.Element("text");
                if (textElement != null)
                {
                    string textValue = textElement.Value.Trim();
                    if (textValue == PRODUCTS_NOT_FOUND_ERROR)
                    {
                        return new ListBlock<MovieDetails>(startIndex, false);
                    }
                    else if (textValue == INCORRECT_PARAMETERS_ERROR)
                    {
                        throw new InternalErrorException("Failure building web service request.");
                    }
                    else
                    {
                        throw new XmlErrorException("Unknown error message");
                    }
                }

                throw new XmlErrorException("No <text> tag found for <error>.");
            }

            bool hasMore;

            XElement moreElement = xmlDocument.Element("xml").Element("more");
            if (moreElement != null)
            {
                string moreValue = moreElement.Value.Trim();
                if (moreValue == "true")
                {
                    hasMore = true;
                }
                else if (moreValue == "false")
                {
                    hasMore = false;
                }
                else
                {
                    throw new InternalErrorException("Tag <more> has neither \"true\" nor \"false\" values.");
                }
            }
            else
            {
                throw new InternalErrorException("No tag <more> was found.");
            }

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            var result = (from ev in xmlDocument.Element("xml").Element("allproducts").Elements("product")
                          select new MovieDetails
                          {
                              MovieId = Int64.Parse(ev.Element("id").Value.Trim(), culture),
                              Title = ev.Element("name").Value.Trim(),
                              Description = null,
                              Price = Double.Parse(ev.Element("price").Value.Trim(), culture),
                              LinkCount = LinkDao.CountForMovie(Int64.Parse(ev.Element("id").Value.Trim(), culture))
                          }).ToList();

            return new ListBlock<MovieDetails>(result, startIndex, hasMore);
        }

        public XmlDocument SearchMoviesXml(string keywords, int startIndex, int count)
        {
            string xmlString;
            try
            {
                xmlString = MovieProxy.FindMovies(keywords, startIndex, count);
            }
            catch (Exception ex)
            {
                throw new NetworkException(ex);
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);

            return xml;
        }

        #endregion

    }
}
