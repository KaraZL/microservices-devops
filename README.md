# microservices-devops

Courses.API
  - SQL Serveur
  - Rabbitmq Publisher
  - Grpc Client

GeneralStore.API
  - SQL Serveur
  - Rabbitmq Subscriber
  - Grpc Server

RabbitMQ envoie un message "CoursePublishDto" depuis Courses.API vers GeneralStore.API. (PublishNewCourse dans MessageBusClient)
GS.API récupère le message dans un background service (MessageBusSubscriber), vérifie son event avec l'attribut Event de "CoursePublishDto" et l'ajoute à la BDD de GS.API (EventProcessing)

Grpc.Client envoie une requête HTTP/2 (Courses.API) vers Grpc.Server (GeneralStore.API).
Cela a pour conséquence d'appeler la méthode correspondantz dans CourseDataClient, se connecte sur le serveur et la méthode correspondante dans GrpcCourseService puis retourne la valeur à CourseDataClient.
