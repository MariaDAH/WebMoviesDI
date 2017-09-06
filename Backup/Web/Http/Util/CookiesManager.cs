using System;
using System.Web;
using System.Web.Security;

namespace Es.Udc.DotNet.WebMovies.Web.Http.Util
{
    public class CookiesManager
    {

        private const string LOGIN_NAME_COOKIE = "loginName";
        private const string ENCRYPTED_PASSWORD_COOKIE = "encryptedPassword";
        private const string PREFERRED_SEARCH_ENGINE_COOKIE = "preferredSearchEngine";

        private const int ONE_MONTH = 30 * 24 * 3600;
        private const int REMEMBER_MY_PASSWORD_TTL = ONE_MONTH;
        private const int PREFERRED_SEARCH_ENGINE_TTL = ONE_MONTH;
        private const int REMOVE_COOKIE_TTL = 0;

        public static void LeaveLoginCookie(HttpContext context, String loginName, String encryptedPassword)
        {
            /* Create the loginName cookie */
            HttpCookie loginNameCookie = new HttpCookie(LOGIN_NAME_COOKIE, loginName);

            /* Create the encryptedPassword cookie */
            HttpCookie encryptedPasswordCookie = new HttpCookie(ENCRYPTED_PASSWORD_COOKIE, encryptedPassword);

            /* Create the authentication ticket cookie */
            HttpCookie authTicket = FormsAuthentication.GetAuthCookie(loginName, true);

            /* Set maximum age to cookies */
            DateTime expirationTime = DateTime.Now.AddSeconds(REMEMBER_MY_PASSWORD_TTL);
            loginNameCookie.Expires = expirationTime;
            encryptedPasswordCookie.Expires = expirationTime;
            authTicket.Expires = expirationTime;

            /* Add cookies to response */
            context.Response.Cookies.Add(loginNameCookie);
            context.Response.Cookies.Add(encryptedPasswordCookie);
        }

        public static void LeavePreferredSearchEngineCookie(HttpContext context, String preferredSearchEngine)
        {
            /* Create the preferredSearchEngine cookie */
            HttpCookie preferredSearchEngineCookie = new HttpCookie(PREFERRED_SEARCH_ENGINE_COOKIE, preferredSearchEngine);

            /* Set maximum age to cookies */
            preferredSearchEngineCookie.Expires = DateTime.Now.AddSeconds(PREFERRED_SEARCH_ENGINE_TTL);

            /* Add cookies to response */
            context.Response.Cookies.Add(preferredSearchEngineCookie);
        }

        public static void RemoveLoginCookie(HttpContext context)
        {
            /* Create the loginName cookie */
            HttpCookie loginNameCookie = new HttpCookie(LOGIN_NAME_COOKIE, "");

            /* Create the encryptedPassword cookie */
            HttpCookie encryptedPasswordCookie = new HttpCookie(ENCRYPTED_PASSWORD_COOKIE, "");

            /* Set maximum age to cookies */
            DateTime expirationTime = DateTime.Now.AddSeconds(REMOVE_COOKIE_TTL);
            loginNameCookie.Expires = expirationTime;
            encryptedPasswordCookie.Expires = expirationTime;

            /* Add cookies to response */
            context.Response.Cookies.Add(loginNameCookie);
            context.Response.Cookies.Add(encryptedPasswordCookie);
        }

        public static void RemovePreferredSearchEngineCookie(HttpContext context)
        {
            /* Create the preferredSearchEngine cookie */
            HttpCookie preferredSearchEngineCookie = new HttpCookie(PREFERRED_SEARCH_ENGINE_COOKIE, "");

            /* Set maximum age to cookies */
            preferredSearchEngineCookie.Expires = DateTime.Now.AddSeconds(REMOVE_COOKIE_TTL);

            /* Add cookies to response */
            context.Response.Cookies.Add(preferredSearchEngineCookie);
        }

        public static String GetLoginName(HttpContext context)
        {
            HttpCookie loginNameCookie = context.Request.Cookies.Get(LOGIN_NAME_COOKIE);

            if (loginNameCookie == null)
            {
                return null;
            }

            return loginNameCookie.Value;
        }

        public static String GetEncryptedPassword(HttpContext context)
        {
            HttpCookie encryptedPasswordCookie = context.Request.Cookies.Get(ENCRYPTED_PASSWORD_COOKIE);

            if (encryptedPasswordCookie == null)
            {
                return null;
            }

            return encryptedPasswordCookie.Value;
        }

        public static String GetPreferredSearchEngine(HttpContext context)
        {
            HttpCookie preferredSearchEngineCookie = context.Request.Cookies.Get(PREFERRED_SEARCH_ENGINE_COOKIE);

            if (preferredSearchEngineCookie == null)
            {
                return null;
            }

            return preferredSearchEngineCookie.Value;
        }

    }
}
