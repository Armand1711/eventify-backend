const express = require('express');
const swaggerUi = require('swagger-ui-express');
const YAML = require('yamljs');
const sequelize = require('./config/database');
const userRoutes = require('./routes/user');
const eventRoutes = require('./routes/event');
const taskRoutes = require('./routes/task');
const budgetRoutes = require('./routes/budget');
const archiveRoutes = require('./routes/archive');
const authMiddleware = require('./middleware/auth');

const app = express();
const port = process.env.PORT || 3000;

// Middleware
app.use(express.json());

// Routes
app.use('/api/users', userRoutes);
app.use('/api/events', eventRoutes); // Base events route
app.use('/api/events/:eventId/tasks', taskRoutes); // Nested tasks route
app.use('/api/events/:eventId/budgets', budgetRoutes); // Nested budgets route
app.use('/api/archives', archiveRoutes);

// Swagger UI setup
const swaggerDocument = YAML.load('./swagger.yaml');
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerDocument));

// Test route
app.get('/', (req, res) => {
  res.send('Eventify Backend is running!');
});

// Start server and connect to database
const startServer = async () => {
  try {
    // Test database connection
    await sequelize.authenticate();
    console.log('Database connection has been established successfully.');

    // Synchronize models (optional, for development)
    await sequelize.sync({ force: false });
    console.log('Models synchronized with database.');

    // Start server
    app.listen(port, () => {
      console.log(`Server running on port ${port}`);
    });
  } catch (error) {
    console.error('Unable to connect to the database:', error);
    process.exit(1);
  }
};

startServer();