Eventify Backend (C#)
Overview
eventify-backend is a C# ASP.NET Core API for managing events, tasks, budgets, and archives. It uses Entity Framework Core for PostgreSQL database interactions, JWT for authentication, and Swashbuckle for Swagger UI. The application is containerized using Docker for easy development and deployment.
This backend supports:

User management (signup, login, profile retrieval)
Event CRUD operations
Task and budget management per event
Event archiving

Prerequisites

Git: To clone the repository.
Docker: To run the application in a container.
Docker Compose: For managing the Docker setup.
.NET SDK: Version 8.0 or later (if running locally without Docker).
psql (PostgreSQL client): Optional, for direct database access.

Setup Instructions
1. Clone the Repository
git clone <repository-url>
cd EventifyBackend

2. Configure Settings
Ensure appsettings.json contains the correct database connection string:
"ConnectionStrings": {
  "DefaultConnection": "Host=eventify-db-armandn03-cdd0.g.aivencloud.com;Port=21571;Database=defaultdb;Username=avnadmin;Password=AVNS_8NsN2OzScejY-EOc3CL;SslMode=Require;"
},
"Jwt": {
  "Secret": "your_jwt_secret_here"
}


⚠️ Security Warning: The database credentials above are sensitive. In production, use environment variables or a secrets manager. Do not commit sensitive data to a public repository.

3. Build and Run with Docker
Build the Docker Image
docker-compose up --build


The application will be available at http://localhost:3000.

Expected Output
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (Xms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE DATABASE IF NOT EXISTS "defaultdb";
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://[::]:3000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.

4. Access the API

Swagger UI: Open http://localhost:3000/api-docs to view and test the API endpoints.
Test Endpoint: Visit http://localhost:3000/ to confirm the server is running (should return Eventify Backend is running!).

5. Test Endpoints with cURL
Sign Up a User
curl -X 'POST' \
  'http://localhost:3000/api/users/signup' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "test@example.com",
  "password": "password123"
}'

Log In
curl -X 'POST' \
  'http://localhost:3000/api/users/login' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "test@example.com",
  "password": "password123"
}'

Create an Event
curl -X 'POST' \
  'http://localhost:3000/api/events' \
  -H 'accept: application/json' \
  -H 'Authorization: Bearer <your-jwt-token>' \
  -H 'Content-Type: application/json' \
  -d '{
  "title": "My Event",
  "description": "A test event",
  "date": "2025-06-10T10:00:00Z"
}'

6. Verify Database (Optional)
psql -h eventify-db-armandn03-cdd0.g.aivencloud.com -p 21571 -U avnadmin -d defaultdb --set=sslmode=require


Password: AVNS_8NsN2OzScejY-EOc3CL
Query: SELECT * FROM "Users";

7. Stop the Application
docker-compose down

Project Structure

Controllers/: API endpoint definitions.
Models/: Entity Framework Core models and database context.
Program.cs: Main application entry point.
appsettings.json: Configuration settings.
Dockerfile and docker-compose.yml: Docker setup.

Additional Commands
Rebuild Without Cache
docker-compose up --build --no-cache

View Docker Logs
docker-compose logs

Run Locally Without Docker

Restore dependencies:dotnet restore


Run the application:dotnet run



Troubleshooting

Database Connection Issues: Verify your IP is allowlisted in Aiven.
JWT Errors: Ensure Jwt:Secret is set in appsettings.json.
404 Errors: Check that all controllers are defined correctly.

For further assistance, check the Docker logs or consult the project maintainers.
