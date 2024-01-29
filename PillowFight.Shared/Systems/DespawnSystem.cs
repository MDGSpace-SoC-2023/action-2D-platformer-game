using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
	internal class DespawnSystem : AEntitySetSystem<float>
	{
		public DespawnSystem(World world) : base(world.GetEntities().With<PositionComponent>().Without<NoOffscreenDespawn>().AsSet()) { }

		protected override void Update(float state, in Entity entity)
		{
			ref var position = ref entity.Get<PositionComponent>();
			Camera camera = Game1.Camera;
			Vector2 screenPosition = Vector2.Transform(position.Position, camera.TransformationMatrix);

			float leftDistance = screenPosition.X - camera.Viewport.Bounds.Left;
			float rightDistance = screenPosition.X - camera.Viewport.Bounds.Right;
			float upDistance = screenPosition.Y - camera.Viewport.Bounds.Top;
			float downDistance = screenPosition.Y - camera.Viewport.Bounds.Bottom;

			if (leftDistance < -Enums.Gameplay.DespawnDistance ||
				rightDistance > Enums.Gameplay.DespawnDistance ||
				upDistance < -Enums.Gameplay.DespawnDistance ||
				downDistance > Enums.Gameplay.DespawnDistance)
			{
				entity.Set(new KillComponent());
			}
		}
	}
}
