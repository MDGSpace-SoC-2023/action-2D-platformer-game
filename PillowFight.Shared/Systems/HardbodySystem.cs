using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems {
	internal class HardbodySystem : AEntitySetSystem<float> {
		public HardbodySystem(World world) 
			: base (world.GetEntities().With<Hardbody>().AsSet()) {
								
		}
	}
} 
