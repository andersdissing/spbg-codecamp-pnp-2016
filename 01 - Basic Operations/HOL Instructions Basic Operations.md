Preparations
------------
1. Start Visual Studio 2015 as administrator
2. Open PnPBasicOperations - Starter solution
3. Select the SharePoint add-in in solution explorer and change the Site URL in property window
4. Double click the AppManifest.xml file, switch to the Permissions tab and add Scope=Web | Permission=FullControl
5. Check the check-box 'Allow the app to make app-only calls to SharePoint.'
6. Deploy the add-in to SharePoint Online with CTRL-F5 (takes about 3 minutes)

Plain CSOM example
------------------
1. Replace code for Index view of the Home controller (Views/Home/Index.cshtml - snippet 1)
3. Run the code - CTRL-F5, start dev tools in browser (F12), go to network tab and click the button
4. Right click on Controllers folder, Add, Controller..., MVC5 Controller - Empty, name it 'CsomController'
6. Replace the code of CsomController with snippet 2
7. Replace URL to your SPO site in the pasted code
8. Run the code - CTRL-F5, click the button and check that list was created

PnP example
-----------
1. Get SharePointPnPCoreOnline via NuGet (name change, not OfficeDevPnP anymore, make sure to choose packages from Nuget – not Microsoft and .NET)
2. Replace code for Index view of the Home controller (Views/Home/Index.cshtml - snippet 3)
4. Right click on Controllers folder, Add, Controller..., MVC5 Controller - Empty, name it 'PnPController'
5. Replace the code of PnPController with snippet 4
6. Change the URL in PnPController for your SharePoint Online site
7. Run the code - CTRL-F5, click the button and check that list was created

Note on authencation methods
----------------------------
1. Remove query string parameters from button and run again
2. Set host filter in Fiddler: SharePoint online URL;localhost:44347
3. Close browser and go to https://localhost:44347/Csom/CreateList?SPHostUrl=[your SharePoint Online site URL] and login
4. Point-out: 1. SharePoint context filter requires query string parameters, 2. SharePoint context filter does redirect when needed (and removes custom query string parameters)
