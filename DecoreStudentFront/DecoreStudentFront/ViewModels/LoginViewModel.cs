using DecoreStudentFront.UserServiceRef;
namespace DecoreStudentFront.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public StudentUsers studentUser { get; set; }
    }
}