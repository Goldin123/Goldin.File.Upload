# Goldin File Upload

This is a .NET 8 ASP.NET Web API application that employs a clean architecture approach, encouraging best practices and adhering to SOLID principles in its code implementation. An MS SQL Server database serves as the main data store. The application design utilizes class libraries that function as application modules, along with a facade-inspired architecture, where a manager module handles all requests and issues responses.

## Description

The application targets the .NET 8.0 framework, making it independent of the underlying operating system on which it runs or is hosted. All application services and modules use dependency injection when interacting with one another, supported by class libraries that manage these dependencies. The application structure supports modularization, allowing future modules to be independently hosted as NuGet packages for use where relevant.

The chosen ORM is Dapper, as it is lightweight and performs SQL executions more quickly. The main data storage is MS SQL Server, which utilizes stored procedures for all Data Manipulation Language (DML) operations. Additionally, all transactions are only committed once they are successful. A facade design approach is employed, using a manager as the main entry point for all requests, which sends data to the relevant module for processing and provides the user with a response.

To enhance the application’s scalability and maintainability, a comprehensive logging and monitoring system is integrated. This system captures important metrics and error information, enabling developers to identify issues promptly and make data-driven improvements. By leveraging tools like Serilog and Application Insights, the application can provide insights into user behavior and system performance, ensuring a robust and responsive user experience over time.

## Getting Started

### Dependencies

* Latest Visual Studio.
* .net 8.0 SDK
* MS SQL Server v5.1.5
* MS SQL Management Studio 20.2

### Installing

* Clone the repository.
* Run the FileManager DB.sql file located in the SQL folder of the Goldin.File.Upload.Database module to ensure that all database dependencies are created.
* Set Goldin.File.Upload.Api as the startup project.
* Build the application.

### Executing program

* Press F5.
* Swagger UI will load.
