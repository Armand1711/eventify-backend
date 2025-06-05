'use strict';
module.exports = (sequelize, DataTypes) => {
  const Task = sequelize.define('Task', {
    eventId: {
      type: DataTypes.INTEGER,
      allowNull: false,
      references: { model: 'Events', key: 'id' }
    },
    title: DataTypes.STRING,
    priority: DataTypes.STRING,
    priorityClass: DataTypes.STRING,
    assignedTo: DataTypes.INTEGER,
    budget: DataTypes.FLOAT,
    completed: {
      type: DataTypes.BOOLEAN,
      defaultValue: false
    }
  }, {});
  Task.associate = function(models) {
    Task.belongsTo(models.Event, { foreignKey: 'eventId' });
  };
  return Task;
};