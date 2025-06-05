const express = require('express');
const router = express.Router();
const authMiddleware = require('../middleware/auth');
const { Sequelize, DataTypes } = require('sequelize');
const sequelize = require('../config/database');

const Event = sequelize.define('Event', {
  title: { type: DataTypes.STRING, allowNull: false },
  description: { type: DataTypes.STRING },
  date: { type: DataTypes.DATE, allowNull: false },
  userId: { type: DataTypes.INTEGER, allowNull: false },
});

router.post('/', authMiddleware, async (req, res) => {
  const { title, description, date } = req.body;
  try {
    const event = await Event.create({ title, description, date, userId: req.user.userId });
    res.status(201).json(event);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.get('/', authMiddleware, async (req, res) => {
  try {
    const events = await Event.findAll({ where: { userId: req.user.userId } });
    res.status(200).json(events);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.get('/:id', authMiddleware, async (req, res) => {
  try {
    const event = await Event.findOne({ where: { id: req.params.id, userId: req.user.userId } });
    if (!event) return res.status(404).json({ error: 'Event not found' });
    res.status(200).json(event);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.put('/:id', authMiddleware, async (req, res) => {
  const { title, description, date } = req.body;
  try {
    const event = await Event.findOne({ where: { id: req.params.id, userId: req.user.userId } });
    if (!event) return res.status(404).json({ error: 'Event not found' });
    await event.update({ title, description, date });
    res.status(200).json(event);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.delete('/:id', authMiddleware, async (req, res) => {
  try {
    const event = await Event.findOne({ where: { id: req.params.id, userId: req.user.userId } });
    if (!event) return res.status(404).json({ error: 'Event not found' });
    await event.destroy();
    res.status(204).send();
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

module.exports = router;