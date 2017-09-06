using System;
using System.Collections.Generic;

namespace Es.Udc.DotNet.WebMovies.Web.Http.Session
{
    public class UserSession
    {

        private long userProfileId;
        private string login;
        private string firstName;

        public long UserProfileId
        {
            get { return userProfileId; }
            set { userProfileId = value; }
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

    }
}
