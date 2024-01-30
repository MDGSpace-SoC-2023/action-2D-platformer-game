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
            : base(world.GetEntities().With<PillowComponent>().AsSet()) { }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var pillow = ref entity.Get<PillowComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();
            ref var item = ref entity.Get<ItemStatus>();
            ref var render = ref entity.Get<RenderModifier>();

            float damage = pillow.ExplosionDamage;
            float dx = velocity.X - pillow.Velocity.X;
            float dy = velocity.Y - pillow.Velocity.Y;
            float impulse = (float)Math.Pow(dx * dx + .4 * dy * dy, .5);
            bool shouldDie = false;

            pillow.Velocity = velocity.Velocity;
            if (impulse > pillow.ProjectileThreshold && pillow.State != Running) pillow.State = Projectile;

            if (impulse > pillow.ProjectileThreshold || pillow.State == Running /*&& pillow.State != Projectile*/)
            {
                entity.Disable<AccelerationComponent>();
                velocity.Velocity = velocity.Velocity.NormalizedCopy() * pillow.ProjectileSpeed;
                render.OnColor = render.OnColor == Color.Blue ? Color.Blue : Color.Red;
                entity.Get<TimedActions>().Add(e => e.Get<PillowComponent>().State = Projectile, .125f);
            }

            Action a = pillow.State switch
            {
                Held => () => { }
                ,
                Projectile => () => { }
                ,
                Running => () => { }
                ,
                _ => () => { }
            };
            a.Invoke();

            var characters = World.GetEntities().With<CharacterProperties>().AsSet().GetEntities();
            ref var position = ref entity.Get<PositionComponent>();
            foreach (var character in characters)
            {
                Vector2 distance = character.Get<PositionComponent>().Position - position.Position;
                if (distance.Length() < pillow.CharacterExplosionRadius)
                {
                    if (pillow.OnExplode != null) pillow.OnExplode.Invoke(character);
                    character.Set(new DamageComponent(pillow.ExplosionDamage, render.Color));
                    character.Set(new ImpulseComponent(pillow.ExplosionImpulse * distance.NormalizedCopy()));
                    shouldDie = true;
                    break;
                }
            }
            if (pillow.State == Projectile)
            {
                // var colliders = entity.Get<Colliders>();
                var solidCollider = entity.Get<SolidCollider>();
                // var characters = World.GetEntities().With<CharacterProperties>().AsSet().GetEntities();
                // ref var position = ref entity.Get<PositionComponent>();

                if (solidCollider.Colliding)
                {
                    foreach (var character in characters)
                    {
                        Vector2 distance = character.Get<PositionComponent>().Position - position.Position;
                        float distanceMag = distance.NormalizedCopy().Length();
                        if (distance.Length() < pillow.StageExplosionRadius)
                        {
                            if (pillow.OnExplode != null) pillow.OnExplode.Invoke(character);
                            character.Set(new DamageComponent(pillow.ExplosionDamage * distanceMag, render.Color));
                            character.Set(new ImpulseComponent(pillow.ExplosionImpulse * distanceMag * distance.NormalizedCopy()));
                        }
                    }
                    shouldDie = true;
                }
                else
                {
                    foreach (var character in characters)
                    {
                        Vector2 distance = character.Get<PositionComponent>().Position - position.Position;
                        if (distance.Length() < pillow.CharacterExplosionRadius)
                        {
                            if (pillow.OnExplode != null) pillow.OnExplode.Invoke(character);
                            character.Set(new DamageComponent(pillow.ExplosionDamage, render.Color));
                            character.Set(new ImpulseComponent(pillow.ExplosionImpulse * distance.NormalizedCopy()));
                            shouldDie = true;
                            break;
                        }
                    }
                }
            }

            if (pillow.State == Stationery)
            {

                if (velocity.Velocity.Length() < pillow.DecayVelocityThreshold && !item.Airborne)
                {
                    if (pillow.DecayTimer < 0)
                    {
                        shouldDie = true;
                    }

                    pillow.PreDecayTimer -= deltaTime;
                    if (pillow.PreDecayTimer < 0)
                    {
                        pillow.DecayTimer -= deltaTime;
                        render.Flicker = .3f;
                    }
                }
                else
                {
                    pillow.PreDecayTimer = pillow.PreDecayTime;
                    pillow.DecayTimer = pillow.DecayTime;
                    render.Flicker = 0;
                }
            }

            if (shouldDie)
            {
                entity.Set(new KillComponent());
            }
        }
    }
}
