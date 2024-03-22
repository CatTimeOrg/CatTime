Start a postgres database:
1. Install docker
2. Run "docker run -d --name cattime-postgres -p 5432:5432 -e POSTGRES_PASSWORD=123456 postgres"
3. Connect to the database with:
    Server: localhost
    Port: 5432
    Database: postgres
    User: postgres
    Password: 123456
4. Create a database called "cattime" using "CREATE DATABASE cattime;"

Install "dotnet-ef" tool:
1. Run "dotnet tool install --global dotnet-ef" to install the Entity Framework Core command-line tool
2. If you have it installed already and want to update it, run "dotnet tool update --global dotnet-ef"