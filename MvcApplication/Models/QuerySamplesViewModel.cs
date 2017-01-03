using System.Collections.Generic;
using Domain.Objects;
using Trendyol.App.Mvc.Models;

namespace MvcApplication.Models
{
    public class QuerySamplesViewModel : BaseViewModel
    {
        public List<Sample> Samples { get; set; }
    }
}