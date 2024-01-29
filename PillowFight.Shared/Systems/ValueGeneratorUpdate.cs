using System;
using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
	internal class ValueGeneratorUpdater : AEntitySetSystem<float>
	{
		public ValueGeneratorUpdater(World world) : base(world.GetEntities().With<SineGenerator>().AsSet()) { }

		protected override void Update(float deltaTime, in Entity entity)
		{
			ref var generator = ref entity.Get<SineGenerator>();
			generator.Time += deltaTime;
			generator.Time %= generator.Frequency * MathF.PI * 2;
			generator.Target.Invoke(entity, generator.Value);
		}
	}
}
