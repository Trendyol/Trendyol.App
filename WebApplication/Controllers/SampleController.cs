using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using Common.Logging;
using Data;
using Domain.Objects;
using Domain.Requests;
using Domain.Responses;
using Domain.Services;
using Swashbuckle.Swagger.Annotations;
using Trendyol.App.Configuration;
using Trendyol.App.Domain.Abstractions;
using Trendyol.App.Domain.Objects;
using Trendyol.App.EntityFramework.Extensions;
using Trendyol.App.Validation.Aspects;
using Trendyol.App.WebApi.Controllers;

namespace WebApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : TrendyolApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger<SampleController>();

        private readonly ISampleService _sampleService;
        private readonly IConfigManager _configManager;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SampleController(ISampleService sampleService, IConfigManager configManager, IDateTimeProvider dateTimeProvider)
        {
            _sampleService = sampleService;
            _configManager = configManager;
            _dateTimeProvider = dateTimeProvider;
        }

        [Route("")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(QuerySamplesResponse))]
        public IHttpActionResult Filter([FromUri(Name = "")]QuerySamplesRequest request)
        {
            Logger.Trace("Fetching samples.");

            if (request == null)
            {
                request = new QuerySamplesRequest();
            }

            if (request.Name == "badrequest")
            {
                return InvalidRequest("Dude, that's a bad request.", "ValidationError");
            }

            QuerySamplesResponse response;

            using (var context = new DataContext())
            {
                IQueryable<Sample> query = context.Samples;

                if (!String.IsNullOrEmpty(request.Name))
                {
                    query = query.Where(s => s.Name == request.Name);
                }

                response = new QuerySamplesResponse(query.WithNoLock(q => q.Select(request.Fields).ToPage(request)));
            }

            return Ok(response);
        }

        [Route("")]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created, "Created", typeof(Sample))]
        public IHttpActionResult Post(CreateSampleRequest request)
        {
            Logger.Trace("Creating sample.");

            CreateSampleResponse response = _sampleService.CreateSample(request);
            return Ok(response);
        }

        [Route("{id}")]
        [HttpGet]
        public Sample Get(long id, string fields = null)
        {
            Logger.Trace($"Fetching sample by id: {id}.");

            Sample sample;

            using (var context = new DataContext())
            {
                sample = context.Samples.Where(s => s.Id == id).Select(fields).FirstOrDefault();
            }

            return sample;
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