using System;
using System.Collections.Generic;
using System.Text;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class CameraSystem : AEntitySetSystem<float>
    {
        public CameraSystem(World world) : base(world.GetEntities().With<CameraComponent>().With<PositionComponent>().AsSet()) { }

        protected override void Update(float state, in Entity entity)
        {
            ref var camera = ref entity.Get<CameraComponent>();
            ref var position = ref entity.Get<PositionComponent>();
            camera.Camera.LookAt(new Vector2(position.X + camera.Window.ClientBounds.Width/2f, camera.Window.ClientBounds.Height/2f));
            base.Update(state, in entity);
        }
    }
}
