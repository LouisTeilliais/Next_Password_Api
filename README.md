# Next Password API 

To launch project : 

````
dotnet build 
````

Create Entities in the entity folder to run migrations, ⚠️don't forget to update your DataContext with your Model ⚠️

Init migrations

````
dotnet ef migrations add <NameOfMigrations>
```` 

Update database 

````
dotnet ef database update
````
Once migrations are done, to create an API Route follow this : 

1. Create an Iterface Repository
2. Create a Repository
3. Create an Iterface Service
4. Create a Service
5. Create a Controller
