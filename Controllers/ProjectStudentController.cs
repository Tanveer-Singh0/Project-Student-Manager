using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using product_student_manager.Models;
using ProjectStudentAPI.Data;

namespace product_student_manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectStudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

       public ProjectStudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{projectId}/students")]
        public async Task<IActionResult> GetStudentsOfProjects(int projectId)
        {
            var students = await _context.ProjectStudents.Where(ps => ps.ProjectId == projectId)
                .Select(ps => ps.Student).ToListAsync();

            if (!students.Any())
                return NotFound("No students are found on this Project");

            return Ok(students);
        }

        [HttpPost("{projectId}/students/{studentId}")]
        public async Task<IActionResult> AddStudentOnProject(int projectId,int studentId)
        {
            var projectExist = await _context.Projects.AnyAsync(p => p.Id == projectId);

            if (!projectExist)
                return NotFound("Project Not Found");

            var studentExist = await _context.Students.AnyAsync(p => p.Id == studentId);

            if (!studentExist)
                return NotFound("Student Not Found");

            var alreadyAssigned = await _context.ProjectStudents.AnyAsync(p => p.ProjectId == projectId && p.StudentId == studentId);

            if (alreadyAssigned)
                return BadRequest("Student already assigned to project ");

            var obj = new ProjectStudent
            {
                ProjectId = projectId,
                StudentId = studentId
            };

            _context.ProjectStudents.Add(obj);
            await _context.SaveChangesAsync();

            return Ok("Student Successfully assigned to teacher");
        }

        [HttpGet("{studentId}/projects")]
        public async Task<IActionResult> GetProjectsOfStudent(int studentId)
        {

            var projects = await _context.ProjectStudents.Where(ps => ps.StudentId == studentId)
                .Select(ps => ps.Project).ToListAsync();

            if (!projects.Any())
                return NotFound("Student is not assigned to any project");

            return Ok(new
            {
                StudentId = studentId,
                TotalProjects = projects.Count,
                Projects = projects
            });
        }
    }
}
