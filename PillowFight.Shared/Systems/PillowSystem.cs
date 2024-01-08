using System;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using PillowFight.Shared.Components;
using static PillowFight.Shared.Enums.PillowState;
namespace PillowFight.Shared.Systems
{
    internal class PillowSystem : AEntitySetSystem<float>
    {
        public PillowSystem(World world) 
            : base(world.GetEntities().With<PillowComponent>().AsSet(), true) { }

        protected override void Update(float state, in Entity entity)
        {
            ref var pillow = ref entity.Get<PillowComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();
            ref var item = ref entity.Get<ItemStatus>();
            ref var render = ref entity.Get<RenderModifier>();

            float dx = velocity.X - pillow.Velocity.X;
            float dy = velocity.Y - pillow.Velocity.Y;
            float impulse = (float) Math.Pow(dx * dx + .4 * dy * dy, .5);

            pillow.Velocity = velocity.Velocity;

            float damage = pillow.ExplosionDamage;

            if (pillow.State == Stationery)
            {
                
                if (velocity.Velocity.Length() < pillow.DecayVelocityThreshold && !item.Airborne)
                {
                    if (pillow.DecayTimer < 0)
                    {
                        var colliders = entity.Get<Colliders>().ColliderList;
                        foreach (var c in colliders)
                        {
                            // if (!c.Get<Colliders>().ColliderList.Remove(entity)) throw new NullReferenceException();
                            c.Get<Colliders>().ColliderList.Remove(entity);
                        }
                        entity.Dispose();
                        // entity.Disable();
                        // ref var holder = ref entity.Get<Holdable>().Holder;
                        // if (holder != null)
                        // {
                        //     holder.Value.Get<HolderComponent>().Holding = null;
                        // }
            
                        // entity.Dispose();
                    }
                    pillow.DecayTimer -= state;
                    render.Flicker = .3f;
                }
                else
                {
                    pillow.DecayTimer = pillow.DecayTime;
                    render.Flicker = 0;
                }
            }

            if (impulse > pillow.ProjectileThreshold || pillow.State == Running /*&& pillow.State != Projectile*/)
            {
                // var phy = entity.Get<ModifiableComponent<ItemPhysics>>();
                // phy.Modified.Friction = 0;
                // phy.Modified.AirFriction = 0;
                // phy.Modified.UniversalAcceleration = Vector2.Zero;
                // entity.Disable<ModifiableComponent<ItemPhysics>>();
                entity.Disable<AccelerationComponent>();

                render.Color = render.Color == Color.Blue ? Color.Blue : Color.Red;

                // pillow.State = Projectile;
                entity.Get<TimedActions>().Add(e => e.Get<PillowComponent>().State = Projectile, .125f);
            }

            bool running = pillow.State == Running;

            if (pillow.State == Projectile)
            {

                var colliders = entity.Get<Colliders>();
                var stageCollider = entity.Get<StageCollider>();

                bool stageColliding = 
                    // stageCollider.BottomLeftSide ||
                    //                   stageCollider.BottomRightSide ||
                                      stageCollider.TopLeftSide ||
                                      stageCollider.TopRightSide ||
                                      stageCollider.Head ||

                                      (!running && (stageCollider.LeftFoot ||
                                                    stageCollider.RightFoot));

                if (colliders.ColliderList.Count > 0 || stageColliding)
                {
                    ref var position = ref entity.Get<PositionComponent>();
                    foreach (var collider in colliders.ColliderList)
                    {
                        collider.Set(new DamageComponent(pillow.ExplosionDamage, render.Color));
                        collider.Set(new ImpulseComponent(pillow.ExplosionImpulse * (collider.Get<PositionComponent>().Position - position.Position).NormalizedCopy()));
                        // collider.Get<Colliders>().ColliderList.Remove(entity);
                        if (pillow.OnExplode != null)
                            pillow.OnExplode.Invoke(collider);
                    }

                    if (entity.Has<HeldComponent>())
                        entity.Get<HeldComponent>().Holder.Get<HolderComponent>().Holding = null;
                    // entity.Dispose();

                    // if (World.GetEntities().AsSet().Contains(entity))
                    //     throw new NullReferenceException("My lif is a fuc");
                    entity.Set(new DamageComponent(2, Color.White));
                }
            }
        }
    }
}
