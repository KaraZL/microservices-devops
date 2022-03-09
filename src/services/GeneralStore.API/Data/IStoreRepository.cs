using GeneralStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneralStore.API.Data
{
    public interface IStoreRepository
    {
        Task<bool> CreateCourse(Course course);
        Task<bool> DeleteCourse(int id);
        Task<Course> GetCourseById(int id);
        Task<IEnumerable<Course>> GetAllCourses();
    }
}
