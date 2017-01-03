using System.Collections.Generic;
using System.Web.Mvc;
using Trendyol.App.Domain.Dtos;

namespace Trendyol.App.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static SelectList ToSelectList(this IEnumerable<LookupDto> lookups)
        {
            return new SelectList(lookups, "Value", "Text");
        }

        public static SelectList ToSelectList(this IEnumerable<LookupDto> lookups, object selectedValue)
        {
            return new SelectList(lookups, "Value", "Text", selectedValue);
        }
    }
}