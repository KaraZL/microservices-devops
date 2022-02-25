using Courses.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Courses.API.Data
{
    public interface ICoursesRepository
    {
        Task<bool> CreateCourse(Course course);
        Task<bool> DeleteCourse(int id);
        Task<Course> GetCourseById(int id);
        Task<IEnumerable<Course>> GetAllCourses();
    }
}
