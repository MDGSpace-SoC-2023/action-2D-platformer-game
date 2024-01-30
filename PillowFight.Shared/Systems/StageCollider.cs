using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
	internal class StageColliderSystem : AEntitySetSystem<float>
	{
		private TiledMapTileLayer _collisionLayer;
		public StageColliderSystem(World world, TiledMap map) : base(world.GetEntities().With<SolidCollider>().AsSet())
		{
			_collisionLayer = map.GetLayer<TiledMapTileLayer>("Collision");
		}

		protected override void Update(float deltaTime, in Entity entity)
		{
			ref var position = ref entity.Get<PositionComponent>();
			ref var collider = ref entity.Get<SolidCollider>();
			ref var stageCollider = ref entity.Get<StageCollider>();

			foreach (var offset in collider.BottomColliders)
			{
				Vector2 Position = position.Position + offset;
				_collisionLayer.TryGetTile((ushort)(Position.X / 16), (ushort)(Position.Y / 16), out var tile);
				stageCollider.Bottom = tile.HasValue ? tile.Value.GlobalIdentifier : 0;
			}

			foreach (var offset in collider.TopColliders)
			{
				Vector2 Position = position.Position + offset;
				_collisionLayer.TryGetTile((ushort)(Position.X / 16), (ushort)(Position.Y / 16), out var tile);
				stageCollider.Top = tile.HasValue ? tile.Value.GlobalIdentifier : 0;
			}

			foreach (var offset in collider.LeftColliders)
			{
				Vector2 Position = position.Position + offset;
				_collisionLayer.TryGetTile((ushort)(Position.X / 16), (ushort)(Position.Y / 16), out var tile);
				stageCollider.Left= tile.HasValue ? tile.Value.GlobalIdentifier : 0;
			}

			foreach (var offset in collider.RightColliders)
			{
				Vector2 Position = position.Position + offset;
				_collisionLayer.TryGetTile((ushort)(Position.X / 16), (ushort)(Position.Y / 16), out var tile);
				stageCollider.Right = tile.HasValue ? tile.Value.GlobalIdentifier : 0;
			}
		}
	}
}
