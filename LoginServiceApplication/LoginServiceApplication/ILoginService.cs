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


    /*
    [DataContract]
    public class EmployeeUsers
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? Student_Id { get; set; }

        [DataMember]
        public int? Personal_Id { get; set; }

        [DataMember]
        public string SocSecNum { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public bool EmailVerified { get; set; }

        [DataMember]
        public string TelNum { get; set; }

        [DataMember]
        public EmployeeInfo EmployeeInfo { get; set; }
    }*/



}
