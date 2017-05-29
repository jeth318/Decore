using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DecoreStudentFront.UserServiceRef;
using DecoreStudentFront.EventServiceRef;
using DecoreStudentFront.TicketServiceRef;
using DecoreStudentFront.EmployeeServiceRef;
using DecoreStudentFront.ViewModels;
using log4net;

namespace DecoreStudentFront.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        UserServiceClient userService = new UserServiceClient();
        StudentUsers studentUser = new StudentUsers();
        EventServiceClient eventService = new EventServiceClient();
        private readonly EventServiceClient _eventWCFclient = new EventServiceClient();
        private readonly TicketServiceClient _ticketWCFclient = new TicketServiceClient();
        private readonly EmployeeServiceWCFClient _employeeWcfClient = new EmployeeServiceWCFClient();
        UserServiceRef.UserInfo userInfo = new UserServiceRef.UserInfo();
        

        private static readonly ILog logger = LogManager.GetLogger("StudentFrontLogger");

        
        // GET: Student
        public ActionResult Start()
        {     
            string idString = User.Identity.Name;
            int id = Int32.Parse(idString);

            var events = _eventWCFclient.GetEvents();
            var eventTypes = _eventWCFclient.GetEventTypes();
            var sectionTypes = _employeeWcfClient.GetAllSections();

            var viewModel = new EventViewModel
            {
                Events = events,
                EventTypes = eventTypes,
                SectionTypes = sectionTypes
            };

            studentUser = userService.GetStudentUser(id);
            if (studentUser.Email != null)
            {
                ViewBag.User = studentUser.FirstName + " " + studentUser.LastName;
                           
                return View(viewModel);
            }
            else
            {              
                logger.Fatal("Failed getting student");
                return View();
            }                  
        }

        public ActionResult Edit()
        {
            int id = Int32.Parse(User.Identity.Name);
            try
            {
                userInfo = userService.GetUserById(id);
                
                return View(userInfo);
            }
            catch (Exception)
            {
                logger.Fatal("Failed update student with ID " + id);
                ViewBag.Message = "Snap, couldn't find user details for user with ID: " + id;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, UserServiceRef.UserInfo updatedUser)
        {      
            try
            {
                userService.UpdateUser(updatedUser);
                logger.Debug("Success update student with ID " + id);
                return RedirectToAction("Start");
            }
            catch (Exception)
            {

                ViewBag.Message = "Snap, couldn't update the user info";
                logger.Fatal("Failed update student with ID " + id);
                return View(userInfo);
            }
        }

        // ********** NOT IN USE. BUILT FOR FUTURE
        public ActionResult Delete(int id)
        {
            studentUser = userService.GetStudentUser(id);
            if (studentUser.Email != null)
                return View(studentUser);

            else
                return View();

        }

        // ********** NOT IN USE YET. BUILT FOR FUTURE
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            
            if(userService.DeleteUser(id))
            {
                logger.Debug("Success delete student with ID " + id);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                logger.Fatal("Failed delete student with ID " + id);
                return View();
            }      
        }

        public ActionResult LogOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}