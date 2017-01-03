using MvcApplication.Models;
using Trendyol.App.Mvc.ControllerHandlers;

namespace MvcApplication.ControllerHandlers
{
    public interface ISampleControllerHandler : IControllerHandler
    {
        QuerySamplesViewModel QuerySamples(QuerySamplesFormModel model);

        CreateSampleViewModel Create(CreateSampleFormModel model = null);
    }
}