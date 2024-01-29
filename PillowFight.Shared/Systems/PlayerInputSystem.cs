using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class PlayerInputSystem : AEntitySetSystem<float>
    {
        public PlayerInputSystem(World world)
            : base(world.GetEntities().With<InputComponent>().With<PlayerInputSource>().AsSet())
        { }

        protected override void Update(float state, in Entity entity)
        {
            ref var input = ref entity.Get<InputComponent>();
            ref var source = ref entity.Get<PlayerInputSource>();

            input.PreviousState = input.CurrentState;
            input.CurrentState = source.InputSource();

            base.Update(state, in entity);
        }
    }
}
