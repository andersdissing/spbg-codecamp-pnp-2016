Snippet 1 - Views/Home/Index.cshtml
-----------------------------------
@{
    ViewBag.Title = "Home Page";
}

@section scripts {
    <script language="javascript">
        $(document).ready(function() {
            $("#thebutton").on("click", function(e) {
                e.preventDefault();

                $.ajax(
                {
                    // Append query string to the call-back
                    url: "/Csom/CreateList" + window.location.search,
                    // Important - disable caching for this get request
                    cache: false
                }).done(function() { alert("complete"); });
            });
        });
    </script>
}

<div class="jumbotron">
    <h2>Demo Buttons</h2>
    <p class="lead">
        <b>Basic Operations - </b>learn how to get started updating existing code.
    </p>
    <p>
        <a id="thebutton" href="#" class="btn btn-default btn-large">Execute the Example (CSOM)</a>
    </p>
</div>


Snippet 2 - Controllers/CsomController
--------------------------------------
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

Snippet 3 - Views/Home/Index.cshtml
-----------------------------------
@{
    ViewBag.Title = "Home Page";
}

@section scripts {
    <script language="javascript">
        $(document).ready(function ()
        {
            $("#thebutton").on("click", function (e)
            {
                e.preventDefault();

                $.ajax(
                {
                    // Append query string to the call-back
                    url: "/Csom/CreateList" + window.location.search,
                    // Important - disable caching for this get request
                    cache: false
                }).done(function () { alert("complete"); });
            });

            $("#thepnpbutton").on("click", function (e)
            {
                e.preventDefault();

                $.ajax(
                    {
                        url: "/PnP/CreateList",
                        // Important - disable caching for this get request
                        cache: false
                    }).done(function () { alert("complete"); });
            });
        });
    </script>
}

<div class="jumbotron">
    <h2>Demo Buttons</h2>
    <p class="lead">
        <b>Basic Operations - </b>learn how to get started updating existing code.
    </p>
    <p>
        <a id="thebutton" href="#" class="btn btn-default btn-large">Execute the Example (CSOM)</a>
    </p>
    <p>
        <a id="thepnpbutton" href="#" class="btn btn-primary btn-large">Execute the Example (PnP)</a>
    </p>
</div>

Snippet 4 - Controllers/PnPController
-------------------------------------
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
            var clientContext = authManager.GetAppOnlyAuthenticatedContext("https://[your-url-here]", ConfigurationManager.AppSettings["ClientId"], ConfigurationManager.AppSettings["ClientSecret"]);

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