using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DecoreStudentFront.StudentServiceRef;
using DecoreStudentFront.UserServiceRef;
    

namespace DecoreStudentFront.Controllers
{
    public class StudentController : Controller
    {
        
        UserServiceClient userService = new UserServiceClient();
        StudentUsers studentUser = new StudentUsers();
        UserInfo userInfo = new UserInfo();
        
        // GET: Student
        public ActionResult Start()
        {
            string idString = User.Identity.Name;
            int id = Int32.Parse(idString);

            try
            {
                
                studentUser = userService.GetStudentUser(id);
                ViewBag.User = studentUser.FirstName + " " + studentUser.LastName;
                return View(studentUser);
                
            }
            catch (Exception)
            {
                return View();
            }      
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
            updatedUser.Id = id;

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