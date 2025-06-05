'use strict';
module.exports = (sequelize, DataTypes) => {
  const Event = sequelize.define('Event', {
    name: {
      type: DataTypes.STRING,
      allowNull: false
    },
    client: DataTypes.STRING,
    date: DataTypes.DATE,
    status: DataTypes.STRING,
    progress: DataTypes.INTEGER,
    completedTasks: DataTypes.INTEGER,
    totalTasks: DataTypes.INTEGER,
    colorClass: DataTypes.STRING
  }, {});
  Event.associate = function(models) {
    Event.hasMany(models.Task, { foreignKey: 'eventId' });
    Event.hasMany(models.Budget, { foreignKey: 'eventId' });
  };
  return Event;
};