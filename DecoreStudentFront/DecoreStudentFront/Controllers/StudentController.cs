using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DecoreStudentFront.StudentServiceRef;
using DecoreStudentFront.UserServiceRef;
using DecoreStudentFront.EventServiceRef;
using DecoreStudentFront.TicketServiceRef;
using DecoreStudentFront.EmployeeServiceRef;
using DecoreStudentFront.ViewModels;

namespace DecoreStudentFront.Controllers
{
    public class StudentController : Controller
    {
        UserServiceClient userService = new UserServiceClient();
        StudentUsers studentUser = new StudentUsers();
        EventServiceClient eventService = new EventServiceClient();
        private readonly EventServiceClient _eventWCFclient = new EventServiceClient();
        private readonly TicketServiceClient _ticketWCFclient = new TicketServiceClient();
        private readonly EmployeeServiceWCFClient _employeeWcfClient = new EmployeeServiceWCFClient();
        UserInfo userInfo = new UserInfo();
  

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
            
            try
            {
                
                studentUser = userService.GetStudentUser(id);
                ViewBag.User = studentUser.FirstName + " " + studentUser.LastName;
                return View(viewModel);
                
            }
            catch (Exception)
            {
                return View();
            }      
        }

        public ActionResult MyInfo()
        {
            string idString = User.Identity.Name;
            int id = Int32.Parse(idString);

            studentUser = userService.GetStudentUser(id);
            return View(studentUser);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                userInfo = userService.GetUserById(id);
                return View(userInfo);
            }
            catch (Exception)
            {

                ViewBag.Message = "Snap, coulnd't find user detailjs for user with ID: " + id;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, UserInfo updatedUser)
        {
            

            try
            {
                userService.UpdateUser(updatedUser);
                return RedirectToAction("Start");
            }
            catch (Exception)
            {

                ViewBag.Message = "Snap, coulnd't update the user info";
                return View(userInfo);
            }
        }



        public ActionResult Delete(int id)
        {
            try
            {
                studentUser = userService.GetStudentUser(id);
                return View(studentUser);
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                userService.DeleteUser(id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

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