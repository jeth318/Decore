﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DecoreSysAdminFront.UserServiceRef;
using DecoreSysAdminFront.StudentServiceRef;

using log4net;

namespace DecoreSysAdminFront.Controllers
{
    [Authorize]
    public class SysAdminController : Controller
    {
        UserServiceClient userService = new UserServiceClient();
        StudentServiceClient studentService = new StudentServiceClient();
    
        UserInfo userInfo = new UserInfo();
        StudentInfo studentInfo = new StudentInfo();     
        StudentUsers studentUser = new StudentUsers();
        EmployeeUsers employeeUser = new EmployeeUsers();
        //EmployeeServiceRef.EmployeeInfo employee = new EmployeeServiceRef.EmployeeInfo();

        private static readonly ILog logger = LogManager.GetLogger("TestLogger");


        


        // GET: SysAdmin   STARTSIDA FÖR INLOGGAD SYSADMIN   
        public ActionResult Dashboard()
        {
         
            
            List<UserServiceRef.UserInfo> allUsers = new List<UserInfo>();

            try
            {
                allUsers = userService.GetAllUsers().ToList();
                return View(allUsers);
            }
            catch (Exception)
            {
                logger.Fatal("Failed ot get all users");
                ViewBag.Error = "Failed to fetch users list";
                return View();
            }      
        }

        // GET: SysAdmin/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                userInfo = userService.GetUserById(id);
                return View(userInfo);
            }
            catch (Exception)
            {
                logger.Fatal("Failed to find user:" + id);
                ViewBag.Error = "Can't find user with ID: " + id;
                return View();
            }     
        }

        // GET: SysAdmin/AddStudent
        public ActionResult AddStudent()
        {
            return View(studentUser);
        }

        // POST: SysAdmin/AddStudent
        [HttpPost]
        public ActionResult AddStudent(StudentUsers newStudentUser)
        {
            // Building a StudentUsers-object combining the UserInfo and StudentInfo-objects.
            userInfo.Email = newStudentUser.Email;
            userInfo.SocSecNum = newStudentUser.SocSecNum;
            userInfo.Password = newStudentUser.Password;
            userInfo.FirstName = newStudentUser.FirstName;
            userInfo.LastName = newStudentUser.LastName;
            userInfo.TelNum = newStudentUser.TelNum;
            studentInfo.ProgramCode = newStudentUser.ProgramCode;
            studentInfo.UnionExpiration = newStudentUser.UnionExpiration;
            studentInfo.UnionName = newStudentUser.UnionName;

            try
            {
                // Creating the main user-account
                userInfo = userService.CreateUser(userInfo);
                // Fetching the newly created user-account to get the UserId.
                userInfo = userService.GetUserBySocSecNum(userInfo.SocSecNum);
                userService.Close();
                studentInfo.UserId = userInfo.Id;
                // Creating the accociated student-account.
                studentInfo = studentService.CreateStudent(studentInfo);
                studentService.Close();

                return RedirectToAction("Dashboard");
            }
            catch
            {
                ViewBag.Error = "Failed creating the student user";
                logger.Fatal("Failed to create user");
                return View();
            }
        }

        public ActionResult AddCoreUser()
        {
            return View(userInfo);
        }

        [HttpPost]
        public ActionResult AddCoreUser (UserInfo user)
        {

            userService.CreateUser(user);

            var url = "http://193.10.202.73/Frontend/";

            return Redirect(url);

        }

        // GET: SysAdmin/AddEmployee -- NOT IN USE AT THE MOOOMENT -------------------------------------------- 
      
        public ActionResult AddEmployee()
        {
            return View();
        }


        // POST: SysAdmin/AddEmployee -- NOT IN USE AT THE MOOOMENT. -------------------------------------------- 
        [HttpPost]
        public ActionResult AddEmployee(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here               
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }  


        // GET: SysAdmin/AttatchStudent
        public ActionResult AttatchStudent(int id)
        {        
            
            return View(studentInfo);
        }

        // POST: SysAdmin/AtttachStudent 
        [HttpPost]
        public ActionResult AttatchStudent(int id, StudentInfo newStudentInfo)
        {
            newStudentInfo.UserId = id;
            try
            {
                studentService.CreateStudent(newStudentInfo);
                return RedirectToAction("Dashboard");
            }
            catch
            {
                logger.Fatal("Error while attatching student to user with ID: " + id);
                ViewBag.Error = "Error while attatching student to user with ID: " + id;
                return View();
            }
        }
        // GET: SysAdmin/AttatchEmployee NOT IN USE AT THE MOOOMENT. -------------------------------------------- 
        public ActionResult AttatchEmployee()
        {
            return View();
        }

        // POST: SysAdmin/AttatchEmployee NOT IN USE AT THE MOOOMENT. -------------------------------------------- 
        [HttpPost]
        public ActionResult AttatchEmployee(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                // Ta in ett ifyllt EmployeeInfo-object och skicka in i en servicemetod. 

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SysAdmin/DetatchStudent
        public ActionResult DetatchStudent(int id)
        {

            try
            {
                studentUser = userService.GetStudentUser(id);
                return View(studentUser);
            }
            catch (Exception)
            {
                logger.Fatal("Error while getting the stundet information for user with ID: " + id);
                ViewBag.Error = "Error while getting the stundet information for user with ID: " + id;
                return View();
            }                                
        }

        // POST: SysAdmin/DetatchStudent
        [HttpPost]
        public ActionResult DetatchStudent(int id, FormCollection collection)
        {
            try
            {
                studentInfo = studentService.GetStudentByUserId(id);
                studentService.DeleteStudent(studentInfo.Id);
                return RedirectToAction("Dashboard");
            }
            catch (Exception)
            {
                return View();
            }
        }

        // GET: SysAdmin/DetatchEmployee REDIRECTS TO GROUP 3 . -------------------------------------------- 
        public ActionResult DetatchEmployee(int id)
        {
            var url = "http://193.10.202.73/Frontend";
            return Redirect (url);
        }

       
        // GET: SysAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                userInfo = userService.GetUserById(id);
                return View(userInfo);
            }
            catch (Exception)
            {
                logger.Fatal("Can't find user with ID " + id);
                ViewBag.Error = "Can't find user with ID " + id;
                return View();
            }      
        }

        // POST: SysAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserInfo updatedUser)
        {
            try
            {
                userService.UpdateUser(updatedUser);
                return RedirectToAction("Dashboard");
            }
            catch
            {
                return View();
            }
        }

        // GET: SysAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                userInfo = userService.GetUserById(id);
                return View(userInfo);
            }
            catch (Exception)
            {
                logger.Fatal("Can't find user with ID " + id);
                ViewBag.Error = "Can't find user with ID " + id;
                return View();
            }
            
        }

        // POST: SysAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                userService.DeleteUser(id);
                return RedirectToAction("Dashboard");
            }
            catch
            {
                logger.Fatal("Can't delete user with ID " + id);
                ViewBag.Error = "Can't delete user with ID " + id;
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
