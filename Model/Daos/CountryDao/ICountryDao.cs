using System.Collections.Generic;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.CountryDao
{
    public interface ICountryDao
        : IGenericDao<Country, string>
    {

        /// <summary>
        /// Get the list of all countries supported by the system
        /// </summary>
        /// <returns>The list of countries</returns>
        /// <exception cref="InstanceNotFoundException&lt;Country&gt;"/>
        List<Country> FindAll();

    }
}
