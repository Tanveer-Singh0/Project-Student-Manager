

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
namespace product_student_manager.Models
{
    public class Student
    {
       
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public ICollection<ProjectStudent> ProjectStudents { get; set; } = new List<ProjectStudent>();
    }
}
