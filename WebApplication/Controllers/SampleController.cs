using System;
using System.Collections.Generic;
using System.Web.Http;
using Common.Logging;
using Domain.Objects;
using Domain.Requests;
using Domain.Services;
using Trendyol.App.WebApi.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger<SampleController>();

        private readonly ISampleService _sampleService;

        public SampleController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<Sample> Filter(string fields = null, string name = null)
        {
            Logger.Trace("Fetching samples.");

            var response = _sampleService.QuerySamples(new QuerySamplesRequest() { Fields = fields, Name = name });

            return response.Samples;
        }

        [Route("")]
        [HttpPost]
        public Sample Post(CreateSampleRequest request)
        {
            Logger.Trace("Creating sample.");

            return _sampleService.CreateSample(request).Data;
        }

        [Route("{id}")]
        [HttpGet]
        public Sample Get(long id, string fields = null)
        {
            Logger.Trace($"Fetching sample by id: {id}.");

            return null;
        }

        [Route("{id}")]
        [HttpPut]
        public void Put(long id, Sample sample)
        {
            throw new Exception("Error....");
        }

        [Route("{id}")]
        [HttpPatch]
        public void Patch(long id, List<PatchParameter> parameters)
        {
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(long id)
        {
        }
    }
}