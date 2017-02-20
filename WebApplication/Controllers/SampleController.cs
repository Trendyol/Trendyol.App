using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Common.Logging;
using Domain.Objects;
using Domain.Requests;
using Domain.Services;
using Swashbuckle.Swagger.Annotations;
using Trendyol.App.Domain.Objects;
using Trendyol.App.WebApi.Controllers;
using Trendyol.App.WebApi.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : TrendyolApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger<SampleController>();

        private readonly ISampleService _sampleService;

        public SampleController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        [Route("")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(IEnumerable<Sample>))]
        public IHttpActionResult Filter(string fields = null, string name = null)
        {
            Logger.Trace("Fetching samples.");

            if (name == "badrequest")
            {
                return InvalidRequest("Dude, that's a bad request.", "ValidationError");
            }

            var response = _sampleService.QuerySamples(new QuerySamplesRequest() { Fields = fields, Name = name });

            return Ok(response.Samples);
        }

        [Route("")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created, "Created", typeof(Sample))]
        public IHttpActionResult Post(CreateSampleRequest request)
        {
            Logger.Trace("Creating sample.");

            Sample sample = _sampleService.CreateSample(request).Data;
            return Created(sample);
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
        public void Patch(long id, List<OperationParameter> parameters)
        {
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(long id)
        {
        }
    }
}