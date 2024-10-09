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

## Application Breakdown

The application has several components, but we will mostly focus on the following four major parts:

* Application Business Model
* Application Architecture Diagram
* Main Database Table Design
* CSV Upload process flow

### Application Business Model

![alt text](https://github.com/Goldin123/Goldin.File.Upload/blob/45a2bf215c66f7829fe3b95f3ad0ae80321608b5/Goldin.File.Upload.Common/Images/FileManagerBusinessModel.jpg?raw=true)

The business model for the entire application is based on the above technologies and concepts:

* The main component is the ASP.NET Web API using .NET 8.
* The application is independent of the OS or device.
* It uses JWT authentication for all authorization and authentication.
* The source code repository used is GitHub.
* An Agile approach is used for new developments or enhancements.
* MS SQL Database is used as the main data storage.
* All technologies adhere to the latest certified security standards.

### Application Architecture Diagram

![alt text](https://github.com/Goldin123/Goldin.File.Upload/blob/45a2bf215c66f7829fe3b95f3ad0ae80321608b5/Goldin.File.Upload.Common/Images/FileManagerArchitechtureDiagram.jpg?raw=true)

The application aims to used the above technologies and concepts.

### Main Database Table Design

![alt text](https://github.com/Goldin123/Goldin.File.Upload/blob/45a2bf215c66f7829fe3b95f3ad0ae80321608b5/Goldin.File.Upload.Common/Images/DataFileDatabaseEntities.jpg?raw=true)

| Name             | Type       | Constraints     | Length |
|------------------|------------|------------------|--------|
| Id               | int        | Primary Key      | n/a    |
| FileName         | varchar    | NULL             | 255    |
| Name             | varchar    | NULL             | 100    |
| Type             | varchar    | NULL             | 50     |
| Search           | bit        | NULL             | n/a    |
| Library Filter    | bit        | NULL             | n/a    |
| Visible          | bit        | NULL             | n/a    |
| UploadedAt       | datetime   | DEFAULT getDate()| n/a    |

### CSV Upload process flow

![alt text](https://github.com/Goldin123/Goldin.File.Upload/blob/45a2bf215c66f7829fe3b95f3ad0ae80321608b5/Goldin.File.Upload.Common/Images/UploadCSVFileWorkflow.jpg?raw=true)
