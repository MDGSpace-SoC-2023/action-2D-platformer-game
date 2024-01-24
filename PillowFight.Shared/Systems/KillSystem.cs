using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems {
	internal class KillSystem : AEntitySetSystem<float> {
		public KillSystem(World world) : base(world.GetEntities().With<KillComponent>().AsSet(), true) {}

		protected override void Update(float state, in Entity entity) {
			entity.Dispose();
		}
	}
}
