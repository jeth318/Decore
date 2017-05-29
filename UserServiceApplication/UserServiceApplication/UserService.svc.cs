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
using log4net;


namespace UserServiceApplication
{

    public class UserService : IUserService
    {
        UserInfo userInfo = new UserInfo();
        Users userModel = new Users();
        StudentInfo studentInfo = new StudentInfo();
        EmployeeInfo employeeInfo = new EmployeeInfo();
        EmployeeServiceWCFClient employeeService = new EmployeeServiceWCFClient();
        StudentServiceClient studentService = new StudentServiceClient();

        UserDbModel db = new UserDbModel();

        private static readonly ILog logger = LogManager.GetLogger("TestLogger");



        public UserInfo CreateUser(UserInfo user)
        {
            logger.Debug("Get user was called");
            var row = (from users in db.Users
                       where users.Email == user.Email
                       select users).FirstOrDefault();

            if (row == null)
            {
                userModel.SocSecNum = user.SocSecNum;
                userModel.FirstName = user.FirstName;
                userModel.LastName = user.LastName;
                userModel.Email = user.Email;
                userModel.EmailVerified = user.EmailVerified;
                userModel.Password = user.Password;
                userModel.StudentId = user.StudentId;
                userModel.EmployeeId = user.EmployeeId;
                userModel.TelNum = user.TelNum;

                try
                {
                    db.Users.Add(userModel);
                    db.SaveChanges();
                    user.SuccessfulOperation = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return user;
        }

        public UserInfo GetUserById(int user_Id)
        {
            logger.Debug("Get user was called");
            try
            {
                //userModel = db.Users.Find(user_Id);

                userModel = (from users in db.Users
                             where users.Id == user_Id
                             select users).FirstOrDefault();


                userInfo.Id = userModel.Id;
                userInfo.SocSecNum = userModel.SocSecNum;
                userInfo.FirstName = userModel.FirstName;
                userInfo.LastName = userModel.LastName;
                userInfo.Email = userModel.Email;
                userInfo.EmailVerified = userModel.EmailVerified;
                userInfo.StudentId = userModel.StudentId;
                userInfo.EmployeeId = userModel.EmployeeId;
                userInfo.TelNum = userModel.TelNum;
                userInfo.SuccessfulOperation = true;
            }
            catch (Exception)
            {

            }

            return userInfo;
        }

        public StudentUsers GetStudentUser(int user_Id)
        {
            logger.Debug("Get user was called");
            try
            {
                studentInfo = studentService.GetStudentByUserId(user_Id);
                userModel = db.Users.Find(studentInfo.UserId);
            }
            catch (Exception)
            {

            }

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
                        StudentUsers studentUser = new StudentUsers();

                        studentUser.Id = userModel.Id;
                        studentUser.SocSecNum = userModel.SocSecNum;
                        studentUser.FirstName = userModel.FirstName;
                        studentUser.LastName = userModel.LastName;
                        studentUser.TelNum = userModel.TelNum;
                        studentUser.Email = userModel.Email;
                        studentUser.EmailVerified = userInfo.EmailVerified;
                        studentUser.StudentId = studentInfoList[i].Id;
                        studentUser.ProgramCode = studentInfoList[i].ProgramCode;
                        studentUser.UnionExpiration = studentInfoList[i].UnionExpiration;
                        studentUser.UnionName = studentInfoList[i].UnionName;

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
            EmployeeUsers employeeUser = new EmployeeUsers();

            employeeInfo = employeeService.GetEmployeeByUserId(user_Id);
            if (employeeInfo == null)
                return null;

            userModel = db.Users.Find(employeeInfo.UserId);


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


        public UserInfo GetUserBySocSecNum(string socSecNum)
        {
            try
            {
                userModel = (from users in db.Users
                             where users.SocSecNum == socSecNum
                             select users).FirstOrDefault();

                userInfo.Id = userModel.Id;
                userInfo.SocSecNum = userModel.SocSecNum;
                userInfo.FirstName = userModel.FirstName;
                userInfo.LastName = userModel.LastName;
                userInfo.Email = userModel.Email;
                userInfo.EmailVerified = userModel.EmailVerified;
                userInfo.StudentId = userModel.StudentId;
                userInfo.EmployeeId = userModel.EmployeeId;
                userInfo.TelNum = userModel.TelNum;
                userInfo.SuccessfulOperation = true;
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
                UserInfo userInfo = new UserInfo();
                userInfo.Id = userModelList[i].Id;
                userInfo.SocSecNum = userModelList[i].SocSecNum;
                userInfo.FirstName = userModelList[i].FirstName;
                userInfo.LastName = userModelList[i].LastName;
                userInfo.Email = userModelList[i].Email;
                userInfo.EmailVerified = userModelList[i].EmailVerified;
                userInfo.StudentId = userModelList[i].StudentId;
                userInfo.EmployeeId = userModelList[i].EmployeeId;
                userInfo.TelNum = userModelList[i].TelNum;
                userInfo.SuccessfulOperation = true;
                userInfoList.Add(userInfo);

            }

            return userInfoList;
        }

        public UserInfo UpdateUser(UserInfo user)
        {

            userModel = db.Users.Find(user.Id);
            if (userModel != null)
            {
                userModel = (from x in db.Users
                             where x.Id == user.Id
                             select x).FirstOrDefault();


                userModel.Id = userModel.Id;
                userModel.FirstName = user.FirstName;
                userModel.LastName = user.LastName;
                userModel.Email = userModel.Email;
                userModel.SocSecNum = userModel.SocSecNum;
                userModel.Password = userModel.Password;
                userModel.EmailVerified = userModel.EmailVerified;
                userModel.StudentId = userModel.StudentId;
                userModel.EmployeeId = userModel.EmployeeId;
                userModel.TelNum = user.TelNum;

                try
                {
                    db.SaveChanges();
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

            if (userModel.StudentId != null)
            {
                studentService.DeleteStudent(userModel.StudentId);
            }
            db.Users.Remove(userModel);
            db.SaveChanges();

            return true;
        }

        public UserInfo ValidateUser(string email, string password)
        {

            userModel = (from users in db.Users
                         where users.Email == email && users.Password == password
                         select users).FirstOrDefault();

            if (userModel == null)
                return null;

            userInfo.Id = userModel.Id;
            userInfo.SocSecNum = userModel.SocSecNum;
            userInfo.FirstName = userModel.FirstName;
            userInfo.LastName = userModel.LastName;
            userInfo.Email = userModel.Email;
            userInfo.EmailVerified = userModel.EmailVerified;
            userInfo.StudentId = userModel.StudentId;
            userInfo.EmployeeId = userModel.StudentId;
            userInfo.TelNum = userModel.TelNum;
            userInfo.SuccessfulOperation = true;

            return userInfo;
        }

        public bool SetUserStudentId(int User_Id, int? student_Id)
        {
            bool result = false;


            userModel = db.Users.Find(User_Id);

            if (student_Id == null)
            {
                userModel.StudentId = null;

            }
            else
            {
                var row = (from users in db.Users
                           where users.StudentId == student_Id
                           select users).FirstOrDefault();

                if (row == null)
                {
                    userModel.StudentId = student_Id;
                }
            }

            try
            {
                db.SaveChanges();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public bool SetUserEmployeeId(int User_Id, int? employee_Id)
        {
            bool result = false;
            userModel = db.Users.Find(User_Id);

            if (employee_Id == null)
            {
                userModel.EmployeeId = null;
            }
            else
            {
                var row = (from users in db.Users
                           where users.EmployeeId == employee_Id
                           select users).FirstOrDefault();

                if (row == null)
                {
                    userModel.EmployeeId = employee_Id;
                }
            }
            try
            {
                db.SaveChanges();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public bool IsRunning()
        {
            logger.Debug("Is running  was called");
            return true;
        }
    }
}
