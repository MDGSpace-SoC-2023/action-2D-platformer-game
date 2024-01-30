using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems {
	internal class FollowingSystem : AEntitySetSystem<float> {
		public FollowingSystem(World world) : base(world.GetEntities().With<Follower>().AsSet()) { }

		protected override void Update(float deltaTime, in Entity entity) {
			ref var follower = ref entity.Get<Follower>();
			ref var position = ref entity.Get<PositionComponent>();
			ref var velocity = ref entity.Get<VelocityComponent>();
			var targetPos = follower.Target.Get<PositionComponent>().Position - position.Position;
			targetPos.Normalize();
			if (follower.VelocityMonitor) {
				var targetVel = follower.Target.Get<VelocityComponent>().Velocity;			
				targetVel.Normalize();
				targetPos += targetVel;
			}
			velocity.Velocity = targetPos * follower.Velocity;
		}
	}
}
