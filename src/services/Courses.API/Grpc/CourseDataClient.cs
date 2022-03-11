using AutoMapper;
using Courses.API.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using Grpc.Net.Client;
using Courses.API.Protos;

namespace Courses.API.Grpc
{
    public class CourseDataClient : ICourseDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CourseDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        private GrpcCourse.GrpcCourseClient ConnectToGrpc()
        {
            Console.WriteLine($"--> Calling Grpc Service {_configuration["GrpcCourseServer"]}");

            //talk with Grpc
            //Grpc server address
            var channel = GrpcChannel.ForAddress(_configuration["GrpcCourseServer"]);
            var client = new GrpcCourse.GrpcCourseClient(channel); //GrpcCourseClient is from the nuget itself

            return client;
        }

        public Course GetCourseById(int id)
        {
            var client = ConnectToGrpc(); // On se connecte

            var request = new GetCourseByIdRequest { Id = id }; // On cree une requete

            try
            {
                var reply = client.GetCourseById(request);
                Console.WriteLine($"--> Connected to GRPC server and received : {reply.Course.Name}");
                return _mapper.Map<Course>(reply.Course);
            }
            catch (Exception ex )
            {
                Console.WriteLine($"--> Couldn't call Grpc Server : {ex.Message}");
                return null;
            }
        }

        public IEnumerable<Course> ReturnAllCourses()
        {
            var client = ConnectToGrpc();

            var request = new GetAllRequest(); //cree une requete pour la fonction getallcourses

            try
            {
                var reply = client.GetAllCourses(request);
                Console.WriteLine($"--> Connected to GRPC server and received : {reply.Course.Count}");
                return _mapper.Map<IEnumerable<Course>>(reply.Course);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't call Grpc Server : {ex.Message}");
                return null;
            }
        }
    }
}
