using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.WebMovies.Model.Daos.CountryDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LanguageDao;
using Es.Udc.DotNet.WebMovies.Model.Util;

namespace Es.Udc.DotNet.WebMovies.Model.Services.LocalizationService
{
    public interface ILocalizationService
    {

        ILanguageDao LanguageDao { set; }

        ICountryDao CountryDao { set; }

        [Transactional]
        Dictionary<string, string> getAvailableLanguages();

        [Transactional]
        Dictionary<string, string> getAvailableCountries();

        [Transactional]
        List<Locale> getAvailableLocales();

    }
}
