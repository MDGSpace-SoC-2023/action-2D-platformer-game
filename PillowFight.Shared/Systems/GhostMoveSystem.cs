using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems {
    internal class GhostMoveSystem : AEntitySetSystem<float>
    {
        public GhostMoveSystem(World world) : base(world.GetEntities().With<PositionComponent>().With<VelocityComponent>().Without<SolidCollider>().AsSet()) { }

		protected override void Update(float deltaTime, in Entity entity) {
			ref var position = ref entity.Get<PositionComponent>();
			ref var velocity = ref entity.Get<VelocityComponent>();

			position.Position += velocity.Velocity * deltaTime * 100;
		}
    }
}
