# delta-signal-core

## Gameplay Overview

This is a classic memory card matching game where players:

- Flip two cards at a time
- Try to find matching pairs
- Complete the board with the least turns possible
- Earn score based on performance

The game supports multiple dynamic layouts and scales automatically to fit the display area.

---

## Features Implemented

### ✅ Core Gameplay
- Smooth card flip animations
- Continuous card interaction (no hard locking of input)
- Match and mismatch detection
- Dynamic grid generation

### ✅ Multiple Layout Support
Supports various board sizes:
- 2x2
- 2x3
- 3x4
- 4x4
- 4x5
- 5x6

Cards automatically scale to fit the container.

## Save / Load System
- Persists player progress between game restarts
- Restores:
  - Matches
  - Turns
  - Score
  - Selected layout

## Scoring System
- Score increases when matching pairs
- Turn counter tracks player attempts
- Game Over screen displays final results

## Sound Effects
Integrated basic sound effects for:
- Card flip
- Match
- Mismatch
- Game over

## Clean Git Workflow
- Project initialized from empty Unity project
- Frequent commits with meaningful messages
- Structured development process
