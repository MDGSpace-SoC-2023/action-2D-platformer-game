using System;
using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class PhysicsSystem : AEntitySetSystem<float>
    {
        public PhysicsSystem(World world)
            : base(world.GetEntities().With<VelocityComponent>().With<AccelerationComponent>().With<ModifiableComponent<ItemPhysics>>().With<ItemStatus>().AsSet()) { }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var velocity = ref entity.Get<VelocityComponent>();
            ref var acceleration = ref entity.Get<AccelerationComponent>();
            ref var physics = ref entity.Get<ModifiableComponent<ItemPhysics>>();
            ref var status = ref entity.Get<ItemStatus>();

            status.Falling = velocity.Y < 0;

            if (Math.Abs(velocity.X) < physics.Modified.MinXVelocity)
            {
                velocity.X = 0;
            }
            else
            {
                velocity.X -= (status.Airborne ? physics.Modified.AirFriction : physics.Modified.Friction) * velocity.X * deltaTime;
            }

            velocity.Velocity += physics.Modified.UniversalAcceleration * deltaTime;
            velocity.X = Math.Min(Math.Max(velocity.X, -physics.Modified.MaxXVelocity), physics.Modified.MaxXVelocity);
            velocity.Y = Math.Min(Math.Max(velocity.Y, -physics.Modified.MaxYVelocity), physics.Modified.MaxYVelocity);
        }
    }
}
