using AutoMapper;
using GeneralStore.API.Data;
using GeneralStore.API.Dtos;
using GeneralStore.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace GeneralStore.API.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scope;
        private readonly IMapper _mapper;

        /* Il est impossible d'injecter une dépendance d'une durée différente dans le subscriber (moins longue) que dans le publisher
         * Le problème est que les autres dépendances d'injection (comme le repository) doivent également avoir une plus grande durée de vie
         * Alors on va appeler les services manuellement
         * Cela dit on peut quand même injecter IMapper
         */
        public EventProcessor(IServiceScopeFactory scope, IMapper mapper)
        {
            _scope = scope;
            _mapper = mapper;
        }

        //Une fois Event définit, on ajoute les données à la BDD ou autre action
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.CoursesPublished:
                    AddCourse(message);
                    break;
                default:
                    break;
            }
        }

        //Détermine la propriété Event de CoursePublishedDto
        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            //On deserialise la propriété event avec une classe créée pour cette tâche
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Courses_Published":
                    Console.WriteLine("--> Course Published EventType Detected");
                    return EventType.CoursesPublished;
                default:
                    Console.WriteLine("--> Could not determine EventType");
                    return EventType.Undetermined;
            }
        }

        //Ajouter course reçue depuis RabbitMQ to DB
        private void AddCourse(string coursePublishedMessage)
        {
            //Ici on récupère les services manuellement
            using (var scope = _scope.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IStoreRepository>();

                //On peut maintenant le déserialiser dans son object car on sait que c'est le bon Event
                var coursePublishedDto = JsonSerializer.Deserialize<CoursePublishedDto>(coursePublishedMessage);

                try
                {
                    var course = _mapper.Map<Course>(coursePublishedDto);
                    repo.CreateCourse(course);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Course to Store DB : {ex.Message}");
                }
            }

        }
    }

    enum EventType
    {
        CoursesPublished,
        Undetermined
    }
}
