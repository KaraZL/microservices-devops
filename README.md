# microservices-devops

## Courses.API
  - SQL Serveur
  - Rabbitmq Publisher
  - Grpc Client

## GeneralStore.API
  - SQL Serveur
  - Rabbitmq Subscriber
  - Grpc Server

### RabbitMQ <br>
envoie un message "CoursePublishDto" depuis Courses.API vers GeneralStore.API. (PublishNewCourse dans MessageBusClient) <br>
GS.API récupère le message dans un background service <b>(MessageBusSubscriber)</b>, vérifie son event avec l'attribut Event de <b>"CoursePublishDto"</b> et l'ajoute à la BDD de GS.API <b>(EventProcessing)</b>

### Grpc.Client <br>
envoie une requête HTTP/2 <b>(Courses.API)</b> vers Grpc.Server <b>(GeneralStore.API)</b>. <br>
Cela a pour conséquence d'appeler la méthode correspondante dans <b>CourseDataClient</b>, se connecte sur le serveur et la méthode correspondante dans <b>GrpcCourseService</b> puis retourne la valeur à <b>CourseDataClient</b>.
