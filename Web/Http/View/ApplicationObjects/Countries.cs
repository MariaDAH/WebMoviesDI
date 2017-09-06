using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.WebMovies.Model.Services.LocalizationService;
using Es.Udc.DotNet.WebMovies.Web.Util;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.WebMovies.Web.Http.View.ApplicationObjects
{
    /// <summary>
    /// Load the countries...
    /// </summary>
    public class Countries
    {

        private static ILocalizationService localizationService;

        public ILocalizationService LocalizationService
        {
            set { localizationService = value; }
        }

        private static readonly ArrayList countryCodes = new ArrayList();

        /* Access modifiers are not allowed on static constructors
         * so if we want to prevent that anybody creates instances 
         * of this class we must do the following... */
        private Countries() { }

        static Countries()
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            localizationService = container.Resolve<ILocalizationService>();

            Dictionary<string, string> countries = localizationService.getAvailableCountries();
            foreach (string code in countries.Keys)
            {
                countryCodes.Add(code);
            }
        }

        public static ICollection GetCountryCodes()
        {
            return countryCodes;
        }

        /* This could have been calculated in static constructor but this way
         * allows to resolve against any culture instead of a fixed language. */
        public static SortedSet<ListItem> GetCountryData()
        {
            SortedSet<ListItem> countryData = new SortedSet<ListItem>(new AlphabeticalListItemTextComparer());
            foreach (string countryCode in countryCodes)
            {
                countryData.Add(new ListItem(Resources.Countries.ResourceManager.GetString(countryCode), countryCode));
            }

            return countryData;
        }

    }
}
