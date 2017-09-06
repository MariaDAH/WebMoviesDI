using System;
using Es.Udc.DotNet.WebMovies.Web.Http.Session;
using Microsoft.Practices.Unity;
using System.Web;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;

namespace Es.Udc.DotNet.WebMovies.Web.Pages.Favorite
{
    public partial class Favorite
        : SpecificCulturePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                long linkId = Int32.Parse(Request.Params.Get("linkId"));

                IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];

                ILinkService linkService = container.Resolve<ILinkService>();
                LinkDetails link = linkService.GetLink(linkId);
            }
            catch (Exception)
            {
            }
        }

    }
}
