using System;
using System.Globalization;
using Es.Udc.DotNet.ModelUtil.Log;
using Es.Udc.DotNet.WebMovies.Model;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Es.Udc.DotNet.WebMovies.Web.Http.View.ApplicationObjects;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.User
{
    public partial class Register
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginError.Visible = false;

            if (!IsPostBack)
            {
                /* Get current language and country from browser */
                String defaultLanguage = GetLanguageFromBrowserPreferences();
                String defaultCountry = GetCountryFromBrowserPreferences();

                /* Combo box initialization */
                UpdateComboLanguage(defaultLanguage);
                UpdateComboCountry(defaultLanguage, defaultCountry);
            }

            var welcomePlaceHolder = Master.FindControl("ContentPlaceHolder_Welcome");
            if (welcomePlaceHolder != null)
            {
                var registerLink = welcomePlaceHolder.FindControl("lnkRegister");
                if (registerLink != null)
                {
                    registerLink.Visible = false;

                    var welcomeDash = welcomePlaceHolder.FindControl("lblDashWelcome");
                    if (welcomeDash != null)
                    {
                        welcomeDash.Visible = false;
                    }
                }
            }
            var linksPlaceHolder = Master.FindControl("ContentPlaceHolder_Links");
            if (linksPlaceHolder != null)
            {
                var welcomeLabel = linksPlaceHolder.FindControl("lblWelcome");
                if (welcomeLabel != null)
                {
                    welcomeLabel.Visible = false;
                }
            }
        }

        private String GetLanguageFromBrowserPreferences()
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(Request.UserLanguages[0]);

            String language = cultureInfo.TwoLetterISOLanguageName;

            LogManager.RecordMessage("Preferred language of user" + " (based on browser preferences): " + language);

            return language;
        }

        private String GetCountryFromBrowserPreferences()
        {
            String country;
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(Request.UserLanguages[0]);

            if (cultureInfo.IsNeutralCulture)
            {
                country = "";
            }
            else
            {
                String cultureInfoName = cultureInfo.Name; // cultureInfoName is something like en-US
                country = cultureInfoName.Substring(cultureInfoName.Length - 2); // Gets the last two caracters of cultureInfoname

                LogManager.RecordMessage("Preferred region/country of user " + "(based on browser preferences): " + country);
            }

            return country;
        }

        /// <summary>
        /// Loads the languages in the comboBox in the *selectedLanguage*. 
        /// Also, the selectedLanguage will appear selected in the 
        /// ComboBox
        /// </summary>
        private void UpdateComboLanguage(String selectedLanguage)
        {
            this.comboLanguage.DataSource = Languages.GetLanguages();
            this.comboLanguage.DataTextField = "text";
            this.comboLanguage.DataValueField = "value";
            this.comboLanguage.DataBind();
            this.comboLanguage.SelectedValue = selectedLanguage;
        }

        /// <summary>
        /// Loads the countries in the comboBox in the *selectedLanguage*. 
        /// Also, the *selectedCountry* will appear selected in the 
        /// ComboBox
        /// </summary>
        private void UpdateComboCountry(String selectedLanguage, String selectedCountry)
        {
            this.comboCountry.DataSource = Countries.GetCountryData();
            this.comboCountry.DataTextField = "text";
            this.comboCountry.DataValueField = "value";
            this.comboCountry.DataBind();
            this.comboCountry.SelectedValue = selectedCountry;
        }

        /// <summary>
        /// Handles the Click event of the btnRegister control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance 
        /// containing the event data.</param>
        protected void BtnRegisterClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SessionManager.RegisterUser(Context, txtLogin.Text, txtPassword.Text, txtFirstName.Text, txtSurname.Text, txtEmail.Text, comboLanguage.SelectedValue, comboCountry.SelectedValue);

                    Response.Redirect(Response.ApplyAppPathModifier("~/Pages/MainPage.aspx"));
                }
                catch (DuplicateInstanceException<UserProfileDetails>)
                {
                    lblLoginError.Visible = true;
                }
            }
        }

        protected void ComboLanguageSelectedIndexChanged(object sender, EventArgs e)
        {
            /* After a language change, the countries are printed in the
             * correct language */
            this.UpdateComboCountry(comboLanguage.SelectedValue, comboCountry.SelectedValue);
        }

    }
}
