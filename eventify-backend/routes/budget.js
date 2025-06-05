const express = require('express');
const router = express.Router({ mergeParams: true }); // Merge eventId from parent route
const authMiddleware = require('../middleware/auth');
const { Sequelize, DataTypes } = require('sequelize');
const sequelize = require('../config/database');

const Budget = sequelize.define('Budget', {
  category: { type: DataTypes.STRING, allowNull: false },
  amount: { type: DataTypes.FLOAT, allowNull: false },
  eventId: { type: DataTypes.INTEGER, allowNull: false },
});

router.post('/', authMiddleware, async (req, res) => {
  const { category, amount } = req.body;
  try {
    const budget = await Budget.create({ category, amount, eventId: req.params.eventId, userId: req.user.userId });
    res.status(201).json(budget);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.get('/', authMiddleware, async (req, res) => {
  try {
    const budgets = await Budget.findAll({ where: { eventId: req.params.eventId, userId: req.user.userId } });
    res.status(200).json(budgets);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.get('/:budgetId', authMiddleware, async (req, res) => {
  try {
    const budget = await Budget.findOne({ where: { id: req.params.budgetId, eventId: req.params.eventId, userId: req.user.userId } });
    if (!budget) return res.status(404).json({ error: 'Budget not found' });
    res.status(200).json(budget);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.put('/:budgetId', authMiddleware, async (req, res) => {
  const { category, amount } = req.body;
  try {
    const budget = await Budget.findOne({ where: { id: req.params.budgetId, eventId: req.params.eventId, userId: req.user.userId } });
    if (!budget) return res.status(404).json({ error: 'Budget not found' });
    await budget.update({ category, amount });
    res.status(200).json(budget);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.delete('/:budgetId', authMiddleware, async (req, res) => {
  try {
    const budget = await Budget.findOne({ where: { id: req.params.budgetId, eventId: req.params.eventId, userId: req.user.userId } });
    if (!budget) return res.status(404).json({ error: 'Budget not found' });
    await budget.destroy();
    res.status(204).send();
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

module.exports = router;