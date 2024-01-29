using DefaultEcs;
using DefaultEcs.System;
using MonoGame.Extended.Tiled;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
	internal class StageCollider : AEntitySetSystem<float>
	{
		private TiledMap _map;
		public StageCollider(World world) : base(world.GetEntities().With<SolidCollider>().AsSet()) { }

		protected override void Update(float deltaTime, in Entity entity)
		{

		}
	}
}
