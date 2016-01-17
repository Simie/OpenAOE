# OpenAOE
### An open-source implementation of Age of Empires 2 in C# and .NET.

## What

OpenAOE is an implementation of a deterministic entity-component-system game engine to replicate Age of Empires 2 gameplay. It is not a reimplementation of the Genie engine used by AOE2, rather it is an attempt to imitate the existing AOE2 gameplay in a modern RTS engine.

## Why

I love Age of Empires 2, but I get frustrated with the networking lag and the restrictions of the original game. AOE2:HD didn't fix the particular problems I have, namely the player limit and network lag. 
This project is an attempt to build a modern RTS game engine to replicate the feel and gameplay of AOE2.

It is also a personal attempt to learn more about programming best-practices by fully embracing Test Driven Development, modern .NET build environments, Continuous Integration, etc.

## How

### Libraries

Technology                | Component
--------------------------|----------
**C#/.NET**               | Engine, Gameplay
**Ninject**               | Dependency Injection
**Mono**                  | Linux/Mac OS support (TODO)
**CAKE**                  | Build system (TODO)

### Inspirations

Source                    | Inspired
--------------------------|----------
[Forge][Forge]            | Entity/Component system design.
[OpenAge][OpenAge]        | Documentation about existing AOE2 formats and algorithms.

[Forge](https://github.com/jacobdufault/forge)
[OpenAge](https://github.com/SFTtech/openage/)
[DepInj](https://en.wikipedia.org/wiki/Dependency_injection)
[EntityComponent](https://en.wikipedia.org/wiki/Entity_component_system)