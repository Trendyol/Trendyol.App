using System.Web.Mvc;
using MvcApplication.ControllerHandlers;
using MvcApplication.Models;

namespace MvcApplication.Controllers
{
    [RoutePrefix("samples")]
    public class SampleController : Controller
    {
        private readonly ISampleControllerHandler _controllerHandler;

        public SampleController(ISampleControllerHandler controllerHandler)
        {
            _controllerHandler = controllerHandler;
        }

        [HttpGet, Route("")]
        public ActionResult Index(QuerySamplesFormModel formModel)
        {
            QuerySamplesViewModel model = _controllerHandler.QuerySamples(formModel);
            return View(model);
        }

        [HttpGet, Route("create")]
        public ActionResult Create()
        {
            CreateSampleViewModel model = _controllerHandler.Create();
            return View(model);
        }

        [HttpPost, Route("create")]
        public ActionResult Create(CreateSampleFormModel formModel)
        {
            CreateSampleViewModel model;

            if (!ModelState.IsValid)
            {
                model = _controllerHandler.Create();
                return View(model);
            }

            model = _controllerHandler.Create(formModel);

            if (model.HasError)
            {
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}