using Domain.Requests;
using Domain.Responses;
using Domain.Services;
using MvcApplication.Models;

namespace MvcApplication.ControllerHandlers
{
    public class SampleControllerHandler : ISampleControllerHandler
    {
        private readonly ISampleService _sampleService;

        public SampleControllerHandler(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        public QuerySamplesViewModel QuerySamples(QuerySamplesFormModel model)
        {
            QuerySamplesViewModel result = new QuerySamplesViewModel();

            QuerySamplesResponse response = _sampleService.QuerySamples(new QuerySamplesRequest() { Name = model.Name });

            if (response.HasError)
            {
                result.Messages = response.Messages;
                return result;
            }

            result.Samples = response.Samples;

            return result;
        }

        public CreateSampleViewModel Create(CreateSampleFormModel model = null)
        {
            CreateSampleViewModel result = new CreateSampleViewModel();
            CreateSampleResponse response = _sampleService.CreateSample(new CreateSampleRequest() { Name = result.Name, Size = result.Size });

            if (response.HasError)
            {
                result.Messages = response.Messages;
                return result;
            }

            return result;
        }
    }
}