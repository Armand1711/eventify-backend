Eventify Backend
Overview
eventify-backend is a Node.js/Express API for managing events, tasks, budgets, and archives. It uses Sequelize for PostgreSQL database interactions, JWT for authentication, and Swagger UI for API documentation. The application is containerized using Docker for easy development and deployment.
This backend supports:

User management (signup, login, profile retrieval)
Event CRUD operations
Task and budget management per event
Event archiving

Prerequisites
Before running this project, ensure you have the following installed:

Git: To clone the repository.
Docker: To run the application in a container.
Docker Compose: For managing the Docker setup.
Node.js: Optional, only if you need to run the app without Docker (v20 recommended).
psql (PostgreSQL client): Optional, for direct database access.

Setup Instructions
1. Clone the Repository
Clone this repository to your local machine:
git clone <repository-url>
cd eventify-backend

2. Configure Environment Variables
Create a .env file in the project root with the following content:
PORT=3000
JWT_SECRET=your_jwt_secret_here
DB_HOST=eventify-db-armandn03-cdd0.g.aivencloud.com
DB_PORT=21571
DB_USER=avnadmin
DB_PASSWORD=AVNS_8NsN2OzScejY-EOc3CL
DB_NAME=defaultdb


⚠️ Security Warning: The database credentials above are sensitive. In a production environment or public repository, do not commit the .env file. Use environment variables or a secrets management tool instead. For this setup, ensure .env is listed in .gitignore.

3. Build and Run with Docker
Ensure Docker and Docker Compose are running on your machine, then execute the following commands:
Build the Docker Image
docker-compose up --build


This command builds the Docker image and starts the container.
The application will be available at http://localhost:3000.

Expected Output
You should see logs indicating:
app-1  | Database connection has been established successfully.
app-1  | Models synchronized with database.
app-1  | Server running on port 3000

4. Access the API

Swagger UI: Open http://localhost:3000/api-docs in your browser to view and test the API endpoints.
Test Endpoint: Visit http://localhost:3000/ to confirm the server is running (should return Eventify Backend is running!).

5. Test Endpoints with cURL
Here are some example cURL commands to test the API:
Sign Up a User
curl -X 'POST' \
  'http://localhost:3000/api/users/signup' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "test@example.com",
  "password": "password123"
}'


Expected response: 201 Created with a JWT token and user details.

Log In
curl -X 'POST' \
  'http://localhost:3000/api/users/login' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "email": "test@example.com",
  "password": "password123"
}'


Copy the token from the response for authenticated requests.

Create an Event (Authenticated)
Replace <your-jwt-token> with the token from the login response:
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
To confirm data is being saved, connect to the Aiven PostgreSQL database:
psql -h eventify-db-armandn03-cdd0.g.aivencloud.com -p 21571 -U avnadmin -d defaultdb --set=sslmode=require


Password: AVNS_8NsN2OzScejY-EOc3CL
Example query: SELECT * FROM "Users";


⚠️ Security Note: Ensure your IP is allowlisted in the Aiven firewall for database access.

7. Stop the Application
To stop the Docker container:
docker-compose down

Project Structure

config/database.js: Sequelize database configuration.
middleware/auth.js: JWT authentication middleware.
routes/: Route handlers for users, events, tasks, budgets, and archives.
swagger.yaml: API documentation for Swagger UI.
index.js: Main application entry point.
Dockerfile: Docker image configuration.
docker-compose.yml: Docker Compose setup.

Additional Commands
Rebuild Without Cache
If you make changes to the Dockerfile or dependencies:
docker-compose up --build --no-cache

View Docker Logs
To debug issues:
docker-compose logs

Run Locally Without Docker (Optional)
If you prefer to run the app directly:

Install dependencies:npm install


Start the server:npm start



Notes

Development Mode: To enable live reloading, add nodemon as a dev dependency and update the Dockerfile to use npm run dev.
Production: Secure the .env file, use a secrets manager, and deploy with a reverse proxy (e.g., Nginx).

Troubleshooting

404 Errors: Ensure all route files are present and correctly mounted in index.js.
Database Connection Issues: Verify your IP is allowlisted in Aiven, and check the credentials in .env.
JWT Errors: Ensure JWT_SECRET is set in .env.

For further assistance, check the Docker logs or consult the project maintainers.
