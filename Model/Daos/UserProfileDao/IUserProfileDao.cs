using System;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao
{
    public interface IUserProfileDao
        : IGenericDao<UserProfile, long>
    {

        /// <summary>
        /// Finds a UserProfile by loginName
        /// </summary>
        /// <param name="loginName">loginName</param>
        /// <returns>The UserProfile</returns>
        /// <exception cref="InstanceNotFoundException&lt;UserProfile&gt;"/>
        /// <exception cref="DuplicateInstanceException&lt;UserProfile&gt;"/>
        UserProfile FindByLoginName(String loginName);
        
        /// <summary>
        /// Checks if a UserProfile exists with a given loginName
        /// </summary>
        /// <param name="loginName">loginName</param>
        /// <returns>True if the user already exists</returns>
        /// <exception cref="DuplicateInstanceException&lt;UserProfile&gt;"/>
        bool ExistsWithLoginName(string loginName);

    }
}
