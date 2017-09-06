using System;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;

namespace Es.Udc.DotNet.WebMovies.Model.Services.UserService
{
    public interface IUserService
    {

        IUserProfileDao UserProfileDao { set; }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="clearPassword">The clear password.</param>
        /// <param name="userProfileDetails">The user profile details.</param>
        /// <exception cref="DuplicateInstanceException&lt;UserProfileDetails&gt;"/>
        [Transactional]
        long Register(string loginName, string clearPassword, string firstName, string lastName, string email, string languageCode, string countryCode);
        
        /// <summary>
        /// Logins the specified login name.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <param name="passwordIsEncrypted">if set to <c>true</c> [password is
        /// encrypted].</param>
        /// <returns>LoginResult</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="IncorrectPasswordException" />        
        [Transactional]
        LoginResult Login(String loginName, String password, Boolean passwordIsEncrypted);

        /// <summary>
        /// Finds the user profile details.
        /// </summary>
        /// <param name="userId">The user profile id.</param>
        /// <returns>The user profile details</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        [Transactional]
        UserProfileDetails GetUserProfile(long userId);
        
        /// <summary>
        /// Updates the user profile details.
        /// </summary>
        /// <param name="userId">The user profile id.</param>
        /// <param name="userProfileDetails">The user profile details.</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        [Transactional]
        void UpdateUserProfile(long userId, string firstName, string lastName, string email, string languageCode, string countryCode);
        
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userId">The user profile id.</param>
        /// <param name="oldClearPassword">The old clear password.</param>
        /// <param name="newClearPassword">The new clear password.</param>
        /// <exception cref="InstanceNotFoundException&lt;UserProfileDetails&gt;" />
        /// <exception cref="IncorrectPasswordException" />
        [Transactional]
        void ChangePassword(long userId, String oldClearPassword, String newClearPassword);

    }
}
