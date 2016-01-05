# OpenAOE
### An open-source implementation of Age of Empires 2 in C# and .NET.

## Overview

The intention of this project is to provide a similar experience to Age of Empires 2 based on the original assets but with improved networking and multiplayer features.

### Goals

#### General
- Match existing AOE2 gameplay as far as possible. It doesn't have to be exact.
- Larger maps.
- Support for more than 8 players.
- Smoother and less laggy multiplayer.

#### Technical
- Implement an Entity/Component system with support for deterministic simulation and multithreading.
- Systems act on the data model every tick to implement gameplay.
- UI implemented in HTML5/CSS with Ember.js linking with data model via JSON api.
- Unit tested engine module.

### Original Assets

The intention is to include a converter to transform the existing graphics assets into a format OpenAOE can display. Game data is likely to be converted via script, cleaned up by hand, and included in this repo (ala OpenTTD).