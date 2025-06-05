'use strict';
module.exports = (sequelize, DataTypes) => {
  const Archive = sequelize.define('Archive', {
    eventName: DataTypes.STRING,
    status: DataTypes.STRING,
    dateCompleted: DataTypes.DATE,
    completedTasks: DataTypes.STRING,
    totalBudget: DataTypes.FLOAT,
    notes: DataTypes.TEXT
  }, {});
  Archive.associate = function(models) {
    // No associations needed for Archive
  };
  return Archive;
};