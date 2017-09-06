using System.Collections.Generic;
using Es.Udc.DotNet.WebMovies.Model.Daos.CountryDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LanguageDao;
using Es.Udc.DotNet.WebMovies.Model.Util;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Model.Services.LocalizationService
{
    public class LocalizationService
        : ILocalizationService
    {

        [Dependency]
        public ILanguageDao LanguageDao { private get; set; }

        [Dependency]
        public ICountryDao CountryDao { private get; set; }

        public Dictionary<string, string> getAvailableLanguages()
        {
            Dictionary<string, string> availableLanguages = new Dictionary<string, string>();

            List<Language> languages;
            try
            {
                languages = LanguageDao.FindAll();
            }
            catch (InstanceNotFoundException<Language> ex)
            {
                throw new InternalErrorException(ex);
            }

            foreach (Language language in languages)
            {
                availableLanguages.Add(language.languageCode, language.languageName);
            }

            return availableLanguages;
        }

        public Dictionary<string, string> getAvailableCountries()
        {
            Dictionary<string, string> availableCountries = new Dictionary<string, string>();

            List<Country> countries;
            try
            {
                countries = CountryDao.FindAll();
            }
            catch (InstanceNotFoundException<Country> ex)
            {
                throw new InternalErrorException(ex);
            }

            foreach (Country country in countries)
            {
                availableCountries.Add(country.countryCode, country.countryName);
            }

            return availableCountries;
        }

        public List<Locale> getAvailableLocales()
        {
            List<Locale> availableLocales = new List<Locale>();

            List<Language> languages;
            try
            {
                languages = LanguageDao.FindAll();
            }
            catch (InstanceNotFoundException<Language> ex)
            {
                throw new InternalErrorException(ex);
            }

            foreach (Language language in languages)
            {
                foreach (Country country in language.Countries)
                {
                    availableLocales.Add(new Locale(language.languageCode, country.countryCode));
                }
            }

            return availableLocales;
        }

    }
}
