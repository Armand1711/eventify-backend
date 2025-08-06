# 🎪 Eventify Backend

![GitHub](https://img.shields.io/badge/license-MIT-blue)
![Swagger](https://img.shields.io/badge/docs-Swagger-green)
![PostgreSQL](https://img.shields.io/badge/db-PostgreSQL-blue)
![Node.js](https://img.shields.io/badge/runtime-Node.js-green)
![Express](https://img.shields.io/badge/framework-Express-lightgrey)

Eventify Backend is a server-side application built for managing events, users, and related tasks. It uses JWT authentication and PostgreSQL as the database and provides a RESTful API with interactive Swagger documentation.

## Table of Contents
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Configuration](#configuration)
- [API Documentation](#api-documentation)
  - [Authentication](#authentication)
  - [Events](#events)
  - [Tasks](#tasks)
- [Running the Project](#running-the-project)
- [Database Schema](#database-schema)
- [Testing](#testing)
- [Contributors](#contributors)
- [License](#license)
- [Related Links](#related-links)

## Features

- User registration and login with JWT authentication
- Swagger UI for exploring API endpoints
- Uses PostgreSQL for data storage
- CRUD endpoints for managing events and tasks
- Endpoint testing with Moq
- Standard RESTful API structure
- Configuration via environment variables

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL (v15 or newer)
- Visual Studio Code (or another editor)

### Clone the Repository

```sh
git clone <your-repo-url>
cd eventify-backend/EventifyBackend
```

### Configure the Database

Edit `appsettings.json` and confirm the `DefaultConnection` string matches your PostgreSQL credentials. You can use the provided cloud database or set up your own.

### Set the JWT Secret

A random string (minimum 32 characters) should be set for `Jwt:Secret` in `appsettings.json`. The project comes with a default, but you can replace it.

### Apply Database Migrations

To set up the initial database tables:

```sh
dotnet tool install --global dotnet-ef
dotnet ef database update
```

### Running the Project

Start the server:

```sh
dotnet run
```

By default, the API will be available at `http://localhost:5256`.

### Testing with Swagger

Visit [http://localhost:5256/swagger](http://localhost:5256/swagger) in your browser. You can view and try out all API endpoints from here.

## Example: Using the API

1. **Sign up a user**  
   Send a POST request to `/api/users/signup` with a JSON body like:
   ```json
   {
     "email": "testuser@example.com",
     "password": "TestPassword123!"
   }
   ```

2. **Log in**  
   POST to `/api/users/login` to receive a JWT token.

3. **Authorize in Swagger**  
   Click "Authorize" in Swagger and paste your token: `Bearer <your-token>`

4. **Try other endpoints**  
   After authenticating, you can create events, tasks, budgets, and archives.

## Troubleshooting

- **Database errors:** Double-check your connection string and ensure PostgreSQL is running.
- **JWT errors:** Make sure the secret is secure and at least 32 characters.
- **Port issues:** If `http://localhost:5256` isn't working, check the terminal output for the correct port.

## Useful Commands

- Update your database:
  ```sh
  dotnet ef database update
  ```
- Run the server:
  ```sh
  dotnet run
  ```

## Need Help?

Reach out to your team lead or post your error message in the team chat for assistance.

---
