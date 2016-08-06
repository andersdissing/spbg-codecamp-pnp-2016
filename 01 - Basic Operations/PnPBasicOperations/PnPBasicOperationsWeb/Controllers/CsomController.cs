using System;
using System.Web.Mvc;
using Microsoft.SharePoint.Client;

namespace PnPBasicOperationsWeb.Controllers
{
    public class CsomController : Controller
    {
        [SharePointContextFilter]
        public ActionResult CreateList()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            using (var clientContext = spContext.CreateAppOnlyClientContextForSPHost())
            {
                // Create list on host web
                var list = clientContext.Web.Lists.Add(new ListCreationInformation
                {
                    Title = "CSOM",
                    Url = "lists/csom",
                    TemplateType = (int)ListTemplateType.GenericList
                });
                clientContext.Load(list);
                clientContext.ExecuteQuery();

                // Add a view to list
                var view = list.Views.Add(new ViewCreationInformation
                {
                    Title = "New View",
                    ViewTypeKind = Microsoft.SharePoint.Client.ViewType.Html,
                    RowLimit = 10,
                    ViewFields = new[] { "Title", "Modified" },
                    SetAsDefaultView = true,
                    Paged = false
                });
                clientContext.Load(view);
                clientContext.ExecuteQuery();

                // Set a property bag value
                var properties = clientContext.Web.AllProperties;
                clientContext.Load(properties);
                clientContext.ExecuteQuery();
                properties["ourwebkey"] = Guid.NewGuid().ToString();
                clientContext.Web.Update();
                clientContext.ExecuteQuery();
            }

            return new EmptyResult();
        }
    }
}