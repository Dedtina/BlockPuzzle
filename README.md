# BlockPuzzle

**BlockPuzzle** is a 9x9 grid-based drag-and-drop puzzle game built with **Unity**. Players drag shapes onto the grid to complete rows or columns, clearing space for subsequent shapes and striving for the highest possible score.

---

## Gameplay Overview

* **9x9 Grid System:** Features randomly generated shapes.
* **Drag-and-Drop:** Players manually position shapes into valid slots on the board.
* **Line Clearing:** Completing any row or column clears the line and awards points.
* **Combo Effects:** Clearing multiple lines in a single move triggers congratulatory visual effects.
* **Progression & Goals:** Includes a color-based bonus system and local high-score saving to enhance replayability.
* **Shape Swap:** A limited-use feature allowing players to refresh the current set of three shapes.

## Core Features

* **Input System:** Smooth drag-and-drop gameplay using Unity UI and `Physics2D`.
* **Shape Management:** Utilizes `ScriptableObjects` for easy creation, editing, and reuse of shape patterns.
* **Data Persistence:** Local high-score saving implemented via `BinaryDataStream`.
* **UI & VFX:** Game Over popups, best score tracking, and bonus/congratulation animations.
* **Scene Architecture:** Separated Menu and Gameplay scenes for better scalability.

## How to Play

1.  **Select:** Choose a shape from the available pool.
2.  **Drag:** Move the shape over the 9x9 grid.
3.  **Place:** Release the shape on a valid spot to lock it onto the board.
4.  **Clear:** Fill rows or columns to clear lines and gain points.
5.  **Swap:** Use the swap button when stuck (limited uses).
6.  **Game Over:** The match ends when no remaining shapes can fit onto the grid.

## Demo
https://youtu.be/pCOdJvWsuCE
