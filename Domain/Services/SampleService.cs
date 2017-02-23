using Domain.Repositories;
using Domain.Requests;
using Domain.Responses;

namespace Domain.Services
{
    public class SampleService : ISampleService
    {
        private readonly ISampleRepository _sampleRepository;

        public SampleService(ISampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
        }

        public QuerySamplesResponse QuerySamples(QuerySamplesRequest request)
        {
            QuerySamplesResponse response = new QuerySamplesResponse(null);

            _sampleRepository.QuerySamples(request.Fields, request.Name);

            return response;
        }

        public CreateSampleResponse CreateSample(CreateSampleRequest request)
        {
            CreateSampleResponse response = new CreateSampleResponse();

            response.Data = _sampleRepository.CreateSample(request.Name);

            return response;
        }
    }
}