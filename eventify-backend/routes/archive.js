const express = require('express');
const router = express.Router();
const authMiddleware = require('../middleware/auth');
const { Sequelize, DataTypes } = require('sequelize');
const sequelize = require('../config/database');

const Archive = sequelize.define('Archive', {
  eventId: { type: DataTypes.INTEGER, allowNull: false },
  title: { type: DataTypes.STRING, allowNull: false },
  description: { type: DataTypes.STRING },
  date: { type: DataTypes.DATE, allowNull: false },
  userId: { type: DataTypes.INTEGER, allowNull: false },
});

router.post('/events/:eventId/archive', authMiddleware, async (req, res) => {
  try {
    const event = await sequelize.models.Event.findOne({ where: { id: req.params.eventId, userId: req.user.userId } });
    if (!event) return res.status(404).json({ error: 'Event not found' });
    const archive = await Archive.create({
      eventId: event.id,
      title: event.title,
      description: event.description,
      date: event.date,
      userId: req.user.userId,
    });
    await event.destroy();
    res.status(201).json(archive);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.get('/', authMiddleware, async (req, res) => {
  try {
    const archives = await Archive.findAll({ where: { userId: req.user.userId } });
    res.status(200).json(archives);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.get('/:archiveId', authMiddleware, async (req, res) => {
  try {
    const archive = await Archive.findOne({ where: { id: req.params.archiveId, userId: req.user.userId } });
    if (!archive) return res.status(404).json({ error: 'Archive not found' });
    res.status(200).json(archive);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

router.delete('/:archiveId', authMiddleware, async (req, res) => {
  try {
    const archive = await Archive.findOne({ where: { id: req.params.archiveId, userId: req.user.userId } });
    if (!archive) return res.status(404).json({ error: 'Archive not found' });
    await archive.destroy();
    res.status(204).send();
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

module.exports = router;