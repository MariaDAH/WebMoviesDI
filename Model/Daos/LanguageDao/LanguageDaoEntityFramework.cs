using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Es.Udc.DotNet.WebMovies.Model.Util.Dao;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Model.Daos.LanguageDao
{
    class LanguageDaoEntityFramework
        : GenericDaoEntityFramework<Language, string>, ILanguageDao
    {

        public LanguageDaoEntityFramework() { }

        public List<Language> FindAll()
        {
            ObjectSet<Language> languages = Context.CreateObjectSet<Language>();

            var result = (from l in languages.Include("Countries")
                          select l).ToList();

            if (result.Count == 0)
            {
                throw new InstanceNotFoundException<Language>("*", null);
            }

            return result;
        }

    }
}
