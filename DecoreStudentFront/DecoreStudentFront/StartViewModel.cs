using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DecoreStudentFront.EventServiceRef;
using DecoreStudentFront.TicketServiceRef;
using DecoreStudentFront.UserServiceRef;

namespace DecoreStudentFront
{
    public class StartViewModel
    {
        public StudentUsers studentUser { get; set; }
        public List<Event> evenList { get; set; }
    }
}