using System;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Error
{
    public partial class InternalError
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UrlReferrer != null)
            {
                lnkReturn.NavigateUrl = Response.ApplyAppPathModifier(Request.UrlReferrer.AbsoluteUri);
            }
            else
            {
                lnkReturn.NavigateUrl = Response.ApplyAppPathModifier("/Pages/MainPage.aspx");
            }
        }

    }
}
