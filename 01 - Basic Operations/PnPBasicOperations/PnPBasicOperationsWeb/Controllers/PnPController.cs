using System;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;

namespace PnPBasicOperationsWeb.Controllers
{
    public class PnPController : Controller
    {
        public ActionResult CreateList()
        {
            var authManager = new AuthenticationManager();
            var clientContext = authManager.GetAppOnlyAuthenticatedContext("https://rickenberg.sharepoint.com/codecamp2016", ConfigurationManager.AppSettings["ClientId"], ConfigurationManager.AppSettings["ClientSecret"]);

            using (clientContext)
            {

                // Create list on host web
                var list = clientContext.Web.CreateList(ListTemplateType.GenericList, "PnP", false);

                // Add a view to list
                list.CreateView("New View", Microsoft.SharePoint.Client.ViewType.Html, new[] { "Title", "Modified" }, 10, true);

                // Set a property bag value
                clientContext.Web.SetPropertyBagValue("ourwebkey", Guid.NewGuid().ToString());
            }

            return new EmptyResult();
        }
    }
}