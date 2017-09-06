using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.CountryDao
{
    public class CountryDaoEntityFramework
        : GenericDaoEntityFramework<Country, string>, ICountryDao
    {

        public CountryDaoEntityFramework() { }

        public List<Country> FindAll()
        {
            ObjectSet<Country> countries = Context.CreateObjectSet<Country>();

            var result = (from c in countries.Include("Languages")
                          select c).ToList();

            if (result.Count == 0)
            {
                throw new InstanceNotFoundException<Country>("*", null);
            }

            return result;
        }

    }
}
