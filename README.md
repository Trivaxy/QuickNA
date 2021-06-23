# QuickNA
Simple and concise wrapper around FNA. A lightweight game engine if you will.

QuickNA aims to remove most of the overhead of developing FNA projects by forcing you to use its architecture, which is based on a bit of an odd mix between object oriented programming and entity-component systems.

QuickNA is based on two concepts: Entities and Behaviors.
- Entities are objects that have an ID. You extend from the base Entity class to create more complex entities with fields and properties. You'll notice that entities do not update on their own.
- Behaviors are essentially methods that run every tick in your game. To make your entities do things, you must create behaviors for them, and there can be different behaviors doing different things to an entity - this lets you separate logic in your game in a clear manner (and, in the future, mix & match behaviors).

What if I want an entity that moves? Well, code conveys more than English in less characters.
Start off by creating a class that derives from `Entity`:
```cs
public class MovingEntity : Entity
{
    public Vector2 Position;
    public Vector2 Velocity;
}
```

`MovingEntity` doesn't do anything on its own. Let's create a behavior for it:
```cs
public class MovingBehavior : QueryBehavior<MovingEntity>
{
    public void Update(MovingEntity entity)
    {
        entity.Position += entity.Velocity;
    }
}
```
Done! Behaviors in QuickNA must be registered before they can be used, which is super simple.

You should probably read the [wiki](https://github.com/Trivaxy/QuickNA/wiki) first before before moving on to registering; it covers most of QuickNA in 10 minutes.