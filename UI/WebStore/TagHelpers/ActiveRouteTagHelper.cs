using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = attributeName)]
    public class ActiveRouteTagHelper : TagHelper
    {
        private const string attributeName = "is-active-route";  // имя тэга

        private const string ignoreAction = "ignore-action";


        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        // второй атрибут означает, что свойство не связано с разметкой
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        // в словарь попадут слова относящиеся к тегам указанным в HtmlAttributeName
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var ignore_action = output.Attributes.ContainsName(ignoreAction);

            if (IsActive(ignore_action))
                MakeActive(output);

            output.Attributes.RemoveAll(attributeName);
            output.Attributes.RemoveAll(ignoreAction);
        }

        private bool IsActive(bool ignoreAction)
        {
            var route_values = ViewContext.RouteData.Values;

            var current_controller = route_values["controller"].ToString();
            var current_action = route_values["action"].ToString();

            if (!string.IsNullOrEmpty(Controller) && !string.Equals(current_controller, Controller))
                return false;

            if (!ignoreAction && !string.IsNullOrEmpty(Action) && !string.Equals(current_action, Action))
                return false;

            foreach (var (key, value) in RouteValues)
                if (!route_values.ContainsKey(key) || route_values[key].ToString() != value)
                    return false;

            return true;
        }

        private static void MakeActive(TagHelperOutput output) 
        {
            var class_attribute = output.Attributes.FirstOrDefault(attr => attr.Name == "class");

            if (class_attribute is null)
                output.Attributes.Add("class", "active");
            else 
            {
                // если в элементе разметки, есть атрибут active
                if (class_attribute.Value.ToString()?.Contains("active") ?? false) return;

                // если нет, то дописываем в атрибут "class" свойство "active"
                output.Attributes.SetAttribute("class", class_attribute.Value + "active");
            }
        }
    }
}
