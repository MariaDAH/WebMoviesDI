using System;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using System.Web.UI;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.User
{
    public partial class Logout
        : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionManager.Logout(Context);

            Response.Redirect("~/Pages/MainPage.aspx");
        }

    }
}
