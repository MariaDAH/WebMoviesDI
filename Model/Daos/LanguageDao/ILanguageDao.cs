using System.Collections.Generic;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.LanguageDao
{
    public interface ILanguageDao
        : IGenericDao<Language, string>
    {

        /// <summary>
        /// Get the list of all languages supported by the system
        /// </summary>
        /// <returns>The list of languages</returns>
        /// <exception cref="InstanceNotFoundException&lt;Language&gt;"/>
        List<Language> FindAll();

    }
}
