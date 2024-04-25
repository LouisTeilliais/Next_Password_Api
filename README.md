# Next Password API 

## How to launch project : 

### Lauch database 

To launch the database you have to get [Docker](https://docs.docker.com/desktop/install/windows-install/), because our DB is Dockerize. 

Once it's done, go to the root of NextPasswordApi & run : 

````
docker compose up
````

Wait few seconds and the database is up ! 

### Run migrations 

Now to get the last version of database you have to run migrations : 

Init migrations

````
dotnet ef migrations add <NameOfMigrations>
```` 

Update database 

````
dotnet ef database update
````

Now you get all the element to lauch the backen of Next Password, to launch the API execute this : 

````
dotnet build 
````

## How to work on the project

Create Entities in the entity folder to run migrations, ⚠️don't forget to update your DataContext with your Model ⚠️

Init migrations

````
dotnet ef migrations add <NameOfMigrations>
```` 

Once migrations are done, to create an API road follow this : 

1. Create an Iterface Repository
2. Create a Repository
3. Create an Iterface Service
4. Create a Service
5. Create a Controller

