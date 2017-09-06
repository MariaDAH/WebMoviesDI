using System;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Es.Udc.DotNet.WebMovies.Web.Http.View.ApplicationObjects;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.User
{
    public partial class UpdateUserProfile
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserProfileDetails userProfileDetails = SessionManager.FindUserProfileDetails(Context);

                txtFirstName.Text = userProfileDetails.FirstName;
                txtSurname.Text = userProfileDetails.LastName;
                txtEmail.Text = userProfileDetails.Email;

                /* Combo box initialization */
                UpdateComboLanguage(userProfileDetails.LanguageCode);
                UpdateComboCountry(userProfileDetails.LanguageCode, userProfileDetails.CountryCode);
            }
        }

        /// <summary>
        /// Loads the languages in the comboBox in the *selectedLanguage*. 
        /// Also, the selectedLanguage will appear selected in the 
        /// ComboBox
        /// </summary>
        private void UpdateComboLanguage(String selectedLanguage)
        {
            comboLanguage.DataSource = Languages.GetLanguages();
            comboLanguage.DataTextField = "text";
            comboLanguage.DataValueField = "value";
            comboLanguage.DataBind();
            comboLanguage.SelectedValue = selectedLanguage;
        }

        /// <summary>
        /// Loads the countries in the comboBox in the *selectedLanguage*. 
        /// Also, the *selectedCountry* will appear selected in the 
        /// ComboBox
        /// </summary>
        private void UpdateComboCountry(String selectedLanguage, String selectedCountry)
        {
            comboCountry.DataSource = Countries.GetCountryData();
            comboCountry.DataTextField = "text";
            comboCountry.DataValueField = "value";
            comboCountry.DataBind();
            comboCountry.SelectedValue = selectedCountry;
        }

        /// <summary>
        /// Handles the Click event of the btnUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance 
        /// containing the event data.</param>
        protected void BtnUpdateClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SessionManager.UpdateUserProfileDetails(Context, txtFirstName.Text, txtSurname.Text, txtEmail.Text, comboLanguage.SelectedValue, comboCountry.SelectedValue);

                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/MainPage.aspx"));
            }
        }

        protected void ComboLanguageSelectedIndexChanged(object sender, EventArgs e)
        {
            /* After a language change, the countries are printed in the
             * correct language */
            UpdateComboCountry(comboLanguage.SelectedValue, comboCountry.SelectedValue);
        }

    }
}
