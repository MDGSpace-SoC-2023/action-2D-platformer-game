using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
	internal class ConditionCheckerSystem : AComponentSystem<float, LevelCompleteConditions>
	{
		public ConditionCheckerSystem(World world) : base(world)
		{ }

		protected override void Update(float deltaTime, ref LevelCompleteConditions conditions)
		{
			var checks = conditions.Checks;
			foreach (var check in checks)
			{
				if (check.Invoke())
				{

				}
			}
		}
	}
}
