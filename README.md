# Eventify Backend

This is the backend for the Eventify app, built with ASP.NET Core and PostgreSQL.  
It provides a REST API for managing users, events, tasks, budgets, and archives.

---

## 🚀 Getting Started

### 1. **Prerequisites**

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) (or use the provided cloud connection string)
- [Visual Studio Code](https://code.visualstudio.com/) (recommended, but not required)

---

### 2. **Clone the Repository**

```sh
git clone <your-repo-url>
cd eventify-backend/EventifyBackend
```

---

### 3. **Configure the Database**

- Open `appsettings.json`.
- Make sure the `DefaultConnection` string is correct for your PostgreSQL database.
  - You can use the provided cloud database, or set up your own and update the connection string.

---

### 4. **Set the JWT Secret**

- The `Jwt:Secret` in `appsettings.json` should be a long, random string (at least 32 characters).
- This is already set for you, but you can change it if you want.

---

### 5. **Apply Database Migrations**

This will create all the necessary tables in your database.

```sh
dotnet tool install --global dotnet-ef
dotnet ef database update
```

---

### 6. **Run the Project**

```sh
dotnet run
```

- The API will start, usually at `http://localhost:5256`.

---

### 7. **Test with Swagger**

- Open your browser and go to: [http://localhost:5256/swagger](http://localhost:5256/swagger)
- You will see a web interface where you can test all the API endpoints.

---

## 🧪 Example: Test the API

1. **Sign up a user**
   - Use the `POST /api/users/signup` endpoint.
   - Example body:
     ```json
     {
       "email": "testuser@example.com",
       "password": "TestPassword123!"
     }
     ```

2. **Log in**
   - Use the `POST /api/users/login` endpoint.
   - Copy the returned `token`.

3. **Authorize**
   - Click the "Authorize" button in Swagger (top right).
   - Paste your token as: `Bearer <your-token>`

4. **Try other endpoints**
   - Now you can create events, tasks, budgets, and archives.

---

## 🛠 Troubleshooting

- **Database errors:** Make sure your connection string is correct and the database is running.
- **JWT errors:** Make sure your JWT secret is at least 32 characters.
- **Port issues:** If `http://localhost:5256` doesn't work, check your terminal for the correct port.

---

## 📚 Useful Commands

- Update database:  
  ```sh
  dotnet ef database update
  ```
- Run the project:  
  ```sh
  dotnet run
  ```

---

## 🙋 Need Help?

If you get stuck, ask your team lead or post your error message in the team chat!

---