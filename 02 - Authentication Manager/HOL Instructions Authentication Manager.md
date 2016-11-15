Preparations
------------
1. Start Visual Studio 2015
2. Open PnPAuthenticationManager solution
3. Restore NuGet packages: SharePointPnPCoreOnline and  Microsoft.IdentityModel.Clients.ActiveDirectory v2.28 (important!) aka ADAL
5. Build the solution
6. Change the variable siteUrl to match your SharePoint Online target site

SharePoint Online hard-coded credentials
----------------------------------------
1. Uncomment the first block
2. Set the variable username with you SharePoint Online user name
3. Run the application with CTRL-F5

SharePoint Online interactive login
-----------------------------------
1. Comment the first block again
2. Uncomment the second block
3. Run the application with CTRL-F5

SharePoint Online app only
--------------------------
1. Comment the second block
2. Uncomment the third block
3. Go to https://yoursite.sharepoint.com/subsite/_layouts/appregnew.aspx and manually create an app principal, note the client ID and client secret
	Client Id:  	xxxxx
	Client Secret:  xxxxx
	Title:  	Demo
	App Domain:  	demo.contoso.com
	Redirect URI:  	https://demo.contoso.com/default.aspx
4. https://yoursite.sharepoint.com/subsite/_layouts/15/appinv.aspx, copy&paste the client id from previous step into the app id input field, click the Lookup button and copy&paste the code snippet 1 into the App Permission Request XML input box
5. Run the application with CTRL-F5, enter the client secrect from step 3 when prompted

Azure AD interactive login (native add-in)
------------------------------------------
1. Comment the third block
2. Uncomment the fourth block
3. Go to the new azure portal at portal.azure.com and login with the SharePoint Online tenant administrator (the account you used to create the Office365 tenant with)
4. Go to Azure Active Directory management
5. Click on App Registrations
6. Click Add and fill-out form and click Create button:
   - Name: Demo1
   - Application Type: Native
   - Redirect URI: https://someurl.com
7. Click Demo1 app / Required permissions / click Add:
   - Select API: Office 365 SharePoint Online and click Select
   - Select Permissions: Read items in all site collections and click Select
8. Click Done
9. When the app was created, copy the application id and paste it into appId variable in the code
10. Run the application with CTRL-F5, login with your normal SharePoint Online user account
11. Change the URL https://somerurl.com in the code and repeat step 10.
12. Inspect the tiny (but good) error message in the bottom of the login page

Azure AD app only (web add-in)
------------------------------
1. Comment the fourth block
2. Uncomment the fifth block
3. Go to the azure classic portal at manage.windowsazure.com and login with the SharePoint Online tenant administrator (the account you used to create the Office365 tenant with)
   Note: This does currently not work in the new portal, that's why we use the classic portal
4. Go to the AD extension, click the Applications tab, change the drop-down to Application my company owns
5. Click the Add button in the footer, Add an application my organization is developing:
   - Name: Demo2
   - Type: Web application
   - Sign-on URL: https://demo.yoursite.com/login
   - App id URI: https://demo.yoursite.com
6. When the application was created, go to the Configure tab and copy the client id into the appId variable in Program.cs
7. Start a PowerShell prompt as administrator
8. Create a self-signed certificate with the PowerShell script provided in the hand outs and enter a password when prompted (code snippet 2): 
   .\Create-SelfSignedCertificate.ps1 -CommonName "PnPCert" -StartDate 2016-01-01 -EndDate 2018-12-31
9. Get the metadata from the certificate with the PowerShell script provided (code snippet 3):
   .\Get-SelfSignedCertificateInformation.ps1 -CertPath "d:\PnPCert.cer"
10. Copy the keyCredentials JSON element (ignore the thumbprint and do not reformat code!)
11. Go back to the application configure tab in Azure portal and click Manage manifest / download manifest in footer
12. Edit the downloaded manifest file - paste the keyCredentials JSON from step 10 into it, save it and upload it back to the Azure portal via Manage manifest / upload manifest
13. Click on application permissions (not delegated permissions!) for Office 365 SharePoint Online and check 'Read items in all site collections' and click the save button in the footer (important!)
14. In Program.cs enter your tenant URL (such as yourtenant.onmicrosoft.com) and the full path to the PFX certificate created in step 8
15. Run the application with CTRL-F5 and enter the certifcate password from step 8 when prompted



