using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentServiceApplication
{
    public class Employee
    {
        public int Id { get; set; }
        public int User { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}