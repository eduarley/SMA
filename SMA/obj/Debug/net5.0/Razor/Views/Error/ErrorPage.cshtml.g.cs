#pragma checksum "C:\Users\Eduardo Arley\Desktop\SMA\SMA\Views\Error\ErrorPage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5aab543e3f793e32c595550cee1eebf2e469477f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Error_ErrorPage), @"mvc.1.0.view", @"/Views/Error/ErrorPage.cshtml")]
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
#line 1 "C:\Users\Eduardo Arley\Desktop\SMA\SMA\Views\_ViewImports.cshtml"
using SMA;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Eduardo Arley\Desktop\SMA\SMA\Views\_ViewImports.cshtml"
using SMA.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5aab543e3f793e32c595550cee1eebf2e469477f", @"/Views/Error/ErrorPage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5a01f3040082814a240acc24cf84418ca166024f", @"/Views/_ViewImports.cshtml")]
    public class Views_Error_ErrorPage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 2 "C:\Users\Eduardo Arley\Desktop\SMA\SMA\Views\Error\ErrorPage.cshtml"
  
    ViewData["Title"] = "Error";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n\n<center class=\"mt-5\">\n    <h1 style=\"font-weight: lighter;\">");
#nullable restore
#line 8 "C:\Users\Eduardo Arley\Desktop\SMA\SMA\Views\Error\ErrorPage.cshtml"
                                 Write(ViewData["Codigo"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\n    <h5 style=\"font-weight: bold;\">");
#nullable restore
#line 9 "C:\Users\Eduardo Arley\Desktop\SMA\SMA\Views\Error\ErrorPage.cshtml"
                              Write(ViewData["Error"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\n    <a");
            BeginWriteAttribute("href", " href=\"", 191, "\"", 231, 1);
#nullable restore
#line 10 "C:\Users\Eduardo Arley\Desktop\SMA\SMA\Views\Error\ErrorPage.cshtml"
WriteAttributeValue("", 198, Url.Action("Index", "Principal"), 198, 33, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Volver a principal</a>\n</center>\n\n\n\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591