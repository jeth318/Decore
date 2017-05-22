using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using DecoreStudentFront.LoginServiceRef;
using DecoreStudentFront.UserServiceRef;
using DecoreStudentFront.StudentServiceRef;
using DecoreStudentFront.ViewModels;

namespace DecoreStudentFront.Controllers
{
    public class HomeController : Controller
    {
        UserInfo userInfo = new UserInfo();
        StudentInfo studentInfo = new StudentInfo();
        LoginServiceRef.StudentUsers studentUser = new LoginServiceRef.StudentUsers();
        UserServiceClient userService = new UserServiceClient();
        StudentServiceClient studentService = new StudentServiceClient();


        public ActionResult Index()
        {
            // Passing empty studentUser object for the register-form.
            UserServiceRef.StudentUsers studUser = new UserServiceRef.StudentUsers();
            ViewBag.Message = TempData["Message"];
            ViewBag.Email = TempData["Email"];
          

            var viewModel = new LoginViewModel
            {
                Username = "",
                Password = "",
                studentUser = studUser
            };

            return View(viewModel);
        }

        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }
   
        [HttpPost]
        public ActionResult Register(RegisterViewModel regViewModel)
        {
         
            // Building a StudentUsers-object combining the UserInfo and StudentInfo-objects.
            userInfo.Email = regViewModel.Email;
            userInfo.SocSecNum = regViewModel.SocSecNum;
            userInfo.Password = regViewModel.Password;
            userInfo.FirstName = regViewModel.FirstName;
            userInfo.LastName = regViewModel.LastName;
            userInfo.TelNum = regViewModel.TelNum;
            studentInfo.ProgramCode = regViewModel.ProgramCode;
            studentInfo.UnionName = regViewModel.UnionName;

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

                TempData["Message"] = "Registreringen lyckades!";
                TempData["Email"] = regViewModel.Email;
            
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Registreringen misslyckades";
                ViewBag.Message = TempData["Message"];
                return View(regViewModel);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            LoginServiceClient loginService = new LoginServiceClient();
            
            try
            {
                studentUser = loginService.LoginStudent(loginViewModel.Username, loginViewModel.Password);
                loginService.Close();
                if (studentUser.SuccessfulOperation == true)
                {
                    FormsAuthentication.RedirectFromLoginPage(studentUser.Id.ToString(), false);
                    return null;
                } else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["Message"] = "Login failed";
            }

            return RedirectToAction("Index");
        }
    }
}