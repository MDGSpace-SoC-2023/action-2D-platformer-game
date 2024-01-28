using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems {
    internal class HoldingSystem : AEntitySetSystem<float> {
        public HoldingSystem(World world) : base(world.GetEntities().With<HeldComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity) {
            ref var holding = ref entity.Get<HeldComponent>();
            ref var position = ref entity.Get<PositionComponent>();
            ref var holderPosition = ref holding.Holder.Get<PositionComponent>();

            // entity.Get<CollisionIgnore>().entities.Add(holding.Holder);
            // position.Position = holderPosition.Position - new Vector2((position.Hitbox.Width - holderPosition.Hitbox.Width)/2, position.Hitbox.Height);
            position.Position = holderPosition.Position + holding.Holder.Get<HolderComponent>().HoldPosition;
            base.Update(state, in entity);
        }
    }
    
    internal class OnHold : AEntitySetSystem<float> {
        public OnHold(World world) : base(world.GetEntities().WhenAdded<HeldComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity) {
            entity.Get<Holdable>().OnHold.Invoke(entity);
            base.Update(state, in entity);
        }
    }

    internal class OnThrow : AEntitySetSystem<float> {
        public OnThrow(World world) : base(world.GetEntities().WhenRemoved<HeldComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity) {
            // entity.Get<TimedActions>().Add(e => e.Get<CollisionIgnore>().entities.Clear(),0.125f);
            entity.Get<Holdable>().OnThrow.Invoke(entity);
            base.Update(state, in entity);
        }
    }
}
