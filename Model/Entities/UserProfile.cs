using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model
{
    public partial class UserProfile
    {

        public long Rating
        {
            get
            {
                long rating = 0;

                foreach (Link link in this.Links)
                {
                    rating += link.Rating + 1; // We add one extra point just for having the link published
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
            UserProfile target = (UserProfile)obj;

            return (this.userId == target.userId)
                   && (this.userLogin == target.userLogin)
                   && (this.password == target.password)
                   && (this.firstName == target.firstName)
                   && (this.lastName == target.lastName)
                   && (this.email == target.email)
                   && (this.languageCode == target.languageCode)
                   && (this.countryCode == target.countryCode);
        }


        // The GetHashCode method is used in hashing algorithms and data 
        // structures such as a hash table. In order to ensure that it works 
        // properly, it is based on a field that does not change.
        public override int GetHashCode()
        {
            return this.userId.GetHashCode();
        }


        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string strUserProfile = "[ userId = " + this.userId + " | " +
                "userLogin = " + this.userLogin + " | " +
                "password = " + this.password + " | " +
                "firstName = " + this.firstName + " | " +
                "lastName = " + this.lastName + " | " +
                "email = " + this.email + " | " +
                "languageCode = " + this.languageCode + " | " +
                "countryCode = " + this.countryCode + " ]";

            return strUserProfile;
        }

    }
}
