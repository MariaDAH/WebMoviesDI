using System;
using System.Web.Security;
using Es.Udc.DotNet.WebMovies.Model;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.User
{
    public partial class Authentication
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            lblPasswordError.Visible = false;
            lblLoginError.Visible = false;

            var welcomePlaceHolder = Master.FindControl("ContentPlaceHolder_Welcome");
            if (welcomePlaceHolder != null)
            {
                var authenticateLink = welcomePlaceHolder.FindControl("lnkAuthenticate");
                if (authenticateLink != null)
                {
                    authenticateLink.Visible = false;

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

        /// <summary>
        /// Handles the Click event of the btnLogin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance 
        /// containing the event data.</param>
        protected void BtnLoginClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SessionManager.Login(Context, txtLogin.Text, txtPassword.Text, checkRememberPassword.Checked);

                    FormsAuthentication.RedirectFromLoginPage(txtLogin.Text, checkRememberPassword.Checked);
                }
                catch (InstanceNotFoundException<UserProfileDetails>)
                {
                    lblLoginError.Visible = true;
                }
                catch (IncorrectPasswordException)
                {
                    lblPasswordError.Visible = true;
                }
            }
        }

    }
}
