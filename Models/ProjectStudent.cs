namespace product_student_manager.Models
{
    public class ProjectStudent
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
    }
}
