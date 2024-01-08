using DefaultEcs;
using DefaultEcs.System;
using MonoGame.Extended.Sprites;

namespace PillowFight.Shared.Systems
{
    internal class AnimationUpdateSystem : AComponentSystem<float, AnimatedSprite>
    {
        public AnimationUpdateSystem(World world) : base(world)
        { }

        protected override void Update(float deltaTime, ref AnimatedSprite component)
        {
            component.Update(deltaTime);
            base.Update(deltaTime, ref component);
        }
    }
}
