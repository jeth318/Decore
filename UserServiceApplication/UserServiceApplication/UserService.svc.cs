using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using UserServiceApplication;
using UserService.StudentServiceRef;
using UserService;
using UserService.EmployeeServiceRef;


namespace UserServiceApplication
{

    public class UserService : IUserService
    {
        UserInfo userInfo = new UserInfo();
        Users userModel = new Users();
        StudentInfo studentInfo = new StudentInfo();
        EmployeeInfo employeeInfo = new EmployeeInfo();
        StudentUsers studentUser = new StudentUsers();
        EmployeeServiceWCFClient employeeService = new EmployeeServiceWCFClient();
        StudentServiceClient studentService = new StudentServiceClient();

        UserDbModel db = new UserDbModel();

        public UserInfo CreateUser(UserInfo userInfo)
        {
            var row = (from users in db.Users
                       where users.Email == userInfo.Email
                       select users).FirstOrDefault();

            if (row == null)
            {
                userModel = ConvertToUser(userInfo);

                try
                {
                    db.Users.Add(userModel);
                    db.SaveChanges();
                    userInfo.SuccessfulOperation = true;
                    userInfo.Password = null;
                    return userInfo;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public UserInfo GetUserById(int user_Id)
        {
            try
            {
                userModel = db.Users.Find(user_Id);
                userInfo = ConvertToUserInfo(userModel);
                return userInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public StudentUsers GetStudentUser(int user_Id)
        {
            try
            {
                studentInfo = studentService.GetStudentByUserId(user_Id);
                userModel = db.Users.Find(studentInfo.UserId);
                studentUser = ConvertToStudentUser(userModel, studentInfo);
                return studentUser;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<StudentUsers> GetAllStudentUsers()
        {

            List<StudentInfo> studentInfoList = studentService.GetAllStudents().ToList();
            List<StudentUsers> studentUserList = new List<StudentUsers>();

            for (int i = 0; i < studentInfoList.Count; i++)
            {

                try
                {
                    userModel = db.Users.Find(studentInfoList[i].UserId);

                    if (userModel != null)
                    {
                        StudentUsers studentUser = ConvertToStudentUser(userModel, studentInfoList[i]);
                        studentUserList.Add(studentUser);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return studentUserList;
        }

        public EmployeeUsers GetEmployeetUser(int user_Id)
        {
            try
            {
                userModel = db.Users.Find(employeeInfo.UserId);
                employeeInfo = employeeService.GetEmployeeByUserId(user_Id);
                EmployeeUsers employeeUser = ConvertToEmployeeUser(userModel, employeeInfo);
                return employeeUser;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public UserInfo GetUserBySocSecNum(string socSecNum)
        {
            try
            {
                userModel = (from users in db.Users
                             where users.SocSecNum == socSecNum
                             select users).FirstOrDefault();

                userInfo = ConvertToUserInfo(userModel);

            }
            catch (Exception)
            {
                return userInfo;
            }

            return userInfo;
        }

        public List<UserInfo> GetAllUsers()
        {
            List<Users> userModelList = db.Users.ToList();
            List<UserInfo> userInfoList = new List<UserInfo>();

            for (int i = 0; i < userModelList.Count; i++)
            {
                // Fill up empty user object by find from db              
                userModel = db.Users.Find(userModelList[i].Id);
                // Convert to UserInfo-object
                userInfo = ConvertToUserInfo(userModel);
                // Add to list
                userInfoList.Add(userInfo);
            }

            return userInfoList;
        }

        public UserInfo UpdateUser(UserInfo user)
        {
            if (user.Id > 0)
            {
                userModel = (from x in db.Users
                             where x.Id == user.Id
                             select x).FirstOrDefault();

                userModel = ConvertToUser(user);

                try
                {
                    db.SaveChanges();
                    user = ConvertToUserInfo(db.Users.Find(user.Id));
                    // Hiding password field
                    user.Password = null;
                    user.SuccessfulOperation = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return user;
        }

        public bool DeleteUser(int user_Id)
        {
            userModel = db.Users.Find(user_Id);

            // Calling out to studentService for removal of student with associated User-ID.

            studentInfo = studentService.GetStudentByUserId(user_Id);
            employeeInfo = employeeService.GetEmployeeByUserId(user_Id);

            if (studentInfo != null && employeeInfo != null)
            {
                try
                {
                    studentService.DeleteStudent(studentInfo.Id);
                    employeeService.DeleteEmployee(employeeInfo.Id);
                    db.Users.Remove(userModel);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            else if (studentInfo == null && employeeInfo != null)
            {
                try
                {
                    employeeService.DeleteEmployee(employeeInfo.Id);
                    db.Users.Remove(userModel);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (studentInfo != null && employeeInfo == null)
            {
                try
                {
                    employeeService.DeleteEmployee(employeeInfo.Id);
                    db.Users.Remove(userModel);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        public UserInfo ValidateUser(string email, string password)
        {

            userModel = (from users in db.Users
                         where users.Email == email && users.Password == password
                         select users).FirstOrDefault();

            if (userModel != null)
            {
                userInfo = ConvertToUserInfo(userModel);
                return userInfo;
            }
            else
            {
                return null;
            }
        }


        public bool IsRunning()
        {
            return true;
        }

        private UserInfo ConvertToUserInfo(Users userModel)
        {
            if (userModel != null)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.Id = userModel.Id;
                userInfo.SocSecNum = userModel.SocSecNum;
                userInfo.FirstName = userModel.FirstName;
                userInfo.LastName = userModel.LastName;
                userInfo.Email = userModel.Email;
                userInfo.EmailVerified = userModel.EmailVerified;
                userInfo.TelNum = userModel.TelNum;
                userInfo.SuccessfulOperation = true;

                return userInfo;

            }
            else
            {
                return null;
            }
        }

        private Users ConvertToUser(UserInfo userInfo)
        {
            if (userInfo != null)
            {
                Users userModel = new Users();
                userModel.SocSecNum = userInfo.SocSecNum;
                userModel.FirstName = userInfo.FirstName;
                userModel.LastName = userInfo.LastName;
                userModel.Email = userInfo.Email;
                userModel.EmailVerified = userInfo.EmailVerified;
                userModel.Password = userInfo.Password;
                userModel.TelNum = userInfo.TelNum;

                return userModel;
            }
            else
            {
                return null;
            }
        }

        private StudentUsers ConvertToStudentUser(Users userModel, StudentInfo studentInfo)
        {
            StudentUsers studentUser = new StudentUsers();
            studentUser.Id = userModel.Id;
            studentUser.SocSecNum = userModel.SocSecNum;
            studentUser.FirstName = userModel.FirstName;
            studentUser.LastName = userModel.LastName;
            studentUser.TelNum = userModel.TelNum;
            studentUser.Email = userModel.Email;
            studentUser.EmailVerified = userModel.EmailVerified;
            studentUser.StudentId = studentInfo.Id;
            studentUser.ProgramCode = studentInfo.ProgramCode;
            studentUser.UnionExpiration = studentInfo.UnionExpiration;
            studentUser.UnionName = studentInfo.UnionName;
            studentUser.SuccessfulOperation = true;

            return studentUser;
        }

        public EmployeeUsers ConvertToEmployeeUser(Users userModel, EmployeeInfo employeeInfo)
        {
            EmployeeUsers employeeUser = new EmployeeUsers();
            employeeUser.Id = userModel.Id;
            employeeUser.SocSecNum = userModel.SocSecNum;
            employeeUser.FirstName = userModel.FirstName;
            employeeUser.LastName = userModel.LastName;
            employeeUser.TelNum = userModel.TelNum;
            employeeUser.Email = userModel.Email;
            employeeUser.EmailVerified = userModel.EmailVerified;
            employeeUser.EmployeeInfo = employeeInfo;
            employeeUser.SuccessfulOperation = true;

            return employeeUser;
        }

    }
}
