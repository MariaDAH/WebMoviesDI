using System;
using System.IO;
using System.Net;
using System.Web;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.MovieProxy
{
    public class MovieProxy
        : IMovieProxy
    {

        public MovieProxy() { }

        #region IMovieProxy Members

        public string FindMovies(string keywords, int startIndex, int count)
        {
            // Create a remote URL
            string url = String.Format("http://" + Properties.Settings.Default.WebShop_sourceAddress + "/xml/FindProductsXml"
                + "?categoryId=" + Properties.Settings.Default.WebShop_moviesCategoryId
                + "&keywords=" + HttpUtility.UrlEncode(keywords)
                + "&startIndex=" + startIndex
                + "&count=" + count);
            // Create the web request to a Uniform Resource Identifier (URI)
            Uri addressUri = new Uri(url);

            // Create and send an HTTP request to the defined URI
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(addressUri);

            // Get response from the specified resource in the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Get the response stream to read the body response from server
            StreamReader reader = new StreamReader(response.GetResponseStream());

            // Read the whole contents and return it as a string
            return reader.ReadToEnd();
        }

        public string DetailMovie(long movieId)
        {
            // Create a remote URL
            string sourceUrl = string.Format("http://" + Properties.Settings.Default.WebShop_sourceAddress + "/xml/DetailProduct"
                + "?productId=" + movieId);

            // Create the web request to a Uniform Resource Identifier (URI)
            Uri addressUri = new Uri(sourceUrl);

            // Create and send an HTTP request to the defined URI
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(addressUri);

            // Get response from the specified resource in the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Get the response stream to read the body response from server
            StreamReader reader = new StreamReader(response.GetResponseStream());

            // Read the whole contents and return it as a string
            return reader.ReadToEnd();
        }

        #endregion

    }
}
