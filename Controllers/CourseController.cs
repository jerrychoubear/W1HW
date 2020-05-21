using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using W1HW.Models;

namespace W1HW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ContosoUniversityContext context;
        private string GetCoursesUrl => Url.Action(nameof(GetCourses));
        private string GetCourseByIdUrl(int id) => Url.Action(nameof(GetCourseById), new { id });

        public CourseController(ContosoUniversityContext context)
        {
            this.context = context;
        }

        // GET api/Course
        [HttpGet("")]
        public ActionResult<IEnumerable<Course>> GetCourses()
        {            
            return context.Course.ToList();
        }

        // GET api/Course/5
        [HttpGet("{id}")]
        public ActionResult<Course> GetCourseById(int id)
        {
            return context.Course.FirstOrDefault(x => x.CourseId == id);
        }

        // POST api/Course
        [HttpPost("")]
        public IActionResult PostCourse(Course value)
        {
            if (string.IsNullOrWhiteSpace(value.Title)) return BadRequest(new { Message = "Title不可為空" });

            context.Course.Add(value);
            context.SaveChanges();

            return Created(GetCourseByIdUrl(value.CourseId), value);
        }

        // PUT api/Course/5
        [Route("{id}")]
        [HttpPut("{id}")]
        public IActionResult PutCourse(int id, Course value)
        {
            if (value == null) return BadRequest(new { Message = "Course不可為空" });

            var ret = context.Course.Find(id);
            if (ret == null)
            {
                context.Course.Add(value);
                context.SaveChanges();
                return Created(Url.Action(nameof(GetCourseById), new { id = value.CourseId }), value);
            }
            else
            {
                ret.Title = value.Title;
                context.SaveChanges();
                return Ok();
            }
        }

        // DELETE api/Course/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCourseById(int id)
        {
            var foundValue = context.Course.Find(id);
            if (foundValue == null) return NotFound(new { Message = "查無Course" });

            context.Course.Remove(foundValue);
            context.SaveChanges();

            return NoContent();
        }
    }
}