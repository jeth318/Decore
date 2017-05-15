using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using StudentServiceApplication.UserServiceRef;

namespace StudentServiceApplication
{ 
    public class StudentService : IStudentService
    {
        // Database entity
        StudentDbModel db = new StudentDbModel();
        // DataContract entity of Student. Will be returned as response.
        StudentInfo studentInfo = new StudentInfo();
        // Model connected via EntityFramework
        Students studentModel = new Students();
        // Fetched from UserService
        UserInfo userInfo = new UserInfo();

        UserServiceClient userService = new UserServiceClient();

        public StudentInfo CreateStudent(StudentInfo studentInfo)
        {
            if (PreventDuplicate(studentInfo.UserId))
            {
                UserInfo userInfo = userService.GetUserById(studentInfo.UserId);
                if (userInfo.SuccessfulOperation)
                {
                    Students studentModel = new Students();
                    studentModel.UserId = studentInfo.UserId;
                    studentModel.UnionName = studentInfo.UnionName;
                    studentModel.ProgramCode = studentInfo.ProgramCode;
                    studentModel.UnionExpiration = studentInfo.UnionExpiration;

                    try
                    {
                        db.Students.Add(studentModel);
                        db.SaveChanges();
                        studentModel = db.Students.Find(studentModel.Id);
                        userService.SetUserStudentId(studentModel.UserId, studentModel.Id);
                        studentInfo.Id = studentModel.Id;
                        studentInfo.SuccessfulOperation = true;
                    }
                    catch (Exception)
                    {
                        return studentInfo;
                    }            
                }          
            }
         
            return studentInfo;
            
        }

        public bool DeleteStudent(int? student_Id)
        {
            bool result = false;
            var studentModel = (from student in db.Students
                      where student.Id == student_Id
                      select student).FirstOrDefault();
 
            try
            {
                userService.SetUserStudentId(studentModel.UserId, null);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                db.Students.Remove(studentModel);
                db.SaveChanges();
                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public StudentInfo GetStudentByUserId(int user_Id)
        {
            try
            {
                studentModel = (from students in db.Students
                                where students.UserId == user_Id
                                select students).FirstOrDefault();
              
                studentInfo.Id = studentModel.Id;
                studentInfo.UserId = studentModel.UserId;
                studentInfo.UnionName = studentModel.UnionName;
                studentInfo.UnionExpiration = studentModel.UnionExpiration;
                studentInfo.SuccessfulOperation = true;
            }
            catch (Exception)
            {
                return studentInfo;
            }
            return studentInfo;
        }

        public StudentInfo UpdateStudent(StudentInfo studentInfo)
        {     
            studentModel = db.Students.Find(studentInfo.Id);
            if (studentModel != null)
            {
                studentModel = (from x in db.Students
                             where x.Id == studentModel.Id
                             select x).FirstOrDefault();

                studentModel.ProgramCode = studentInfo.ProgramCode;
                studentModel.UnionExpiration = studentInfo.UnionExpiration;
                studentModel.UnionName = studentInfo.UnionName;
                studentInfo.SuccessfulOperation = true;
               
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return studentInfo;
                }
            }

            return studentInfo;
        }

        bool PreventDuplicate(int user_id)
        {
            bool result = false;

            studentModel = (from students in db.Students
                      where students.UserId == user_id
                      select students).FirstOrDefault();

            if(studentModel == null)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public List<StudentInfo> GetAllStudents()
        {
            List<Students> studentModelList = db.Students.ToList();
            List<StudentInfo> studentInfoList = new List<StudentInfo>();


            for (int i = 0; i < studentModelList.Count; i++)
            {

                StudentInfo studentInfo = new StudentInfo();
                studentInfo.Id = studentModelList[i].Id;
                studentInfo.ProgramCode = studentModelList[i].ProgramCode;
                studentInfo.UnionExpiration = studentModelList[i].UnionExpiration;
                studentInfo.UnionName = studentModelList[i].UnionName;
                studentInfo.UserId = studentModelList[i].UserId;
                studentInfoList.Add(studentInfo);
            }
            return studentInfoList;
        }

        public bool IsRunning()
        {
            return true;
        }
    }
}
