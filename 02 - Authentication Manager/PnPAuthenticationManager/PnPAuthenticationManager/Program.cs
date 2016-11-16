using System;
using System.Security;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;

namespace PnPAuthenticationManager
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientContext clientContext = null;
            var siteUrl = "https://yoursite.sharepoint.com/subsite";

            // SharePoint Online - credentials
            //var username = "spo-user-name";
            //var password = GetSecureString("Password");
            //clientContext = new AuthenticationManager()
            //    .GetSharePointOnlineAuthenticatedContextTenant(siteUrl, username, password);

            // SharePont Online - interactive login
            //clientContext = new AuthenticationManager()
            //    .GetWebLoginClientContext(siteUrl);

            // SharePoint Online - app only
            //var clientId = "sharepoint-add-in-client-id";
            //var clientSecret = GetString("Client Secret");
            //clientContext = new AuthenticationManager()
            //    .GetAppOnlyAuthenticatedContext(siteUrl, clientId, clientSecret);

            // Azure AD - interactive
            //var clientId = "azure-ad-native-add-in-client-id";
            //clientContext = new AuthenticationManager()
            //    .GetAzureADNativeApplicationAuthenticatedContext(siteUrl, clientId, "https://someurl.com");

            // Azure AD - app only
            //var clientId = "azure-ad-web-add-in-client-id";
            //var pfxPassword = GetString("PFX password");
            //clientContext = new AuthenticationManager()
            //    .GetAzureADAppOnlyAuthenticatedContext(siteUrl, clientId,
            //        "sometenant.onmicrosoft.com",
            //        @"d:\spbg-codecamp-2016-08-16-master\02 - Authentication Manager\PnPCert.pfx", pfxPassword);

            var web = clientContext.Web;
            clientContext.Load(web, w => w.Title);
            clientContext.ExecuteQuery();
            Console.WriteLine("Site title: {0}", web.Title);
        }

        private static SecureString GetSecureString(string label)
        {
            var password = new SecureString();
            Console.Write("{0}: ", label);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace)
                {
                    password.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                else
                {
                    Console.Write("\b");
                }
            }

            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }

        private static string GetString(string label)
        {
            var password = "";
            Console.Write("{0}: ", label);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    Console.Write("\b");
                }
            }

            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            // Remove trailing return
            return password.Replace("\r", "");
        }
    }
}