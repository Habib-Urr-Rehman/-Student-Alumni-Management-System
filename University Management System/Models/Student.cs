using System;
using System.Collections.Generic;

namespace University_Management_System.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string RollNo { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Session { get; set; }
        public string Degree { get; set; } = null!;
    }
}
