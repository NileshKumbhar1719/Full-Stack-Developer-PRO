using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using New_PRO.Models;
using WebApplication1.Controllers;

namespace New_PRO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PRO : ControllerBase
    {
        public PRO(ILogger<WeatherForecastController> logger)
        {
            _Logger = logger;
        }

        private static List<Students> _students = new List<Students>
        {
             new Students
                {
                    Id = 1,
                    Name = "Test",
                    Email = "Nilu@gmail.com",
                    Phone = "123434543543"
                },
                new Students
                {
                    Id = 2,
                    Name = "Test",
                    Email = "Nilu@gmail.com",
                    Phone = "123434543543"
                },
                new Students
                {
                    Id = 3,
                    Name = "Test",
                    Email = "Nilu@gmail.com",
                    Phone = "123434543543"
                },
                new Students
                {
                    Id = 4,
                    Name = "Test",
                    Email = "Nilu@gmail.com",
                    Phone = "123434543543"
                }



        };
        private readonly ILogger<WeatherForecastController> _Logger;

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_students);
        }

        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {

            var student = _students.FirstOrDefault(x => x.Id == id);
            if (student == null)
            {
                return NotFound("Student is not serching");
            }
            _Logger.LogInformation("Data Show");
            return Ok(student);
            
        
        }

        [HttpPost]
        public IActionResult CreateData([FromBody] Students student)
        {
            if (student == null)
            {
                return BadRequest("Student data is null");
            }

            
            student.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;

            _students.Add(student);

           
            return CreatedAtAction(nameof(GetbyId), new { id = student.Id }, student);
        }
        [HttpPut("{id}")]
        public IActionResult updateData([FromBody] Students student,int id)
        {
            if (student == null)
            {
                return BadRequest("Student data is null");
            }
            var existingStudent = _students.FirstOrDefault(n => n.Id == id);
            if(existingStudent==null)
            {
                return NotFound("Student not Found");
            }


            
            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Phone = student.Phone;


            return Ok(existingStudent);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            
            var existingStudent = _students.FirstOrDefault(n => n.Id == id);
            if (existingStudent == null)
            {
                return NotFound("Student not Found");
            }
            _students.Remove(existingStudent);



          
            return NoContent();
        }
    }
}