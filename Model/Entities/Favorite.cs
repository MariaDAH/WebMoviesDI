using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model
{
    public partial class Favorite
    {

        public long Rating
        {
            get
            {
                return this.Link.Rating;
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
            Favorite target = (Favorite) obj;

            return (this.favoriteId == target.favoriteId)
                   && (this.userId == target.userId)
                   && (this.linkId == target.linkId)
                   && (this.name == target.name)
                   && (this.description == target.description)
                   && (this.date == target.date);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current UserProfile/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.favoriteId.GetHashCode();
        }
        
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string strFavorite = "[ favoriteId = " + this.favoriteId + " | " +
                "userId = " + this.userId + " | " +
                "linkId = " + this.linkId + " | " +
                "name = " + this.name + " | " +
                "description = " + this.description + " | " +
                "date = " + this.date + " ]";

            return strFavorite;
        }

    }
}
