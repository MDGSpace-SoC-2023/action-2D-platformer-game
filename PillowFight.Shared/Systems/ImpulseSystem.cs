using System;
using System.Collections.Generic;
using System.Text;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class ImpulseSystem : AEntitySetSystem<float>
    {
        public ImpulseSystem(World world)
            : base(world.GetEntities().With<ImpulseComponent>().With<VelocityComponent>().With<CollisionComponent>().AsSet()) { }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var impulse = ref entity.Get<ImpulseComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();
            ref var collision = ref entity.Get<CollisionComponent>();

            if (collision.ReceiveImpulse)
            {
                velocity.Velocity = impulse.Impulse + (impulse.overrideVel ? Vector2.Zero : velocity.Velocity);
            }

            entity.Remove<ImpulseComponent>();
        }
    }
}
