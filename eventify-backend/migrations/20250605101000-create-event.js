'use strict';
module.exports = {
  up: async (queryInterface, Sequelize) => {
    await queryInterface.createTable('events', {
      id: {
        allowNull: false,
        autoIncrement: true,
        primaryKey: true,
        type: Sequelize.INTEGER,
      },
      name: {
        type: Sequelize.STRING,
        allowNull: false,
      },
      client: {
        type: Sequelize.STRING,
        allowNull: false,
      },
      date: {
        type: Sequelize.DATE,
        allowNull: false,
      },
      status: {
        type: Sequelize.STRING,
        defaultValue: 'In Progress',
      },
      progress: {
        type: Sequelize.INTEGER,
        defaultValue: 0,
      },
      completedTasks: {
        type: Sequelize.INTEGER,
        defaultValue: 0,
      },
      totalTasks: {
        type: Sequelize.INTEGER,
        defaultValue: 0,
      },
      colorClass: {
        type: Sequelize.STRING,
        defaultValue: 'yellow',
      },
      createdAt: {
        allowNull: false,
        type: Sequelize.DATE,
      },
      updatedAt: {
        allowNull: false,
        type: Sequelize.DATE,
      },
    });
  },
  down: async (queryInterface, Sequelize) => {
    await queryInterface.dropTable('events');
  },
};