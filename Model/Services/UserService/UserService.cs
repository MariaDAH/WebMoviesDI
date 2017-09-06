using System;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Util;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Model.Services.UserService
{
    public class UserService
        : IUserService
    {

        [Dependency]
        public IUserProfileDao UserProfileDao { private get; set; }
        
        #region IUserService Members

        public long Register(string loginName, string clearPassword, string firstName, string lastName, string email, string languageCode, string countryCode)
        {
            if (UserProfileDao.ExistsWithLoginName(loginName))
            {
                throw new DuplicateInstanceException<UserProfileDetails>("loginName", loginName);
            }

            UserProfile userProfile = UserProfile.CreateUserProfile(0, loginName, PasswordEncrypter.Crypt(clearPassword), firstName, lastName, email, languageCode, countryCode);
            try
            {
                UserProfileDao.Create(userProfile);
            }
            catch (DuplicateInstanceException<UserProfile> ex)
            {
                throw new DuplicateInstanceException<UserProfileDetails>(ex.Properties);
            }

            return userProfile.userId;
        }

        public LoginResult Login(string loginName, string password, bool passwordIsEncrypted)
        {
            UserProfile userProfile;
            try
            {
                userProfile = UserProfileDao.FindByLoginName(loginName);
            }
            catch (InstanceNotFoundException<UserProfile> ex)
            {
                throw new InstanceNotFoundException<UserProfileDetails>(ex.Properties);
            }
            catch (DuplicateInstanceException<UserProfile> ex)
            {
                throw new InternalErrorException(ex);
            }

            String storedPassword = userProfile.password;

            if (passwordIsEncrypted)
            {
                if (!password.Equals(storedPassword))
                {
                    throw new IncorrectPasswordException(loginName);
                }
            }
            else
            {
                if (!PasswordEncrypter.IsClearPasswordCorrect(password, storedPassword))
                {
                    throw new IncorrectPasswordException(loginName);
                }
            }

            return new LoginResult(userProfile.userId, userProfile.firstName, storedPassword, userProfile.languageCode, userProfile.countryCode);
        }

        public UserProfileDetails GetUserProfile(long userId)
        {
            UserProfile userProfile;
            try
            {
                userProfile = UserProfileDao.Find(userId);
            }
            catch (InstanceNotFoundException<UserProfile> ex)
            {
                throw new InstanceNotFoundException<UserProfileDetails>(ex.Properties);
            }

            return new UserProfileDetails(userProfile.userLogin, userProfile.firstName, userProfile.lastName, userProfile.email, userProfile.languageCode, userProfile.countryCode);
        }

        public void UpdateUserProfile(long userId, string firstName, string lastName, string email, string languageCode, string countryCode)
        {
            UserProfile userProfile;
            try
            {
                userProfile = UserProfileDao.Find(userId);
            }
            catch (InstanceNotFoundException<UserProfile> ex)
            {
                throw new InstanceNotFoundException<UserProfileDetails>(ex.Properties);
            }

            userProfile.firstName = firstName;
            userProfile.lastName = lastName;
            userProfile.email = email;
            userProfile.languageCode = languageCode;
            userProfile.countryCode = countryCode;

            try
            {
                UserProfileDao.Update(userProfile);
            }
            catch (InstanceNotFoundException<UserProfile> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        public void ChangePassword(long userId, string oldClearPassword, string newClearPassword)
        {
            UserProfile userProfile;
            try
            {
                userProfile = UserProfileDao.Find(userId);
            }
            catch (InstanceNotFoundException<UserProfile> ex)
            {
                throw new InstanceNotFoundException<UserProfileDetails>(ex.Properties);
            }

            String storedPassword = userProfile.password;

            if (!PasswordEncrypter.IsClearPasswordCorrect(oldClearPassword, storedPassword))
            {
                throw new IncorrectPasswordException(userProfile.userLogin);
            }

            userProfile.password = PasswordEncrypter.Crypt(newClearPassword);

            try
            {
                UserProfileDao.Update(userProfile);
            }
            catch (InstanceNotFoundException<UserProfile> ex)
            {
                throw new InternalErrorException(ex);
            }
        }

        #endregion

    }
}
