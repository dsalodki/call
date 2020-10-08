using System;
using System.Threading;
using System.Web.Mvc;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;

namespace Call.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : CallControllerBase
    {
        public ActionResult Index()
        {
            ThreadPool.QueueUserWorkItem(NotifyPendingRequests);
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }

        static void NotifyPendingRequests(Object stateInfo)
        {
                Thread.Sleep(5000);
            //todo send signalR message about all pending requests to this client
        }
    }
}