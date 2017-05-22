using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using DecoreStudentFront.LoginServiceRef;
using DecoreStudentFront.UserServiceRef;
using DecoreStudentFront.StudentServiceRef;

namespace DecoreStudentFront.Controllers
{
    [AllowAnonymous]
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

            return View(studUser);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserServiceRef.StudentUsers newStudentUser)
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

                TempData["Message"] = "Successfull registration";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Failed when registering the student user";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string userinputLogin, string passwordinputLogin, string returnUrl)
        {
            if (Request.IsAuthenticated)
            {       
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Start", "Student");
                } else
                {
                    return Redirect(returnUrl);
                }
                         
            } else
            {
                try
                {
                    LoginServiceClient loginService = new LoginServiceClient();
                    studentUser = loginService.LoginStudent(userinputLogin, passwordinputLogin);
                    loginService.Close();
                    if (studentUser.SuccessfulOperation == true)
                    {
                        FormsAuthentication.SetAuthCookie(studentUser.Id.ToString(), false);

                       
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        } else
                        {
                            return RedirectToAction("Index");
                        }
                           
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception)
                {
                    TempData["Message"] = "Login failed";
                }
            }
            
            
            /*try
            {
                studentUser = loginService.LoginStudent(userinputLogin, passwordinputLogin);
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
            */
            return RedirectToAction("Index");
        }
    }
}