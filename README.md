# Verwalter: An Address Management System

A full-stack address management application built with .NET, ASP.NET Core Web API, and WPF. This project explores REST API development, relational database design, and clean architecture in a maintainable client/server application.

### Features

- Create, edit, and view addresses and related entities.
- Search and filter addresses by criteria such as street, city, and country.
- Paginate address lists to support larger datasets.
- Manage related entities, including countries, cities, places, postcodes, and address categories.
- Basic authentication functionality.
- WPF desktop interface following the MVVM pattern.

### Technology Stack

- **WPF / XAML**              — Desktop UI and styling
- **ASP.NET Core Web API**    — REST API
- **C# / .NET**               — Application development
- **Entity Framework Core**   — Database access and persistence
- **SQL database**            — Data storage

### Architecture

The solution is split into projects with clearly defined responsibilities:

```text
- ClientWPF        # Desktop UI and view models
- APIContracts     # API request and response models
- API              # HTTP endpoints and controllers
- Application      # Application services and interfaces
- Infrastructure   # Database entities and persistence
```

The application separates API contracts, application logic, and persistence models to reduce coupling and make individual components easier to maintain, test, or replace.

The database model represents addresses and related entities separately, with an emphasis on normalisation and meaningful relationships.

### Project Motivation

This is a personal learning and portfolio project focused on writing clear, maintainable code and building a foundation that can be extended with additional functionality. The project provides hands-on experience with:

- Designing and implementing REST APIs.
- Integrating a desktop client with a backend service.
- Relational data modelling and Entity Framework Core.
- Separation of concerns and dependency inversion.
- Search, filtering, and pagination.
- MVVM and automated testing.

### Future Improvements

- Expand unit and integration test coverage.
- Improve validation and error handling.
- Extend authentication and authorisation.
- Add API documentation and further search capabilities.


## Installation and setup

### Prerequisites

Install the following software:

- Visual Studio 2022.
- The .NET desktop development workload in Visual Studio.
- The .NET 8 SDK.
- SQL Server Express.
- SQL Server Management Studio (SSMS).

### 1. Install SQL Server Express

1. Install [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).
2. During installation, create or select an instance named `SQLEXPRESS`.
3. Make sure the SQL Server Express service is running.

The application uses Windows authentication. No separate SQL Server username or password is required.

### 2. Create the database

1. Install [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/ssms/install/install)
2. Open SQL Server Management Studio.
3. Connect to the SQL Server instance using:

   - **Server type:** `Database Engine`
   - **Server name:** `.\SQLEXPRESS`
   - **Authentication:** `Windows Authentication`

4. Open the database setup script from the repository, found under: `Verwalter\Database\Scaffolds\001.01 - Initial Database Setup.sql`
5. Execute the script.
6. Confirm that the `AddressDatabase` database appears under **Databases** in the SSMS Object Explorer.

The repository contains a database seed script if sample data is required. Execute the seed script after the database setup script. 
It can be found under: `Verwalter\Database\Scaffolds\001.03 - Insert Test Addresses.sql`

The application can also be run against an empty database.

### 3. Open the solution in Visual Studio

1. Install Visual Studio. [Download 2026](https://visualstudio.microsoft.com/downloads/)
2. Open the repository directory in Visual Studio.
3. Open the solution file.
4. Allow Visual Studio to restore the NuGet packages.
5. In **Solution Explorer**, right-click the solution and select **Build Solution**.

The solution targets .NET 8. If Visual Studio reports that the .NET 8 SDK is unavailable, install the .NET 8 SDK and restart Visual Studio.

### 4. Verify the database connection

The `Infrastructure` project contains the Entity Framework Core database configuration, found under:
`Verwalter\Infrastructure\Persistence\Entities\AddressDatabaseContext.cs`

Verify that its SQL Server connection string refers to the expected database and server:
`Server=.\SQLEXPRESS;Database=AddressDatabase;Integrated Security=True;TrustServerCertificate=True`

If SQL Server is running under a different instance name, update only the `Server=` portion so that it matches the server name used to connect in SSMS. For example:
`Server=localhost;Database=AddressDatabase;Integrated Security=True;TrustServerCertificate=True`

### 5. Configure the startup projects

The application consists of two executable projects:
- `API`
- `ClientWPF`

Configure Visual Studio to start both projects:

1. Right-click the solution in **Solution Explorer**.
2. Select **Configure Startup Projects**.
3. Select **Multiple startup projects**.
4. Set the action for both `API` and `ClientWPF` to **Start**.
5. Move `API` above `ClientWPF` in the startup-project list if necessary.
6. Select **OK**.

The API exposes the HTTP endpoints used by the WPF client. The WPF client uses its API settings to determine which API URL to call.
The `BaseUrl` can be changed here: `Verwalter\ClientWPF\appsettings.json`

### 6. Run the application

1. Confirm that SQL Server Express is running.
2. Confirm that the `AddressDatabase` database exists.
3. Build the solution.
4. Start the application by pressing **F5** or selecting **Debug > Start Debugging**.
5. Confirm that the API starts successfully.
6. Confirm that the WPF client starts and displays its login window.
7. Log in with username `admin` and password `chase`.

If the WPF client cannot retrieve data, verify that:
- The API is running.
- The API URL configured for the WPF client matches the URL used by the API.
- SQL Server Express is running.
- The database connection string points to the correct SQL Server instance and database.

## Troubleshooting

### SQL Server connection failure

Verify the following:

- The SQL Server Express Windows service is running.
- The server name matches the server name used in SSMS.
- The `AddressDatabase` database exists.
- The Windows account running Visual Studio has permission to access the database.
- The database setup script completed without errors.

### The .NET 8 SDK is not found

Run the following command in PowerShell or Command Prompt:
`dotnet --list-sdks`

Verify that an `8.0.x` SDK is listed. If it is missing, install the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and restart Visual Studio.

### NuGet restore or build errors

In Visual Studio:

1. Select **Build > Restore NuGet Packages**.
2. Wait for package restoration to complete. It may help to restart Visual Studio.
3. Select **Build > Rebuild Solution**.
4. Review the first error reported in the Error List.

### The application starts but no address data is displayed

The database may be empty. Execute the repository's database seed script, and restart the application.