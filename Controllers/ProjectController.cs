using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using product_student_manager.Models;
using ProjectStudentAPI.Data;

namespace product_student_manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _context.Projects.ToListAsync();
            return Ok(projects);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound("Project Not Found");

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project)
        {
            if (await _context.Projects.AnyAsync(p => p.Title == project.Title))
                return BadRequest("Project name must be unique");

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        [HttpPut("{id}")]   
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            var existingProject = await _context.Projects.FindAsync(id);

            if (existingProject == null)
                return NotFound("Project not found");

            existingProject.Title = project.Title;
            await _context.SaveChangesAsync();

            return Ok(existingProject);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound("Project not found");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok("Project deleted successfully");
        }

    }
}
