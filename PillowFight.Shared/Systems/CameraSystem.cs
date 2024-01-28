using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class CameraSystem : AEntitySetSystem<float>
    {
        private readonly Vector2 VelocityCamera = new Vector2(10, 3);
        public CameraSystem(World world) : base(world.GetEntities().With<Camera>().With<PositionComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity)
        {
            ref var camera = ref entity.Get<Camera>();
            ref var position = ref entity.Get<PositionComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();
            camera.Position = Vector2.Lerp(position.Position + velocity.Velocity * VelocityCamera, camera.Position, 0.55f);
            camera.CenterOrigin();
            base.Update(state, in entity);
        }
    }
}
