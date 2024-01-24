using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class DamageSystem : AEntitySetSystem<float>
    {
        public DamageSystem(World world) 
            : base(world.GetEntities().With<DamageComponent>().With<HealthComponent>().AsSet()) { }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var damage = ref entity.Get<DamageComponent>();
            ref var health = ref entity.Get<HealthComponent>();
            ref var render = ref entity.Get<RenderModifier>();
            ref var actions = ref entity.Get<TimedActions>();

            Color color = damage.Color;

            health.Health -= damage.Damage * health.DamageMultiplier;

            // actions.Add(e => e.Set(render), );

            render.FlickerTimer = .3f;
            render.OffColor = color;
            actions.Add((e =>
            {
                ref var render = ref e.Get<RenderModifier>();
                // e.Get<RenderModifier>().Color = Color.White;
                e.Get<RenderModifier>().FlickerTimer = 0;
            }), 2);

			health.OnDamage?.Invoke(entity);
            entity.Remove<DamageComponent>();
            
            if (health.Health < 0)
            {
				entity.Set(new KillComponent());
				health.OnDeath?.Invoke(entity);
            }
        }

    }
}
