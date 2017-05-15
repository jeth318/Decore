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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    

    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        StudentUsers LoginStudent(string email, string password);

        [OperationContract]
        EmployeeUsers LoginEmployee(string email, string password);

        [OperationContract]
        bool IsRunning();
    }
}
