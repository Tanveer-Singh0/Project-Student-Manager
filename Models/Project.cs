
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace product_student_manager.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "";
        public ICollection<ProjectStudent> ProjectStudents { get; set; } = new List<ProjectStudent>();
    }
}
