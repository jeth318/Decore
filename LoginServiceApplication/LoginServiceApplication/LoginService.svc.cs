using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using LoginServiceApplication.StudentServiceRef;
using LoginServiceApplication.UserServiceRef;
using LoginServiceApplication.EmployeeServiceRef;

namespace LoginServiceApplication
{
    public class LoginService : ILoginService
    {
        UserServiceClient userService = new UserServiceClient();
        Service1Client employeeService = new Service1Client();
        StudentServiceClient studentService = new StudentServiceClient();
        UserInfo user = new UserInfo();
        EmployeeUsers employeeUser = new EmployeeUsers();
        StudentUsers studentUser = new StudentUsers();

        public StudentUsers LoginStudent(string email, string password)
        {
            try
            {
                user = userService.ValidateUser(email, password);
                studentUser = userService.GetStudentUser(user.Id);
                studentUser.SuccessfulOperation = true;
            }
            catch (Exception)
            {
                      
            }
            
            return studentUser;
        }

        public EmployeeUsers LoginEmployee(string email, string password)
        {
            try
            {
                user = userService.ValidateUser(email, password);
                employeeUser = userService.GetEmployeetUser(user.Id);
                employeeUser.SuccessfulOperation = true;
            }
            catch (Exception)
            {

                throw;
            }
                               
            return employeeUser;

        }

        public bool IsRunning()
        {
            return true;
        }
    }
}
