# OpenAOE
### An open-source implementation of Age of Empires 2 in C# and .NET.

## What

OpenAOE is an implementation of a deterministic entity-component-system game engine to replicate Age of Empires 2 gameplay. It is not a reimplementation of the Genie engine used by AOE2.

## Why

I love Age of Empires 2, but I get frustrated with the networking lag and the restrictions of the original game. AOE2:HD didn't fix the particular problems I have, namely the player limit and network lag. 
This project is an attempt to build a modern RTS game engine to replicate the feel and gameplay of AOE2.

## Libraries

Technology                | Component
--------------------------|----------
**C#/.NET**               | Engine, Gameplay
**Ninject**               | Dependency Injection
**Mono**                  | Linux/Mac OS support
**CAKE**                  | Build system

## Inspirations

Source                    | Inspired
--------------------------|----------
[Forge][Forge]            | Entity/Component design.
[OpenAge][OpenAge]        | Documentation about existing AOE2 formats.

## FAQ

### Why not join the [OpenAge](OpenAge) project?
I love C# and I develop on primarily on Windows. The OpenAge project focuses primarily on POSIX platforms. In addition, OpenAge is implementing a classic game engine architecture without taking advantage 
of modern game design patterns such as [Entity-Component-System](EntityComponent), [Dependency Injection](DepInj) and Unit Testing.
As these are all areas in which I am interested in learning more, so I decided to start this project as a learning exercise.
I am incredibly grateful for their detailed documentation of AOE2 data and graphics formats and without them this project would not be possible.

[Forge](https://github.com/jacobdufault/forge)
[OpenAge](https://github.com/SFTtech/openage/)
[DepInj](https://en.wikipedia.org/wiki/Dependency_injection)
[EntityComponent](https://en.wikipedia.org/wiki/Entity_component_system)