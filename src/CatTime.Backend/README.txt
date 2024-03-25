Setup the CatTime database:
1. Install docker
2. Run "docker run -d --name cattime-postgres -p 5432:5432 -e POSTGRES_PASSWORD=123456 postgres"
3. Connect to the database with:
    Server: localhost
    Port: 5432
    Database: postgres
    User: postgres
    Password: 123456
4. Create a database called "cattime" using "CREATE DATABASE cattime;"
5. Create the tables using the Entity Framework Core command-line tool

INFO for Step 3:    (Connection to the database "Azure Data Studio")
1. Make sure the docker container is running
2. You can use Azure Data Studio to connect to the database
3. First you have to install the PostgreSQL extension
4. Then you can connect to the database using the information above
5. Create the cattime database

INFO for Step 5:    (Create the tables to the database, e.g from the Developer Powershell)
1. If not already installed: Run "dotnet tool install --global dotnet-ef" to install the Entity Framework Core command-line tool
2. Navigate in the folder where the CatTime.Backend.csproj file is located (not the folder with the solution file)
3. Now run "dotnet tool update --global dotnet-ef"

Congrats! You are now ready to run the backend!