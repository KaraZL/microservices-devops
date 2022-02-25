using Courses.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.API.Data
{
    public class SqlCoursesRepository : ICoursesRepository
    {
        private readonly DatabaseContext _context;

        public SqlCoursesRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateCourse(Course course)
        {
            await _context.AddAsync(course);
            var response = await _context.SaveChangesAsync();
            return response > 0;
        }

        public async Task<bool> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            var response = await _context.SaveChangesAsync();
            return response > 0;

        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            var courses = await _context.Course.ToListAsync();
            return courses;
        }

        public async Task<Course> GetCourseById(int id)
        {
            var course = await _context.Course.FindAsync(id);
            return course;
        }
    }
}
