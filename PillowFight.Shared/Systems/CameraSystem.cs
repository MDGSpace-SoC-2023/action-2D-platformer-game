using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class CameraSystem : AEntitySetSystem<float>
    {
        public CameraSystem(World world) : base(world.GetEntities().With<Camera>().With<PositionComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity)
        {
            ref var camera = ref entity.Get<Camera>();
            ref var position = ref entity.Get<PositionComponent>();
            camera.Position = position.Position - new Vector2(camera.Viewport.Width * Enums.Camera.CameraXOffsetRatio, camera.Viewport.Height * Enums.Camera.CameraYOffsetRatio);
            // camera.CenterOrigin();
            base.Update(state, in entity);
        }
    }
}
