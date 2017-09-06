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
    /// Load the languages...
    /// </summary>
    public class Languages
    {

        private static ILocalizationService localizationService;

        public ILocalizationService LocalizationService
        {
            set { localizationService = value; }
        }

        private static readonly ArrayList languageCodes = new ArrayList();

        /* Access modifiers are not allowed on static constructors
         * so if we want to prevent that anybody creates instances 
         * of this class we must do the following ...
         */
        private Languages() { }

        static Languages()
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            localizationService = container.Resolve<ILocalizationService>();

            Dictionary<string, string> languages = localizationService.getAvailableLanguages();
            foreach (string code in languages.Keys)
            {
                languageCodes.Add(code);
            }
        }

        public static ICollection GetLaguageCodes()
        {
            return languageCodes;
        }

        public static SortedSet<ListItem> GetLanguages()
        {
            SortedSet<ListItem> languageData = new SortedSet<ListItem>(new AlphabeticalListItemTextComparer());
            foreach (string languageCode in languageCodes)
            {
                languageData.Add(new ListItem(Resources.Languages.ResourceManager.GetString(languageCode), languageCode));
            }

            return languageData;
        }

    }
}
