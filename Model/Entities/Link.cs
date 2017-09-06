using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model
{
    public partial class Link
    {

        public long Rating
        {
            get
            {
                long rating = 0;

                foreach (Rating rating_ in this.Ratings)
                {
                    rating += rating_.value;
                }

                return rating;
            }

            private set { }
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            Link target = (Link) obj;

            return (this.linkId == target.linkId)
                   && (this.userId == target.userId)
                   && (this.movieId == target.movieId)
                   && (this.name == target.name)
                   && (this.description == target.description)
                   && (this.url == target.url)
                   && (this.reportRead == target.reportRead)
                   && (this.date == target.date);
        }
        
        // The GetHashCode method is used in hashing algorithms and data 
        // structures such as a hash table. In order to ensure that it works 
        // properly, it is based on a field that does not change.
        public override int GetHashCode()
        {
            return this.linkId.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string strLink = "[ linkId = " + this.linkId + " | " +
                "userId = " + this.userId + " | " +
                "movieId = " + this.movieId + " | " +
                "name = " + this.name + " | " +
                "description = " + this.description + " | " +
                "url = " + this.url + " | " +
                "reportRead = " + this.reportRead + " | " +
                "date = " + this.date + " ]";

            return strLink;
        }

    }
}
