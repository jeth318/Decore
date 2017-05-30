using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using DecoreSysAdminFront.LoginServiceRef;
using log4net;



namespace DecoreSysAdminFront.Controllers
{
    public class HomeController : Controller
    {

        private static readonly ILog logger = LogManager.GetLogger("SysAdminLogger");


        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userinputLogin, string passwordinputLogin)
        {
            LoginServiceClient loginService = new LoginServiceClient();

            EmployeeUsers employeeUser = new EmployeeUsers();
            employeeUser = loginService.LoginEmployee(userinputLogin, passwordinputLogin);
            loginService.Close();

            try
            {
                if(employeeUser.EmployeeInfo.Roles.Any(role => role.Access.Any(access => access.Name.Contains("super_admin"))))
                {
                    System.Web.HttpContext.Current.Session["user_session_string"] = employeeUser.Email;
                    FormsAuthentication.RedirectFromLoginPage(employeeUser.Id.ToString(), false);
                    return null;
                } else if (employeeUser != null)
                {
                    logger.Fatal("Failed to login:" + userinputLogin);
                    
                    TempData["ErrorMessage"] = "Inloggningen misslyckades. Kontrollera att kontot har systemadminbehörighet";
                    return new RedirectResult(Url.Action("Index") + "#login");
                } else
                {
                    TempData["ErrorMessage"] = "Inloggningen misslyckades. Användaren finns ej eller saknar systemadminbehörighet";
                    return new RedirectResult(Url.Action("Index") + "#login");
                }
                
                 
            }
            catch (Exception)
            {

                logger.Fatal("Failed to login:" + userinputLogin);
                TempData["ErrorMessage"] = "Inloggningen misslyckades. Användaren finns ej eller saknar systemadminbehörighet";
                return new RedirectResult(Url.Action("Index") + "#login");
            }
                                
        }            
    }
}