const express = require('express');
const router = express.Router({ mergeParams: true }); // Merge eventId from parent route
const authMiddleware = require('../middleware/auth');
const { Sequelize, DataTypes } = require('sequelize');
const sequelize = require('../config/database');

const Task = sequelize.define('Task', {
  title: { type: DataTypes.STRING, allowNull: false },
  description: { type: DataTypes.STRING },
  dueDate: { type: DataTypes.DATE },
  eventId: { type: DataTypes.INTEGER, allowNull: false },
});

router.post('/', authMiddleware, async (req, res) => {
  const { title, description, dueDate } = req.body;
  try {
    const task = await Task.create({ title, description, dueDate, eventId: req.params.eventId, userId: req.user.userId });
    res.status(201).json(task);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.get('/', authMiddleware, async (req, res) => {
  try {
    const tasks = await Task.findAll({ where: { eventId: req.params.eventId, userId: req.user.userId } });
    res.status(200).json(tasks);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.get('/:taskId', authMiddleware, async (req, res) => {
  try {
    const task = await Task.findOne({ where: { id: req.params.taskId, eventId: req.params.eventId, userId: req.user.userId } });
    if (!task) return res.status(404).json({ error: 'Task not found' });
    res.status(200).json(task);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.put('/:taskId', authMiddleware, async (req, res) => {
  const { title, description, dueDate } = req.body;
  try {
    const task = await Task.findOne({ where: { id: req.params.taskId, eventId: req.params.eventId, userId: req.user.userId } });
    if (!task) return res.status(404).json({ error: 'Task not found' });
    await task.update({ title, description, dueDate });
    res.status(200).json(task);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.delete('/:taskId', authMiddleware, async (req, res) => {
  try {
    const task = await Task.findOne({ where: { id: req.params.taskId, eventId: req.params.eventId, userId: req.user.userId } });
    if (!task) return res.status(404).json({ error: 'Task not found' });
    await task.destroy();
    res.status(204).send();
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

module.exports = router;