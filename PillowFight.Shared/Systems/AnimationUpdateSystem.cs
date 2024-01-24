using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class AnimationUpdateSystem : AComponentSystem<float, AsepriteSprite>
    {
        public AnimationUpdateSystem(World world) : base(world)
        { }

        protected override void Update(float deltaTime, ref AsepriteSprite component)
        {
            component.sprites[component.Index].Update(deltaTime);
            base.Update(deltaTime, ref component);
        }
    }
}
