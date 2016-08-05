
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeDevPnP.Core.Framework.TimerJobs;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Diagnostics;
using System.Threading;

namespace SPBGTimerjob
{
    public class SimpleJob : TimerJob
    {
        private const string SiteGovernanceJobKey = "sitegovernance";
        public SimpleJob() : base("SimpleJob") {
            TimerJobRun += SimpleJob_TimerJobRun;
        }
        private string BuildJavaScriptUrl(string siteUrl, string library)
        {
            // Solve root site collection URL 
            Uri url = new Uri(siteUrl);
            string jsUrl = String.Format("{0}/{1}", url.AbsoluteUri, library);
            // Unique rev generated each time JS is added, so that we force browsers to refresh JS file wiht latest version 
            string revision = Guid.NewGuid().ToString().Replace("-", "");
            return string.Format("{0}/{1}?rev={2}", jsUrl, "sitegovernance.js", revision);
        }

        private void SimpleJob_TimerJobRun(object sender, TimerJobRunEventArgs e)
        {
            try
            {
                string library = "";
                
                // Get the number of admins
                var admins = e.WebClientContext.Web.GetAdministrators();

                Log.Info("SiteGovernanceJob", "ThreadID = {2} | Site {0} has {1} administrators.", e.Url, admins.Count, Thread.CurrentThread.ManagedThreadId);

                // grab reference to list
                library = "SiteAssets";
                List list = e.WebClientContext.Web.GetListByUrl(library);

                if (!e.GetProperty("ScriptFileVersion").Equals("1.0", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (list == null)
                    {
                        // grab reference to list
                        library = "Style%20Library";
                        list = e.WebClientContext.Web.GetListByUrl(library);
                    }

                    if (list != null)
                    {
                        // upload js file to list
                        list.RootFolder.UploadFile("sitegovernance.js", "sitegovernance.js", true);

                        e.SetProperty("ScriptFileVersion", "1.0");
                    }
                }

                if (admins.Count < 2)
                {
                    // Oops, we need at least 2 site collection administrators
                    e.WebClientContext.Site.AddJsLink(SiteGovernanceJobKey, BuildJavaScriptUrl(e.Url, library));
                    Console.WriteLine("Site {0} marked as incompliant!", e.Url);
                    e.SetProperty("SiteCompliant", "false");
                }
                else
                {
                    // We're all good...let's remove the notification
                    e.WebClientContext.Site.DeleteJsLink(SiteGovernanceJobKey);
                    Console.WriteLine("Site {0} is compliant", e.Url);
                    e.SetProperty("SiteCompliant", "true");
                }

                e.CurrentRunSuccessful = true;
                e.DeleteProperty("LastError");
            }
            catch (Exception ex)
            {
                Log.Error("SiteGovernanceJob", "Error while processing site {0}. Error = {1}", e.Url, ex.Message);
                e.CurrentRunSuccessful = false;
                e.SetProperty("LastError", ex.Message);
            }
        }
    }
}
