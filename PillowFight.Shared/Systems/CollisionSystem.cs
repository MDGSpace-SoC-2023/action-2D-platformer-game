using System;
using System.Collections.Generic;
using System.Text;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class CollisionSystem : AEntitySetSystem<float>
    {
        public CollisionSystem(World world) 
            : base(world.GetEntities().With<PositionComponent>().With<Colliders>().With<CollisionComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity)
        {
            ref var colliders = ref entity.Get<Colliders>();
            ref var position = ref entity.Get<PositionComponent>();
            ref var collision = ref entity.Get<CollisionComponent>();
            
            var entities = World.GetEntities().With<PositionComponent>().With<CollisionComponent>().AsSet().GetEntities();
            foreach (var collider in entities)
            {
                if (collider != entity)
                    if (colliders.ColliderList.Contains(collider))
                    {
                        if (!position.Hitbox.Intersects(collider.Get<PositionComponent>().Hitbox))
                        {
                            colliders.ColliderList.Remove(collider);
                        }
                    }
                    else
                    {
                        if (position.Hitbox.Intersects(collider.Get<PositionComponent>().Hitbox))
                        {
                            colliders.ColliderList.Add(collider);
                            
                            ref var vel = ref entity.Get<VelocityComponent>();

                            collider.Set(new ImpulseComponent(collision.Mass * vel.Velocity * 0.8f));
                        }
                    }
            }

            base.Update(state, in entity);
        }
    }
}
