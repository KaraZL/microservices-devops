using AutoMapper;
using GeneralStore.API.Data;
using GeneralStore.API.Protos;
using Grpc.Core;
using System.Threading.Tasks;

namespace GeneralStore.API.Grpc
{
    public class GrpcCourseService : GrpcCourse.GrpcCourseBase //From GrpcCourse protos course.proto
    {
        private readonly IStoreRepository _repo;
        private readonly IMapper _mapper;

        public GrpcCourseService(IStoreRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override async Task<CourseResponse> GetAllCourses(GetAllRequest request, ServerCallContext context)
        {
            //On créé une reponse
            var response = new CourseResponse();

            //On recupere les courses
            var courses = await _repo.GetAllCourses();

            //on ajoute les courses
            foreach (var course in courses) {
                response.Course.Add(_mapper.Map<GrpcCourseModel>(course)); //Transforme en message grpc
            }

            return response;
        }

        public override async Task<SingleCourse> GetCourseById(GetCourseByIdRequest request, ServerCallContext context)
        {
            var response = new SingleCourse();

            var course = await _repo.GetCourseById(request.Id); //Request contient les données envoyées

            response.Course = _mapper.Map<GrpcCourseModel>(course); //On transforme en message grpc

            return response;
        }
    }
}
