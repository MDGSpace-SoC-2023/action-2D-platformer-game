using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class HoldingSystem : AEntitySetSystem<float>
    {
        public HoldingSystem(World world) : base(world.GetEntities().With<HeldComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity)
        {
            ref var holding = ref entity.Get<HeldComponent>();
            ref var position = ref entity.Get<PositionComponent>();

            position.Position = holding.Holder.Get<PositionComponent>().Position;
            // if (holding.Holding.HasValue)
                // holding.Holding.Value.Get<PositionComponent>().Position = position.Position - Vector2.UnitY * 32;
            base.Update(state, in entity);
        }
    }

    internal class HeldSystem : AComponentSystem<float, HeldComponent>
    {
        public HeldSystem(World world) : base(world) { }

        protected override void Update(float state, ref HeldComponent component)
        {
            component.Holder.Get<HolderComponent>().Holding.Value.Get<PositionComponent>().Position = component.Holder.Get<PositionComponent>().Position;
            base.Update(state, ref component);
        }
    }

    internal class OnHold : AEntitySetSystem<float>
    {
        public OnHold(World world) : base(world.GetEntities().WhenAdded<HeldComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity)
        {
            entity.Disable<VelocityComponent>();
            // entity.Disable();
            base.Update(state, in entity);
        }
    }

    internal class OnThrow : AEntitySetSystem<float>
    {
        public OnThrow(World world) : base(world.GetEntities().WhenRemoved<HeldComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity)
        {
            entity.Enable<VelocityComponent>();
            // entity.Enable();
            base.Update(state, in entity);
        }
    }
}
