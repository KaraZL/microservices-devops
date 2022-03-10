using Courses.API.Models;
using System.Collections.Generic;

namespace Courses.API.Grpc
{
    public interface ICourseDataClient
    {
        IEnumerable<Course> ReturnAllCourses();
    }
}
