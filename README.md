Customer Order API is an API which manages Products, Customers and their Orders.

Customer Order API enable adding/deleting/updating Customers along with adding a new Order and listing them. Also managing the Products.

Solution is created based on Clean Architecture and have 4 main projects, Domain, Application, Infrastructure and API or Presentation.
The solution is developed with Domain Driven Design principles.

I have used Entity Framework Core and its features to design the database and the tables based on Domain Driven Design.
I have used Mediator to centralize the communication between components and CQRS pattern to separate the write and read business casess.
I have used UnitOfWork to manage transactions and to ensure consistency in data operations.
I have used Serilog for structured logging.

I have assumed that an Order cannot be created without a Customer, thats why the Customer will be an Aggregate Root and the Order will be Entity.
I have assumed that an OrderItem does not have any distinct identity that is important outside the context of the order, thats why I decided to let it as ValueObject.



