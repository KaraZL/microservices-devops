using Courses.API.Dtos;

namespace Courses.API.MessageBus
{
    public interface IMessageBusClient
    {
        void PublishNewCourse(CoursePublishedDto courseDto);
    }
}
