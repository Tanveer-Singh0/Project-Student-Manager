

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace product_student_manager.Models
{
    public class Student
    { 
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public ICollection<ProjectStudent> ProjectStudents { get; set; } = new List<ProjectStudent>();
    }
}
