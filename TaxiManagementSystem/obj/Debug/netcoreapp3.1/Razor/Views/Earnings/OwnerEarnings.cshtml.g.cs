#pragma checksum "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5bdeee283bcc174629fa689036e63bbc10d3bf43"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Earnings_OwnerEarnings), @"mvc.1.0.view", @"/Views/Earnings/OwnerEarnings.cshtml")]
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
#line 1 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\_ViewImports.cshtml"
using TaxiManagementSystem;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\_ViewImports.cshtml"
using TaxiManagementSystem.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5bdeee283bcc174629fa689036e63bbc10d3bf43", @"/Views/Earnings/OwnerEarnings.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"21a8af98e7c14e34de122d91f983bed9c5027485", @"/Views/_ViewImports.cshtml")]
    public class Views_Earnings_OwnerEarnings : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TaxiManagementSystem.Models.EarningsViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Earnings", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "GetOwnersEarnings", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("display: contents;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Export", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 4 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
  
    ViewData["Title"] = "My Earnings";
    Layout = "~/Views/Shared/_OwnerLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div id=\"Container\">\r\n    <div class=\" row right \">\r\n        <div class=\"col-sm-8\"><h3>Owner\'s Income</h3></div>\r\n        <div class=\"col-sm-4\">\r\n         \r\n        </div>\r\n    </div>\r\n    <br>\r\n    <div class=\"row\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5bdeee283bcc174629fa689036e63bbc10d3bf436164", async() => {
                WriteLiteral("\r\n            <div class=\"col-sm-6 p-2\">\r\n                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "5bdeee283bcc174629fa689036e63bbc10d3bf436482", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 19 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.UserId);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 19 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                                                WriteLiteral(Model.UserId);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "5bdeee283bcc174629fa689036e63bbc10d3bf439025", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 20 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.UserId);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 20 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                                                WriteLiteral(Model.DriverId);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "5bdeee283bcc174629fa689036e63bbc10d3bf4311570", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 21 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.UserId);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 21 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                                                WriteLiteral(Model.TaxiId);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "5bdeee283bcc174629fa689036e63bbc10d3bf4314114", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
#nullable restore
#line 22 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.Search);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n            </div>\r\n            <div class=\"col-sm-2 p-2\">\r\n                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "5bdeee283bcc174629fa689036e63bbc10d3bf4315835", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
#nullable restore
#line 25 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.SearchDate);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n            </div>\r\n            <div class=\"col-sm-2 p-2\">\r\n                ");
#nullable restore
#line 28 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
           Write(Html.CheckBoxFor(model => model.IncludeDateSearch));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                <label class=""form-check-label"" for=""IncludeDateSearch"">
                    Include date
                </label>
            </div>
            <div class=""col-sm-2 p-2""><button class=""btn btn-primary"" type=""submit"">Get earnings</button></div>
        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n    <div class =\"row\">       \r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "5bdeee283bcc174629fa689036e63bbc10d3bf4319699", async() => {
                WriteLiteral("\r\n            <div class=\"col-sm-8 p-2\"> </div>\r\n             \r\n                <div class=\"col-sm-2 p-2\"><button class= \"btn btn-secondary\" type=\"submit\">Export</button></div>\r\n        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
    </div>

    <br>

    <div class=""row"">
        <table class=""table"">
            <thead>
                <tr>
                    <th>
                        Date
                    </th>
                    <th>
                        Driver
                    </th>
                    <th>
                        Vehicle
                    </th>
                    <th>
                        Earnings
                    </th>
                    <th>
                        Expenditure
                    </th>
                    <th>
                        Income Earned
                    </th>
                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 71 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                 foreach (var item in Model.Earnings)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr>\r\n                        <td>\r\n                            ");
#nullable restore
#line 75 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                       Write(Html.DisplayFor(modelItem => item.ShiftDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
#nullable restore
#line 78 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                       Write(Html.DisplayFor(modelItem => item.Driver.FirstName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
#nullable restore
#line 81 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                       Write(Html.DisplayFor(modelItem => item.Taxi.Registration));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
#nullable restore
#line 84 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                       Write(Html.DisplayFor(modelItem => item.Earning));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
#nullable restore
#line 87 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                       Write(Html.DisplayFor(modelItem => item.Expenditure));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n\r\n                            ");
#nullable restore
#line 91 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                       Write(Html.DisplayFor(modelItem => item.IncomeEarned));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </td>\r\n                       \r\n                    </tr>\r\n");
#nullable restore
#line 95 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n\r\n</div>\r\n\r\n\r\n");
#nullable restore
#line 103 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
 if (Model.Earnings.Any())
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"container\">\r\n        <div class=\"row rounded\" style=\"    bottom: 100px;     width:100%;\">\r\n            <div class=\"col-sm-9\"></div>\r\n            <div class=\"col-sm-3\"><b>Total earnings : ");
#nullable restore
#line 108 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                                                 Write(Model.WeeklyEarnings);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b></div>\r\n       \r\n        </div>\r\n    </div>\r\n");
#nullable restore
#line 112 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"

}

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<br><br>
<div id=""chartContainer"" style=""height: 400px; width: 100%;""></div>
<script src=""https://canvasjs.com/assets/script/canvasjs.min.js""></script>
<script>
    window.onload = function () {

        var chart = new CanvasJS.Chart(""chartContainer"", {
            animationEnabled: true,
            title: {
                text: ""Income for 2021""
            },
            toolTip: {
                shared: true
            },
            axisY: {
                prefix: ""$"",
                title: ""Income in (AUD)""
            },
            data: [{
                type: ""line"",
                name: ""Income (AUD)"",
                showInLegend: true,
                yValueFormatString: ""$#,##0.00"",
                dataPoints: ");
#nullable restore
#line 137 "C:\Users\prana\OneDrive - Federation University Australia\Documents\TaxiManagementSystem\TaxiManagementSystem\Views\Earnings\OwnerEarnings.cshtml"
                       Write(Html.Raw(ViewBag.DataPoints1));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            }]\r\n        });\r\n        chart.render();\r\n\r\n    }\r\n</script>\r\n\r\n\r\n<script>\r\n    $(document).ready(function () {\r\n        $(\"#openAdd\").click(function () {\r\n            $(\"#myModal\").modal(\"show\");\r\n        });\r\n    });\r\n\r\n\r\n</script>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TaxiManagementSystem.Models.EarningsViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591