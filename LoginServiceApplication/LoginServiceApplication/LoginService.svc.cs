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
        EmployeeServiceWCFClient employeeService = new EmployeeServiceWCFClient();
        StudentServiceClient studentService = new StudentServiceClient();
        UserInfo user = new UserInfo();
        EmployeeUsers employeeUser = new EmployeeUsers();
        StudentUsers studentUser = new StudentUsers();

        public StudentUsers LoginStudent(string email, string password)
        {

            user = userService.ValidateUser(email, password);

            if (user != null)
            {
                studentUser = userService.GetStudentUser(user.Id);
                studentUser.SuccessfulOperation = true;
            }
            return studentUser;
        }

        public EmployeeUsers LoginEmployee(string email, string password)
        {
            user = userService.ValidateUser(email, password);
            if (user == null)
                return null;

            employeeUser = userService.GetEmployeetUser(user.Id);
            if (employeeUser == null)
                return null;

            employeeUser.SuccessfulOperation = true;
            return employeeUser;

        }

        public bool IsRunning()
        {
            return true;
        }
    }
}
