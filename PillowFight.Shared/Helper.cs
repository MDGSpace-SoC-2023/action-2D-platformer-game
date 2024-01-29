using DefaultEcs;
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


	}
}
