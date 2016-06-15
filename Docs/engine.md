# Engine Architecture

## Entity System
TODO

## Systems
TODO

## Update Loop

The main update loop occurs in 3 stages, has one set of inputs and one set of outputs.

Input: Commands
Output: Events

- Process Commands
- System Update Bursts
- Process Events

### Stages

#### Input
When the engine tick is begun via IEngine.Tick(EngineTickInput input) a list of commands to execute this tick is passed in the EngineTickInput structure.

#### Process Commands
Commands are executed sequentually and in a single-threaded fashion based on the systems update priority.
The same command can be consumed multiple times.

#### System Update Bursts
Systems are grouped into "Update Bursts". Systems that support multi-threaded executed may be run in parallel with other multi-threaded systems. The next burst will begin
once the previous burst has been completed, allowing for single-threaded systems and multi-threaded systems that depend on multiple other systems to be executed after their
dependencies are complete.

For example,
Update Burst 1: AdvanceTimeStepSystem
Update Burst 2: MovementSystem, BuildingProducerSystem (execute in parallel)
Update Burst 3: ConstructCollisionFieldSystem

#### Process Events
Events that occured during the System Update stage are gathered. 
Systems that implement IEventListener<> will receive notifications of events in a single-threaded fashion based on the system update priority.

#### Output
The list of events is then passed as the tick result.

### Limitations

- Entities may only be added during a single-threaded update burst to prevent IDs from being assigned incorrectly on different simulations, breaking determinism.