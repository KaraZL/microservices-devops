using GeneralStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneralStore.API.Data
{
    public class SqlStoreRepository : IStoreRepository
    {
        private readonly DatabaseContext _context;

        public SqlStoreRepository(DatabaseContext context)
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
