#pragma checksum "C:\Users\prana\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Users\SuccessfullRegistration.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5409f0ffb76818298aa4e19b26e26727372cad8a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Users_SuccessfullRegistration), @"mvc.1.0.view", @"/Views/Users/SuccessfullRegistration.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\prana\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\_ViewImports.cshtml"
using TaxiManagementSystem;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\prana\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\_ViewImports.cshtml"
using TaxiManagementSystem.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5409f0ffb76818298aa4e19b26e26727372cad8a", @"/Views/Users/SuccessfullRegistration.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"21a8af98e7c14e34de122d91f983bed9c5027485", @"/Views/_ViewImports.cshtml")]
    public class Views_Users_SuccessfullRegistration : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TaxiManagementSystem.Models.RegistrationViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 4 "C:\Users\prana\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Users\SuccessfullRegistration.cshtml"
  
    bool invalidLogin = Model?.UserId == -1;
    ViewData["Title"] = "Home Page";
    Layout = "~/Views/Shared/_publicLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n<div class=\"container\">\r\n    <div class=\"row\">\r\n        <div class=\"col-sm-8\">\r\n            <h3> Successfull Registrarion</h3>\r\n            <p>You have been sucessfully registered. Please proceed to  ");
#nullable restore
#line 16 "C:\Users\prana\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Users\SuccessfullRegistration.cshtml"
                                                                   Write(Html.ActionLink("Login", "Login", "Users", new { @class = "btn btn-primary" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 22 "C:\Users\prana\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Users\SuccessfullRegistration.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
            }
            );
            WriteLiteral("\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TaxiManagementSystem.Models.RegistrationViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
