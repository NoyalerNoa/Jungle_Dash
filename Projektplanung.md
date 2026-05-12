# Grober Entwicklungsplan

## 1. GUI - Skizzen

## Zeitplan
| Datum   | Aufgabe              | Person        | Zeit |
|--------|------------------------|--------------|------|
| 20.05  | Idee & Planung         | Michael & Noa | 2h   |
| 21.05  | GUI Skizzen            | Noa          | 2h   |
| 22.05  | Klassendiagramm        | Michael      | 3h   |
| 23.05  | Grundgerüst            | Michael      | 4h   |
| 24.05  | Movement System        | Michael      | 5h   |
| 25.05  | Dash & Sprünge         | Michael      | 4h   |
| 26.05  | Gegner                 | Noa          | 4h   |
| 27.05  | Welt / Biome           | Noa          | 5h   |
| 28.05  | Runen / Upgrades       | Beide        | 3h   |
| 29.05  | UI / HUD / Pausemenü   | Noa          | 4h   |
| 30.05  | Testing                | Beide        | 5h   |
| 31.05  | Präsentation           | Beide        | 2h   |

## Klassendiagramm
```mermaid
classDiagram

class Game {
  - currentLevel
  - player
  - enemies
  + startGame()
  + update()
  + render()
  + pauseGame()
}

class Player {
  - health
  - speed
  - positionX
  - positionY
  - runeCount
  - abilities
  + move()
  + jump()
  + dash()
  + crouch()
  + collectRune()
}

class Ability {
  - name
  - unlocked
  + unlock()
  + activate()
}

class Enemy {
  - damage
  - speed
  - position
  + move()
  + attack()
}

class Snake {
  + poison()
}

class Deer {
  + charge()
}

class Rune {
  - position
  - collected
  + collect()
}

class World {
  - biomes
  - mapSize
  + loadBiome()
  + changeArea()
}

Game --> Player
Game --> Enemy
Game --> World
Player --> Ability
Player --> Rune
Enemy <|-- Snake
Enemy <|-- Deer