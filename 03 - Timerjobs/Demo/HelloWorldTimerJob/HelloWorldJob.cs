using System;
using OfficeDevPnP.Core.Framework.TimerJobs;

namespace HelloWorldTimerJob
{
    public class HelloWorldJob : TimerJob
    {
        public HelloWorldJob() : base("HelloWorldJob")
        {
            TimerJobRun += (sender, args) =>
            {
                try
                {
                    var context = args.WebClientContext;
                    context.Load(context.Web, w => w.Title);
                    context.ExecuteQuery();
                    var title = context.Web.Title;
                    Console.WriteLine($"Site title: {title}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error has occured: {ex.Message}");
                }
            };
        }
    }
}
