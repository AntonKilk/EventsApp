# EventsApp

### App structure

Events app is an MVC app that consists of three projects:

- EventWebApp contains Views and Controllers:
  - Run this project as main
- EventAppLibrary contains Model related:
  - Models: Company, Event, Person
  - DataAccess EventContext
  - Migrations with initial seed dummy data
- EventWebApp.Tests
  - Unit tests for controllers

App uses Microsoft SQL Server Database.

## Running instructions

**I. Clone the repo**

```
git clone https://github.com/AntonKilk/EventsApp.git
cd EventsApp
```

**II. Migrate the database (in the initial only)**

In Visual Studio Package Manager Console:

```
Update-Database
```

In CLI:

```
dotnet tool install --global dotnet-ef
dotnet ef database update
```

You can view data in the MSSQL Management Studio.

Ensure your appsettings.json file in the EventWebApp project contains the correct SQL Server connection string.


**III. Run project**

In Visual Studio:

Run IIS Express or any other server.

In CLI:

```
dotnet run --project .\EventWebApp.csproj
```

### Accessing the App

The app is served at `https://localhost:44354/`

### ToDo

~~Create DB instance and EF~~

~~Create UML Diagram~~

~~Create controllers and services~~

~~Create basic UI:~~

- ~~Avaleht~~
- ~~Ürituse lisamise vaade~~
- ~~Üritusest osavõtvate isikute vaade~~
- ~~Osavõtja lisamise vaade~~
- ~~Osavõtja detailandmete vaatamise/muutmise vaade~~

Add UI/ CSS styles

~~controllers logic tests & error handling~~

Add dictionary Json for translation

~~Add UI validation for ID~~

~~Add documentation~~
