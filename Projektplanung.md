# Grober Entwicklungsplan

## Phase 1 – Grundsystem
- Spielerbewegung programmieren
- Kollisionen
- einfache Weltgenerierung (Godot Feature)
- Testwelt

## Phase 2 – Erste Welt
- erstes Biom (Dschungel)
- einfache Hindernisse
- Rätsel
- Upgrades

## Phase 3 – Weitere Gebiete
- Wiesen
- Wald
- Wüste

## Phase 4 – Feinschliff
- Animationen
- Soundeffekte
- Sammelobjekte

## Optionale Features
- Weitere Gegenden mit Gimmicks
- Weitere Upgrades
- Währung

<!-- ```mermaid
---
title: Animal example
---
classDiagram
    note "From Duck till Zebra"
    Animal <|-- Duck
    note for Duck "can fly<br>can swim<br>can dive<br>can help in debugging"
    Animal <|-- Fish
    Animal <|-- Zebra
    Animal : +int age
    Animal : +String gender
    Animal: +isMammal()
    Animal: +mate()
    class Duck{
        +String beakColor
        +swim()
        +quack()
    }
    class Fish{
        -int sizeInFeet
        -canEat()
    }
    class Zebra{
        +bool is_wild
        +run()
    }
``` -->