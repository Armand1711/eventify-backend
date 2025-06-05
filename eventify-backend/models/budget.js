'use strict';
module.exports = (sequelize, DataTypes) => {
  const Budget = sequelize.define('Budget', {
    eventId: {
      type: DataTypes.INTEGER,
      allowNull: false,
      references: { model: 'Events', key: 'id' }
    },
    description: DataTypes.STRING,
    category: DataTypes.STRING,
    amount: DataTypes.FLOAT,
    status: DataTypes.STRING
  }, {});
  Budget.associate = function(models) {
    Budget.belongsTo(models.Event, { foreignKey: 'eventId' });
  };
  return Budget;
};