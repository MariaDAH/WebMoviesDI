using System;
using System.Web;
using System.Web.Security;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using Es.Udc.DotNet.WebMovies.Model.Util;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Util;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Web.Http.Session
{
    ///<summary>
    ///
    /// A facade utility class to transparently manage session objects and 
    /// cookies. The following objects are mantained in the session:
    /// <list>
    ///   <item>* The user's data, stored in the UserSession object under the key 
    ///    <c>USER_SESSION_ATTRIBUTE</c>. This attribute is only present 
    ///    for authenticated users, and is only of interest to the view. The
    ///    UserSession object stores the firstName and the userProfileId</item>
    ///    
    ///   <item>* The user's language and the user's country, stored in the
    ///    Locale object under the key <c>LOCALE_SESSION_ATTRIBUTE</c>. This 
    ///    attribute is only present for authenticated users, and is only of 
    ///    interest to the view.</item>
    /// </list>
    /// 
    /// For users that select "remember my password" in the login wizard, the
    /// following cookies are used (managed in the CookiesManager object):
    /// <list>
    ///   <item>* <c>LOGIN_NAME_COOKIE</c>: to store the login name.</item>
    ///   <item>* <c>ENCRYPTED_PASSWORD_COOKIE</c>: to store the 
    ///    encrypted password.</item>
    ///   <item>* <c>.ASPXAUTH</c>: Authentication Cookie.</item>
    /// </list>
    /// In order to make transparent the management of session objects and cookies
    /// to the implementation of controller actions, this class provides a number
    /// of methods equivalent to some of the ones provided by 
    /// <c>UserService</c>, which manage session objects and cookies,
    /// and call upon the corresponding <c>UserService's</c> method.
    /// These methods are as follows:
    /// <list>
    ///   <item>* <c>Login</c>.</item>
    ///   <item>* <c>RegisterUser</c>.</item>
    ///   <item>* <c>FindUserProfileDetails</c>.</item>
    ///   <item>* <c>UpdateUserProfileDetails</c>.</item>
    ///   <item>* <c>ChangePassword</c>.</item>
    /// </list>
    /// 
    /// It is important to remember that when needing to do some of the above
    /// actions from the controller, the corresponding method of this class
    /// (one of the previous list) must be called, and <b>not</b> the one in
    /// <c>UserService</c>. The rest of methods of <c>UserService</c> must be 
    /// called directly. 
    /// 
    /// For example, in a personalizable portal,<c>UserService</c> could include:
    /// 
    ///   <c>findServicePreferences</c>, <c>updateServicePreferences</c>,
    ///   <c>findLayout</c>, <c>updateLayout</c>, etc. 
    /// 
    /// In a electronic commerce shop <c>UserFacadeDelegate</c> could include:
    /// 
    ///   <c>getShoppingCart</c>, <c>addToShoppingCart</c>,
    ///   <c>removeFromShoppingCart</c>, <c>makeOrder</c>,
    ///   <c>findPendingOrders</c>, etc. 
    ///   
    /// When needing to invoke directly a method of <c>UserSession</c>, the
    /// property <c>SessionManager.UserService</c> must be invoked in order
    /// to get the personal delegate (each user has his/her own delegate).
    ///  
    /// Same as <c>UserService</c>, there exist some logical 
    /// restrictions with regard to the order of method calling. In particular, 
    /// <c>updateUserProfileDetails</c> and <c>changePassword</c>
    /// can not be called if <c>login</c> or <c>registerUser</c> 
    /// have not been previously called. After the user calls one of these two 
    /// methods, the user is said to be authenticated.
    /// </summary>
    public class SessionManager
    {

        public static readonly String LOCALE_SESSION_ATTRIBUTE = "locale";

        public static readonly String USER_SESSION_ATTRIBUTE = "userSession";

        private static IUserService userService;

        public IUserService UserService
        {
            set { userService = value; }
        }

        static SessionManager()
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            userService = container.Resolve<IUserService>();
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="context">Http Context includes request, response, etc.</param>
        /// <param name="loginName">Username</param>
        /// <param name="clearPassword">Password in clear text</param>
        /// <param name="userProfileDetails">The user profile details.</param>
        /// <exception cref="DuplicateInstanceException&lt;UserProfileDetails&gt;"/>
        public static void RegisterUser(HttpContext context, String loginName, String clearPassword, string firstName, string lastName, string email, string languageCode, string countryCode)
        {
            /* Register user */
            long userId = userService.Register(loginName, clearPassword, firstName, lastName, email, languageCode, countryCode);

            /* Insert necessary objects in the session */
            UserSession userSession = new UserSession();
            userSession.UserProfileId = userId;
            userSession.Login = loginName;
            userSession.FirstName = firstName;

            Locale locale = new Locale(languageCode, countryCode);

            UpdateSessionForAuthenticatedUser(context, userSession, locale);

            FormsAuthentication.SetAuthCookie(loginName, false);
        }

        /// <summary>
        /// Login method. Authenticates an user in the current context.
        /// </summary>
        /// <param name="context">Http Context includes request, response, etc.</param>
        /// <param name="loginName">Username</param>
        /// <param name="clearPassword">Password in clear text</param>
        /// <param name="rememberMyPassword">Remember password to the next logins</param>
        /// <exception cref="IncorrectPasswordException"/>
        /// <exception cref="InstanceNotFoundException&lt;UserProfile&gt;"/>
        public static void Login(HttpContext context, String loginName, String clearPassword, Boolean rememberMyPassword)
        {
            /* Try to login, and if successful, update session with the necessary
             * objects for an authenticated user */
            LoginResult loginResult = DoLogin(context, loginName, clearPassword, false, rememberMyPassword);

            /* Add cookies if requested */
            if (rememberMyPassword)
            {
                CookiesManager.LeaveLoginCookie(context, loginName, loginResult.EncryptedPassword); 
            }
        }
        
        /// <summary>
        /// Tries to log in with the corresponding method of  
        /// <c>UserService</c>, and if successful, inserts in the
        /// session the necessary objects for an authenticated user.
        /// </summary>
        /// <param name="context">Http Context includes request, response, etc.</param>
        /// <param name="loginName">Username</param>
        /// <param name="password">User Password</param>
        /// <param name="passwordIsEncrypted">Password is either encrypted or 
        /// in clear text</param>
        /// <param name="rememberMyPassword">Remember password to the next 
        /// logins</param>
        /// <exception cref="IncorrectPasswordException"/>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;"/>        
        private static LoginResult DoLogin(HttpContext context, String loginName, String password, Boolean passwordIsEncrypted, Boolean rememberMyPassword)
        {
            LoginResult loginResult = userService.Login(loginName, password, passwordIsEncrypted);

            /* Insert necessary objects in the session */
            UserSession userSession = new UserSession();
            userSession.UserProfileId = loginResult.UserId;
            userSession.Login = loginName;
            userSession.FirstName = loginResult.FirstName;

            Locale locale = new Locale(loginResult.Language, loginResult.Country);

            UpdateSessionForAuthenticatedUser(context, userSession, locale);

            return loginResult;
        }

        /// <summary>
        /// Updates the session values for an previously authenticated user
        /// </summary>
        /// <param name="context">Http Context includes request, response, etc.</param>
        /// <param name="userSession">The user data stored in session.</param>
        /// <param name="locale">The locale info.</param>
        private static void UpdateSessionForAuthenticatedUser(HttpContext context, UserSession userSession, Locale locale)
        {
            /* Insert objects in session */
            context.Session.Add(USER_SESSION_ATTRIBUTE, userSession);
            context.Session.Add(LOCALE_SESSION_ATTRIBUTE, locale);
        }

        /// <summary>
        /// Determine if a user is authenticated
        /// </summary>
        /// <param name="context">Http Context includes request, response, etc.</param>
        /// <returns>
        ///   <c>true</c> if is user authenticated
        ///   <c>false</c> otherwise
        /// </returns>
        public static Boolean IsUserAuthenticated(HttpContext context)
        {
            if (context.Session == null)
            {
                return false;
            }

            return (context.Session[USER_SESSION_ATTRIBUTE] != null);
        }
        
        public static Locale GetLocale(HttpContext context)
        {
            Locale locale = (Locale)context.Session[LOCALE_SESSION_ATTRIBUTE];

            return locale;
        }

        /// <summary>
        /// Updates the user profile details.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userProfileDetails">The user profile details.</param>
        public static void UpdateUserProfileDetails(HttpContext context, string firstName, string lastName, string email, string languageCode, string countryCode)
        {
            /* Update user's profile details */
            UserSession userSession = (UserSession)context.Session[USER_SESSION_ATTRIBUTE];           

            userService.UpdateUserProfile(userSession.UserProfileId, firstName, lastName, email, languageCode, countryCode);

            /* Update user's session objects */
            Locale locale = new Locale(languageCode, countryCode);

            userSession.FirstName = firstName;

            UpdateSessionForAuthenticatedUser(context, userSession, locale);
        }

        /// <summary>
        /// Finds the user profile with the id stored in the session.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static UserProfileDetails FindUserProfileDetails(HttpContext context)
        {
            UserSession userSession = (UserSession)context.Session[USER_SESSION_ATTRIBUTE];

            UserProfileDetails userProfileDetails = userService.GetUserProfile(userSession.UserProfileId);

            return userProfileDetails;
        }
        
        /// <summary>
        /// Gets the user info stored in the session.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static UserSession GetUserSession(HttpContext context)
        {
            if (IsUserAuthenticated(context))
            {
                return (UserSession)context.Session[USER_SESSION_ATTRIBUTE];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Changes the user's password
        /// </summary>
        /// <param name="context">Http Context includes request, response, etc.</param>
        /// <param name="oldClearPassword">The old password in clear text</param>
        /// <param name="newClearPassword">The new password in clear text</param>
        /// <exception cref="IncorrectPasswordException"/>
        public static void ChangePassword(HttpContext context, String oldClearPassword, String newClearPassword)
        {
            UserSession userSession = (UserSession)context.Session[USER_SESSION_ATTRIBUTE];

            userService.ChangePassword(userSession.UserProfileId, oldClearPassword, newClearPassword);

            /* Remove cookies */
            CookiesManager.RemoveLoginCookie(context);
        }

        /// <summary>
        /// Destroys the session, and removes the cookies if the user had 
        /// selected "remember my password".
        /// </summary>
        /// <param name="context">Http Context includes request, response, etc.</param>
        public static void Logout(HttpContext context)
        {
            /* Remove cookies */
            CookiesManager.RemoveLoginCookie(context);

            /* Invalidate session */
            context.Session.Abandon();

            /* Invalidate Authentication Ticket */
            FormsAuthentication.SignOut();
        }
        
        /// <sumary>
        /// Guarantees that the session will have the necessary objects if the
        /// user has been authenticated or had selected "remember my password"
        /// in the past.
        /// </sumary> 
        /// <param name="context">Http Context includes request, response, etc.</param>       
        public static void TouchSession(HttpContext context)
        {
            /* Check if "UserSession" object is in the session */
            UserSession userSession = null;

            if (context.Session != null)
            {
                userSession = (UserSession) context.Session[USER_SESSION_ATTRIBUTE];

                if (userSession != null) // If userSession object is in the session, nothing should be doing
                {
                    return;
                }
            }

            /* The user had not been authenticated or his/her session has 
             * expired. We need to check if the user has selected "remember my 
             * password" in the last login (login name and password will come 
             * as cookies). If so, we reconstruct user's session objects */
            UpdateSessionFromCookies(context);
        }

        /// <summary>
        /// Tries to login (inserting necessary objects in the session) by using
        /// cookies (if present).
        /// </summary>
        /// <param name="context">Http Context includes request, response, etc.</param>        
        private static void UpdateSessionFromCookies(HttpContext context)
        {
            HttpRequest request = context.Request;                        
            if (request.Cookies == null)
            {
                return;
            }

            /* Check if the login name and the encrypted password come as cookies */
            String loginName = CookiesManager.GetLoginName(context);
            String encryptedPassword = CookiesManager.GetEncryptedPassword(context);

            if ((loginName == null) || (encryptedPassword == null))
            {
                return;
            }

            /* If loginName and encryptedPassword have valid values (the user selected "remember 
             * my password" option) try to login, and if successful, update session with the 
             * necessary objects for an authenticated user */
            try
            {
                DoLogin(context, loginName, encryptedPassword, true, true);

                /* Authentication Ticket */
                FormsAuthentication.SetAuthCookie(loginName, true);
            }
            catch (Exception) // Incorrect loginName or encryptedPassword
            {
                return;
            }
        }

    }
}
