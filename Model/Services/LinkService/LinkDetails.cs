using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model.Services.LinkService
{
    [Serializable()]
    public class LinkDetails
    {

        #region Properties Region

        /// <summary>
        /// ID for the link.
        /// </summary>
        /// <value>The link id.</value>
        public long LinkId { get; private set; }

        /// <summary>
        /// ID of the user who added the link.
        /// </summary>
        /// <value>The user ID.</value>
        public long UserId { get; private set; }

        /// <summary>
        /// Login name of the user who added the link.
        /// </summary>
        /// <value>The user name.</value>
        public string UserName { get; private set; }

        /// <summary>
        /// ID of the referenced movie.
        /// </summary>
        /// <value>The language code.</value>
        public long MovieId { get; private set; }

        /// <summary>
        /// Name of the link.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the link.
        /// </summary>
        /// <value>The description.</c></value>
        public string Description { get; private set; }

        /// <summary>
        /// URL of the link.
        /// </summary>
        /// <value>The URL.</c></value>
        public string Url { get; private set; }

        /// <summary>
        /// Rating of the link.
        /// </summary>
        /// <value>The comment.</c></value>
        public long Rating { get; private set; }

        /// <summary>
        /// If reported the report was already read.
        /// </summary>
        /// <value>The comment.</c></value>
        public bool ReportRead { get; private set; }

        /// <summary>
        /// Date of the addition of the link.
        /// </summary>
        /// <value>The date of addition.</value>
        public DateTime Date { get; private set; }

        #endregion
        
        public LinkDetails(long linkId, long userId, string userName, long movieId, string name, string description, string url, long rating, bool reportRead, DateTime date)
        {
            this.LinkId = linkId;
            this.UserId = userId;
            this.UserName = userName;
            this.MovieId = movieId;
            this.Name = name;
            this.Description = description;
            this.Url = url;
            this.Rating = rating;
            this.ReportRead = reportRead;
            this.Date = date;
        }

        public override bool Equals(object obj)
        {
            LinkDetails target = (LinkDetails) obj;

            return (this.LinkId == target.LinkId)
                   && (this.UserId == target.UserId)
                   && (this.UserName == target.UserName)
                   && (this.MovieId == target.MovieId)
                   && (this.Name == target.Name)
                   && (this.Description == target.Description)
                   && (this.Url == target.Url)
                   && (this.Rating == target.Rating)
                   && (this.ReportRead == target.ReportRead)
                   && (this.Date == target.Date);
        }

        // The GetHashCode method is used in hashing algorithms and data 
        // structures such as a hash table. In order to ensure that it works 
        // properly, it is based on a field that does not change.
        public override int GetHashCode()
        {
            return this.LinkId.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the 
        /// current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current 
        /// <see cref="T:System.Object"></see>.
        /// </returns>
        public override String ToString()
        {
            String strLoginResult = "[ linkId = " + this.LinkId + " | " +
                "UserId = " + this.UserId + " | " +
                "UserName = " + this.UserName + " | " +
                "MovieId = " + this.MovieId + " | " +
                "Name = " + this.Name + " | " +
                "Description = " + this.Description + " | " +
                "Url = " + this.Url + " | " +
                "Rating = " + this.Rating + " | " +
                "ReportRead = " + this.ReportRead + " | " +
                "Date = " + this.Date + " ]";

            return strLoginResult;
        }

    }
}
