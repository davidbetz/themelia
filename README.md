[Themelia Pro website on Azure](http://themeliapro.azurewebsites.net/)

Themelia (aka Nalarium.Web.Processing; **historical**) - now defunct project about drove me mad in 2008. It's the largest things I've ever worked on. *It's ASP.NET on crack* (a concept inspired by my [collegue/mentor](https://github.com/Grimace1975)).

I threw the [Themelia Pro website up on Azure](http://themeliapro.azurewebsites.net/) just for historical purposes. All the docs are there. I didn't update all the images. Don't care.

So... everything ran through a master HTTP module brain. You could define a website, then inherit one site from another (multi-tenancy through OOP XML config). You controlled your website via a surface area (i.e. white-listing): You controlled how you wanted your site to be accessed; locked down by default. Want /contact? Define it. It also ran off "web domains" (like app domains, but for your web site; think of lightweight virtual applications.) Then there was the component model (programmatic packages of stuff analagous to the following XML).

It was crazy huge. I wanted to reinvent web development. I feel like I about lost my mind in the process...

Just for fun, below was one website's config file:

View samples as [ThemeliaProSamples](https://github.com/davidbetz/ThemeliaProSamples).

### Totally random Themelia Pro sample
  
<pre>
  &lt;web.processing&gt;
    &lt;webDomains&gt;
        &lt;add default="/Page_/Home/Root.aspx" catchAllMode="RedirectToRoot"&gt;
            &lt;endpoints&gt;
                &lt;add text="purchase" type="Page" parameter="/Page_/Home/Purchase.aspx" /&gt;
                &lt;add text="howto" type="Page" parameter="/Page_/Home/HowTo.aspx" /&gt;
                &lt;!----&gt;
                &lt;add text="sample/" type="Sequence" parameter="{Sample}" /&gt;
                &lt;add text="account" type="Sequence" parameter="{Account}" /&gt;
                &lt;add text="license" type="Sequence" parameter="{License}" /&gt;
                &lt;add text="register" type="Sequence" parameter="{Register}" /&gt;
                &lt;add text="support" type="Sequence" parameter="{Contact}" /&gt;
                &lt;add text="problem" type="Sequence" parameter="{Contact}" /&gt;
                &lt;add text="contact" type="Sequence" parameter="{Contact}" /&gt;
                &lt;add text="verify" type="Sequence" parameter="{Verification}" /&gt;
                &lt;!----&gt;
                &lt;add text="license/info" type="Page" parameter="/Page_/License/Info.aspx" /&gt;
                &lt;add text="register/check" type="Registration" /&gt;
                &lt;add selector="EndsWith" text="emailsend/" type="EmailProcessing" /&gt;
            &lt;/endpoints&gt;
            &lt;factories&gt;
                &lt;add type="NalariumWS.Web.Processing.HandlerFactory, NalariumWS.Web" /&gt;
            &lt;/factories&gt;
            &lt;security defaultAccessMode="Allow"
                      loginText="login"
                      loginPage="/Page_/Security/Login.aspx"
                      defaultLoggedInTarget="/"
                      validatorType="NalariumWS.Web.Security.SecurityValidator, NalariumWS.Web"&gt;
                &lt;exceptions&gt;
                    &lt;add key="account" /&gt;
                    &lt;!--&lt;add key="contact" /&gt;--&gt;
                    &lt;add key="support" /&gt;
                    &lt;add key="license" /&gt;
                &lt;/exceptions&gt;
            &lt;/security&gt;
        &lt;/add&gt;
        &lt;add default="{Sample}" name="SimpleContent" path="sample/simplecontent"&gt;
            &lt;endpoints&gt;
                &lt;add text="process" type="NalariumWS.Web.DownloadHttpHandler, NalariumWS.Web"&gt;
                    &lt;parameters&gt;
                        &lt;add name="FilePath" value="C:\_REFERENCE\PUBLIC\JampadTechnologySimpleContentViewer.zip" /&gt;
                        &lt;add name="OutputName" value="JampadTechnologySimpleContentViewer.zip" /&gt;
                        &lt;add name="ContentType" value="application/zip" /&gt;
                        &lt;add name="Size" value="31036" /&gt;
                    &lt;/parameters&gt;
                &lt;/add&gt;
            &lt;/endpoints&gt;
            &lt;!--&lt;accessRules&gt;
                &lt;add type="HttpReferrer" text="{blank}" actionType="Write" parameter="" /&gt;
            &lt;/accessRules&gt;--&gt;
        &lt;/add&gt;
        &lt;add default="/Page_/Home/Download.aspx" name="Download" path="download"&gt;
            &lt;endpoints&gt;
                &lt;add text="process" type="NalariumWS.Web.DownloadHttpHandler, NalariumWS.Web"&gt;
                    &lt;parameters&gt;
                        &lt;add name="FilePath" value="C:\_REFERENCE\RELEASE\Beta1\ThemeliaPro.Beta1.zip" /&gt;
                        &lt;add name="OutputName" value="ThemeliaPro.Beta1.zip" /&gt;
                        &lt;add name="ContentType" value="application/zip" /&gt;
                        &lt;add name="Size" value="271509" /&gt;
                    &lt;/parameters&gt;
                &lt;/add&gt;
                &lt;add type="File" text="source/lib/trace-latest.js"&gt;
                    &lt;parameters&gt;
                        &lt;add name="target" value="/Resource_/Source/Client/Lib/Trace-1.5.js" /&gt;
                        &lt;add name="extra" value="text/javascript" /&gt;
                    &lt;/parameters&gt;
                &lt;/add&gt;
            &lt;/endpoints&gt;
            &lt;!--&lt;accessRules&gt;
                &lt;add type="HttpReferrer" text="{blank}" actionType="Write" parameter="" /&gt;
            &lt;/accessRules&gt;--&gt;
        &lt;/add&gt;
        &lt;add name="ImageRendering" path="image/render/download" default="{Handler NalariumWS.Web.TextRenderHttpHandler, NalariumWS.Web}"&gt;
            &lt;parameters&gt;
                &lt;add name="ImageName" value="NalariumWS.Web._RESOURCE.Image.Download.png" /&gt;
                &lt;add name="Text" value="Version 2.0 Beta 1" /&gt;
                &lt;add name="TextColor" value="#ffffff" /&gt;
                &lt;add name="TopPosition" value="32" /&gt;
                &lt;add name="LeftPosition" value="87" /&gt;
                &lt;add name="FontFamily" value="Calibri, Arial" /&gt;
                &lt;add name="FontSize" value="12" /&gt;
            &lt;/parameters&gt;
            &lt;!--&lt;accessRules&gt;
                &lt;add type="HttpReferrer" text="{blank}" actionType="Write" parameter="" /&gt;
            &lt;/accessRules&gt;--&gt;
        &lt;/add&gt;
        &lt;add name="forum" path="forum" default="/Sequence_/Forum.aspx" catchAllMode="PassToDefault"&gt;
            &lt;components&gt;
                &lt;add type="Nalarium.Content.Processing.ContentComponent, Nalarium.Content"&gt;
                    &lt;parameters&gt;
                        &lt;add name="domainGuid" value="9A04ACDC-55B9-4085-9FAD-A913FBC29324" /&gt;
                    &lt;/parameters&gt;
                &lt;/add&gt;
            &lt;/components&gt;
            &lt;security defaultAccessMode="Block"
                      loginText="login"
                      loginPage="/Page_/Security/Login.aspx"
                      defaultLoggedInTarget="/forum/"
                      validatorType="NalariumWS.Web.Security.SecurityValidator, NalariumWS.Web" /&gt;
        &lt;/add&gt;
        &lt;add name="blog" path="blog" default="{Blog}" catchAllMode="PassToDefault"&gt;
            &lt;!--&lt;components&gt;
        &lt;add type="Jampad.SimpleContent.Processing.ContentComponent, Jampad.SimpleContent"&gt;
          &lt;parameters&gt;
            &lt;add name="physicalFolder" value="{AppSetting docsPhysicalFolder}" /&gt;
          &lt;/parameters&gt;
        &lt;/add&gt;
      &lt;/components&gt;--&gt;
        &lt;/add&gt;
        &lt;add name="docs" path="docs" default="{Documentation}" catchAllMode="PassToDefault"&gt;
            &lt;components&gt;
                &lt;add type="Jampad.SimpleContent.Processing.ContentComponent, Jampad.SimpleContent"&gt;
                    &lt;parameters&gt;
                        &lt;add name="physicalFolder" value="{AppSetting docsPhysicalFolder}" /&gt;
                    &lt;/parameters&gt;
                &lt;/add&gt;
            &lt;/components&gt;
        &lt;/add&gt;
        &lt;add name="reference" path="reference" default="{Documentation}" catchAllMode="PassToDefault"&gt;
            &lt;!--&lt;components&gt;
        &lt;add type="Jampad.SimpleContent.Processing.ContentComponent, Jampad.SimpleContent"&gt;
          &lt;parameters&gt;
            &lt;add name="physicalFolder" value="{AppSetting referencePhysicalFolder}" /&gt;
          &lt;/parameters&gt;
        &lt;/add&gt;
      &lt;/components&gt;--&gt;
        &lt;/add&gt;
    &lt;/webDomains&gt;
    &lt;!--&lt;sequences&gt;
        &lt;add name="Register"&gt;
            &lt;views&gt;
                &lt;add name="Input" /&gt;
                &lt;add name="VerificationSent" /&gt;
                &lt;add name="VerificationSuccess" /&gt;
                &lt;add name="VerificationError" /&gt;
            &lt;/views&gt;
        &lt;/add&gt;
    &lt;/sequences&gt;--&gt;
&lt;/web.processing&gt;
</pre>
