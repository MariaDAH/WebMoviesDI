using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model
{
    public partial class Label
    {
        
        public long Rating
        {
            get
            {
                //long rating = 0;

                //foreach (Link link in this.Links)
                //{
                //    rating += link.Rating + 1; // We add one extra point just for having the label
                //}

                //return rating;

                return this.Links.Count;
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
            Label target = (Label) obj;

            return (this.labelId == target.labelId)
                   && (this.text == target.text);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current UserProfile/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.labelId.GetHashCode();
        }
        
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string strLabel = "[ labelId = " + this.labelId + " | " +
                "text = " + this.text + " ]";

            return strLabel;
        }

    }
}
