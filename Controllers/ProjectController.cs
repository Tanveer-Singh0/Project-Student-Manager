using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using product_student_manager.Models;
using ProjectStudentAPI.Data;
using System.Text.Json;

namespace product_student_manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly string filePath = "ID.json";

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{pageSize:int}/{pageNumber:int}")]
        public async Task<IActionResult> GetProjects(int pageSize,int pageNumber)
        {

            var totalProjects = await _context.Projects.CountAsync();

            var projects = await _context.Projects
                .OrderBy(p => p.Id).Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return Ok(new
            {
                pageNumber,
                pageSize,
                totalProjects,
                totalPages = (int)Math.Ceiling(totalProjects / (double)pageSize),
                data = projects
            });
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

            var json = System.IO.File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, int>>(json)!;

            int nextId = data["ProjectId"] + 1;
            data["ProjectId"] = nextId;

            System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));

            project.Id = nextId;
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
