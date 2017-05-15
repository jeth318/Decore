using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace StudentServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IStudentService
    {

        [OperationContract]
        StudentInfo CreateStudent(StudentInfo student);

        [OperationContract]
        StudentInfo GetStudentByUserId(int user_Id);

        [OperationContract]
        List<StudentInfo> GetAllStudents();
      

        [OperationContract]
        bool DeleteStudent(int? student_Id);

        [OperationContract]
        StudentInfo UpdateStudent(StudentInfo updatedStudent);

        [OperationContract]
        bool IsRunning();



        // TODO: Add your service operations here
    }



    [DataContract]
    public class StudentInfo
    {   [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string UnionName { get; set; }
        [DataMember]
        public DateTime? UnionExpiration { get; set; }
        [DataMember]
        public string ProgramCode { get; set; }
        [DataMember]
        public bool SuccessfulOperation { get; set; }
    }

}
