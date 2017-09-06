using System.Data.Objects;
using System.Linq;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao
{
    public class UserProfileDaoEntityFramework
        : GenericDaoEntityFramework<UserProfile, long>, IUserProfileDao
    {

        public UserProfileDaoEntityFramework() { }
        
        #region IUserProfileDao Members

        public UserProfile FindByLoginName(string loginName)
        {
            ObjectSet<UserProfile> userProfiles = Context.CreateObjectSet<UserProfile>();

            var result = (from u in userProfiles
                          where u.userLogin == loginName
                          select u);

            if (result.Count() == 0)
            {
                throw new InstanceNotFoundException<UserProfile>("loginName", loginName);
            }
            else if (result.Count() > 1)
            {
                throw new DuplicateInstanceException<UserProfile>("loginName", loginName);
            }

            return result.First();
        }
        
        public bool ExistsWithLoginName(string loginName)
        {
            ObjectSet<UserProfile> userProfiles = Context.CreateObjectSet<UserProfile>();

            var result = (from u in userProfiles
                          where u.userLogin == loginName
                          select u);

            if (result.Count() > 1)
            {
                throw new DuplicateInstanceException<UserProfile>("loginName", loginName);
            }

            return result.Count() != 0;
        }

        #endregion

    }
}
