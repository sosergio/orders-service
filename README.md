# Orders Service

Sample .NET Core application, based on a simplified microservices architecture and Docker containers.

It uses RabbitMQ as a general purpose messaging solution, to allow the web Api to respond to requests quickly instead of being forced to perform resource-heavy procedures while the user waits for the result.


### Tech

The application uses a number of open source projects to work properly:

* [.NET Core](https://www.microsoft.com/net/download) - open-source managed computer software framework 
* [RabbitMQ](http://wwww.rabbitmq.com) - message broker software that originally implemented AMQP 
* [SwaggerUi](https://swagger.io/tools/swagger-ui) - renders documentation for an API defined with the OpenAPI
* [Flurl](http://flurl.io) - Fluent URL building and wrist-friendly HTTP client for .NET.
* [Docker](http://www.docker.com) - performs operating-system-level virtualization
* [MongoDb](http://www.mongodb.com) - cross-platform document-oriented database program
* [XUnit](https://xunit.github.io) - unit testing tool for the .NET framework

### Installation

Orders Service requires [Docker](http://www.docker.com) to run.

Install the dependencies and start the server.

Docker-Compose is a tool for defining and running multi-container Docker applications. Run the following command to create and start all the services at once.

```sh
$ cd orders-service
$ docker-compose build
$ docker-compose run
```

The following service will be started:

| Url | Service |
| ------ | ------ |
| localhost:9001/swagger | OrdersService Web Api |
| localhost:15672 | RabbitMQ Admin (guest:guest) |
| localhost:5111 | Event Bus receiver console app  |
| mongodb://mongodb:27017 | MongoDb


### Api Client

The source code comes togheter with a client class that helps interacting with the service.

The OrdersService.ApiClient class library gives you programmatic access to the OrdersService.WebApi. To make coding against the APIs easier, this client library can reduce the amount of code you need to write and make your code more robust.

The library can also make it simpler to set up authorization and authentication.

```javascript
Task<Order> CreateOrder(CreateOrderRequest request);
Task<List<Order>> ListByUser(OrderRequest request);
Task<Order> LoadById(OrderRequest request);
Task<Order> Cancel(OrderRequest request);
Task<Order> AddItem(AddOrderItemRequest request);
Task<Order> RemoveItem(RemoveOrderItemRequest request);
Task<Order> Submit(OrderRequest request);
```

### Testing

Inside OrdersService.Tests there are 3 types of tests:
- Unit tests
- Integration tests
- Functional tests

They can be run using dotnet command line tools ```$ dotnet test``` or the integrated test manager of Visual Studio.

### Todos

 - Write more Tests
 - Add sample UI
 - Setup CI/CD server 

License
----

MIT

