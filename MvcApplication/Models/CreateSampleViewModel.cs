using System.Collections.Generic;
using Trendyol.App.Mvc.Models;

namespace MvcApplication.Models
{
    public class CreateSampleViewModel : BaseViewModel
    {
        public string Name { get; set; }

        public string Size { get; set; }

        public List<string> Cities { get; set; }
    }
}