using System;
using DefaultEcs;
using Microsoft.Xna.Framework;
using PillowFight.Shared.Components;

namespace PillowFight.Shared
{
	internal class Helper
	{

		public static void PillowOnHold(Entity entity)
		{

			entity.Disable<ItemProperties>();
			entity.Disable<ItemStatus>();
			entity.Disable<VelocityComponent>();
			entity.Disable<ModifiableComponent<ItemPhysics>>();
			entity.Disable<CollisionComponent>();
			entity.Disable<Colliders>();
			entity.Disable<HolderComponent>();
		}

		public static void PillowOnThrow(Entity pillow)
		{
			if (pillow.Get<SolidCollider>().Colliding)
			{
				// pillow.Remove<ImpulseComponent>();
				// pillow.Set(new VelocityComponent());
				pillow.Set(new KillComponent());
			}
			pillow.Get<PillowComponent>().State = Enums.PillowState.Stationery;

			pillow.Enable<ItemProperties>();
			pillow.Enable<ItemStatus>();
			pillow.Enable<VelocityComponent>();
			pillow.Enable<ModifiableComponent<ItemPhysics>>();
			pillow.Enable<CollisionComponent>();
			pillow.Enable<HolderComponent>();
		}

		public static Entity? GetFirstNear(EntityQueryBuilder builder, Vector2 position, float distance)
		{
			ReadOnlySpan<Entity> entities = builder.With<PositionComponent>().AsSet().GetEntities();
			foreach (var entity in entities)
			{
				float length = (entity.Get<PositionComponent>().Position - position).Length();
				if (length < distance) return entity;
			}
			return null;
		}

		public static void ForAllNear(EntityQueryBuilder builder, Vector2 position, float distance, Entity entity, Action<Entity, Entity> action) {
			ReadOnlySpan<Entity> entities = builder.With<PositionComponent>().AsSet().GetEntities();
			foreach (var collider in entities)
			{
				float length = (entity.Get<PositionComponent>().Position - position).Length();
				if (length < distance) action.Invoke(entity, collider);
			}
		}
	}
}
