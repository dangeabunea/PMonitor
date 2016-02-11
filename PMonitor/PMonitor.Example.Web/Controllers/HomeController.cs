using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMonitor.Core;

namespace PMonitor.Example.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProcessStatus()
        {
            //Instantiate and refresh information regarding the 3 processes (Notepad, Wordpad and Paint). The
            //information is taken from the Web.config file
            AbstractProcessMonitor processMonitor = ProcessMonitorFactory.BuildDefaultOSProcessMonitor();
            processMonitor.RefreshInformation();

            IList<BasicProcessInformation> statusOfProcesses = processMonitor.GetProcessInformation();

            return Json(statusOfProcesses, JsonRequestBehavior.AllowGet);
        }
    }
}