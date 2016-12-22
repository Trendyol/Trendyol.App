using System.Linq;
using System.Web;
using System.Web.Http;
using Common.Logging;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Requests;
using WebApplication.Managers;
using WebApplication.Repositories;

namespace WebApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger<SampleController>();

        private readonly ISampleManager _sampleManager;
        private readonly Context _context;
        private readonly ISampleRepository _sampleRepository;

        public SampleController(ISampleManager sampleManager, Context context, ISampleRepository sampleRepository)
        {
            _sampleManager = sampleManager;
            _context = context;
            _sampleRepository = sampleRepository;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Filter(string fields = null, string name = null)
        {
            Logger.Trace("Fetching samples.");

            var samples = _sampleRepository.GetSamples(fields, name);

            return Ok(samples);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Get(long id, string fields = null)
        {
            Logger.Trace($"Fetching sample by id: {id}.");

            Sample sample = _context.Samples.Where(s => s.Id == id).Select(fields).FirstOrDefault();

            if (sample == null)
            {
                throw new HttpException(404, "");
            }

            if (!_sampleManager.IsValid(sample))
            {
                return BadRequest("Invalid sample.");
            }

            return Ok(sample);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Post(CreateSampleRequest request)
        {
            Logger.Trace("Creating sample.");

            Sample sample = new Sample();
            sample.Name = request.Name;

            using (var context = new Context())
            {
                context.Samples.Add(sample);
                context.SaveChanges();
            }

            return Ok(sample);
        }
    }
}