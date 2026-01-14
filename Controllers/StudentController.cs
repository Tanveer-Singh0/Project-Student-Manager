using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using product_student_manager.Models;
using ProjectStudentAPI.Data;
using System.Text.Json;

namespace product_student_manager.Controllers
{



    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private static readonly string filePath = "ID.json";
        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet ("{pageSize:int}/{pageNumber:int}")]
        public async Task<IActionResult> GetStudents(int pageNumber,int pageSize)
        {

            var totalStudents = await _context.Students.CountAsync();

            var students = await _context.Students
                .OrderBy(p => p.Id).Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return Ok(new
            {
                pageNumber,
                pageSize,
                totalStudents,
                totalPages = (int)Math.Ceiling(totalStudents / (double)pageSize),
                data = students
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound("Student Not Found");

            return Ok(student);

        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            if (await _context.Students.AnyAsync(p => p.Name == student.Name))
                return BadRequest("Student Name should be unique");

            var json = System.IO.File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, int>>(json)!;

            int nextId = data["StudentId"] + 1;
            data["StudentId"] = nextId;

            System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
            student.Id = nextId;
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        [HttpPut("{id}")]
         public async Task<IActionResult> UpdateStudent(int id,Student student)
        {
            var haveStudent =await _context.Students.FindAsync(id);
            if (haveStudent == null)
                return NotFound("Student id not found");

            haveStudent.Name = student.Name;
            await _context.SaveChangesAsync();
            return Ok(haveStudent);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var haveStudent = await _context.Students.FindAsync(id);

            if (haveStudent == null)
                return NotFound("Id not found ");

            _context.Students.Remove(haveStudent);
            await _context.SaveChangesAsync();
            return Ok("Student deleted");
        }

        
        }
}
