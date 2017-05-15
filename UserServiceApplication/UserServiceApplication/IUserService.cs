using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using UserService.EmployeeServiceRef;

namespace UserServiceApplication
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        UserInfo CreateUser(UserInfo user);

        [OperationContract]
        UserInfo GetUserById(int user_Id);

        [OperationContract]
        StudentUsers GetStudentUser(int user_Id);

        [OperationContract]
        List<StudentUsers> GetAllStudentUsers();

        [OperationContract]
        EmployeeUsers GetEmployeetUser(int user_Id);

        [OperationContract]
        UserInfo GetUserBySocSecNum(string socSecNum);

        [OperationContract]
        List<UserInfo> GetAllUsers();

        [OperationContract]
        UserInfo UpdateUser(UserInfo user);

        [OperationContract]
        UserInfo ValidateUser(string email, string password);

        [OperationContract]
        bool DeleteUser(int user_Id);

        [OperationContract]
        bool SetUserStudentId(int user_Id, int? student_Id);

        [OperationContract]
        bool SetUserEmployeeId(int user_Id, int? employee_Id);

        [OperationContract]
        bool IsRunning();

    }

    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? StudentId { get; set; }
        [DataMember]
        public int? EmployeeId { get; set; }
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
        public string Password { get; set; }
        [DataMember]
        public string TelNum { get; set; }
        [DataMember]
        public bool SuccessfulOperation { get; set; }
    }

    [DataContract]
    public class StudentUsers
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? StudentId { get; set; }
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
        public string Password { get; set; }

        [DataMember]
        public string UnionName { get; set; }
        [DataMember]
        public DateTime? UnionExpiration { get; set; }
        [DataMember]
        public string ProgramCode { get; set; }
        [DataMember]
        public bool SuccessfulOperation { get; set; }



    }

    [DataContract]
    public class EmployeeUsers
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? EmployeeId { get; set; }
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
        public string Password { get; set; }
        [DataMember]
        public EmployeeInfo EmployeeInfo { get; set; }
        [DataMember]
        public bool SuccessfulOperation { get; set; }
    }
}
