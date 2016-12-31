using Domain.Requests;
using Domain.Responses;
using Trendyol.App.Domain;

namespace Domain.Services
{
    public interface ISampleService : IService
    {
        QuerySamplesResponse QuerySamples(QuerySamplesRequest request);

        CreateSampleResponse CreateSample(CreateSampleRequest request);
    }
}