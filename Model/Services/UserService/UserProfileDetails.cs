using System;

namespace Es.Udc.DotNet.WebMovies.Model.Services.UserService
{
    /// <summary>
    /// Class which contains the user details
    /// </summary>
    [Serializable()]
    public class UserProfileDetails
    {

        #region Properties Region

        public String LoginName { get; private set; }

        public String FirstName { get; private set; }

        public String LastName { get; private set; }

        public String Email { get; private set; }

        public string LanguageCode { get; private set; }

        public string CountryCode { get; private set; }

        #endregion
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileDetails"/>
        /// class.
        /// </summary>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="language">The language.</param>
        /// <param name="country">The country.</param>
        public UserProfileDetails(String loginName, String firstName, String lastName, String email, String language, String country)
        {
            this.LoginName = loginName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.LanguageCode = language;
            this.CountryCode = country;
        }

        public override bool Equals(object obj)
        {
            UserProfileDetails target = (UserProfileDetails) obj;

            return (this.LoginName == target.LoginName)
                  && (this.FirstName == target.FirstName)
                  && (this.LastName == target.LastName)
                  && (this.Email == target.Email)
                  && (this.LanguageCode == target.LanguageCode)
                  && (this.CountryCode == target.CountryCode);
        }
        
        // The GetHashCode method is used in hashing algorithms and data 
        // structures such as a hash table. In order to ensure that it works 
        // properly, we suppose that the FirstName does not change.        
        public override int GetHashCode()
        {
            return this.LoginName.GetHashCode();
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
            String strUserProfileDetails = "[ LoginName = " + LoginName + " | " +
                "FirstName = " + FirstName + " | " +
                "LastName = " + LastName + " | " +
                "Email = " + Email + " | " +
                "LanguageCode = " + LanguageCode + " | " +
                "CountryCode = " + CountryCode + " ]";

            return strUserProfileDetails;
        }

    }
}
