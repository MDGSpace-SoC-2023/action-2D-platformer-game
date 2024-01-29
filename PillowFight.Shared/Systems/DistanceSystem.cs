using System;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
	internal class DistanceSystem : AEntitySetSystem<float>
	{
		public DistanceSystem(World world) : base(world.GetEntities().With<PollDistance>().AsSet()) { }

		protected override void Update(float state, in Entity entity)
		{
			ref var poll = ref entity.Get<PollDistance>();
			ref var ignore = ref entity.Get<CollisionIgnore>();
			ref var position = ref entity.Get<PositionComponent>();

			ReadOnlySpan<Entity> colliders;
			var a = World.GetEntities();
			if (ignore.CollideType != null) colliders = ignore.CollideType.With<PositionComponent>().AsSet().GetEntities();
			else if (ignore.IgnoreType != null) colliders = ignore.IgnoreType.With<PositionComponent>().AsSet().GetEntities();
			else colliders = World.GetEntities().With<PositionComponent>().AsSet().GetEntities();

			foreach (var collider in colliders)
			{
				if (!ignore.IgnoreEntity.Contains(collider))
				{
					Vector2 distance = collider.Get<PositionComponent>().Position - position.Position;
					if (distance.Length() > poll.Distance_Action.threshold)
					{
						poll.Distance_Action.action.Invoke(entity, collider, distance);
					}
				}
			}
		}
	}
}
