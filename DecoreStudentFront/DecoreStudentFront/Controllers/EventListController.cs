using System.Web.Mvc;
using DecoreStudentFront.ViewModels;
using System;
using DecoreStudentFront.EmployeeServiceRef;
using DecoreStudentFront.EventServiceRef;
using DecoreStudentFront.TicketServiceRef;

namespace DecoreStudentFront.Controllers
{
    public class EventListController : Controller
    {
        private readonly EmployeeServiceWCFClient _employeeWcfClient = new EmployeeServiceWCFClient();

        private readonly EventServiceClient _eventWCFclient = new EventServiceClient();
        private readonly TicketServiceClient _ticketWCFclient = new TicketServiceClient();


        public ActionResult Index()
        {
            var events = _eventWCFclient.GetEvents();
            var eventTypes = _eventWCFclient.GetEventTypes();
            var sectionTypes = _employeeWcfClient.GetAllSections();

            var viewModel = new EventViewModel
            {
                Events = events,
                EventTypes = eventTypes,
                SectionTypes = sectionTypes
            };
            return View(viewModel);
        }
    }
}