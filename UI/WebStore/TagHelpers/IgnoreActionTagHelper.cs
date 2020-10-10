using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.TagHelpers
{
    //[HtmlTargetElement(Attributes = AttributeName)]
    public class IgnoreActionTagHelper : TagHelper
    {
        //public const string AttributeName = "ignore-action";  // имя тэга


        //[HtmlAttributeName("asp-controller")]
        //public string Controller { get; set; }
    }
}
