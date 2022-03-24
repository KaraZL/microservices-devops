# microservices-devops

## Courses.API
  - SQL Serveur
  - Rabbitmq Publisher
  - Grpc Client

## GeneralStore.API
  - SQL Serveur
  - Rabbitmq Subscriber
  - Grpc Server
 
## Books.API
  - SQL Serveur avec Stored Procedures
  - Dapper
  - FluentMigrator

## AzureStore.API
  - Azure Service Bus
  - Application Insights

## AzureStore.Functions
  - Azure Service Bus Trigger (Azure Functions)

## Movies.API
  - MediatR (Use MediatR from DataAccess)
  - FluentMigrator

## Movies.DataAccess
  - SQL Server
  - Dapper
  - MediatR
  - Mapper
  - FluentValidation (MediatR Pipeline Behavior)

### RabbitMQ <br>
envoie un message "CoursePublishDto" depuis Courses.API vers GeneralStore.API. (PublishNewCourse dans MessageBusClient) <br>
GS.API récupère le message dans un background service <b>(MessageBusSubscriber)</b>, vérifie son event avec l'attribut Event de <b>"CoursePublishDto"</b> et l'ajoute à la BDD de GS.API <b>(EventProcessing)</b>

### Grpc.Client <br>
envoie une requête HTTP/2 <b>(Courses.API)</b> vers Grpc.Server <b>(GeneralStore.API)</b>. <br>
Cela a pour conséquence d'appeler la méthode correspondante dans <b>CourseDataClient</b>, se connecte sur le serveur et la méthode correspondante dans <b>GrpcCourseService</b> puis retourne la valeur à <b>CourseDataClient</b>.

### Dapper et FluentMigrator <br>
Dapper ne créé pas automatiquement une base de données et ne permet pas des migrations, il est donc nécessaire d'utiliser un autre Package <b>FluentMigrator</b> pour créer des migrations. L'application utilise une connexion SQL sans spécifier la base de données (car elle n'existe pas), crée une base de données, créé une table et la seed puis crée des stored procedures.

### Azure Service Bus <br>
<b>AzureStore.API</b> envoie un message sur le channel d'Azure Service et <b>AzureStore.Functions</b> récupère le message.

### MediatR (Movies) <br>
MediatR est implémenté dans <b>Movies.DataAccess</b> puis <b>Movies.API</b> utilise MediatR pour lancer les méthodes de <b>DataAccess</b>.
Queries (Read), Commands (Write), Handlers (Execute/Logic) <br>
<b>MediatR Pipeline Behavior</b> es utilisé pour valider les données envoyées par les requêtes (Commands). Dans <b>Movies.DataAccess</b>, dans le dossier <b>Behaviors</b>, il y a un validator pour une command et <b>ValidatorBehavior</b> pour traiter tous les validators une fois appelés.


