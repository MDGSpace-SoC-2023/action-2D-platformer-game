using DefaultEcs;
using DefaultEcs.System;
using MonoGame.Aseprite.Sprites;

namespace PillowFight.Shared.Systems
{
	internal class TextureRenderer : AEntitySetSystem<float>
	{
		public TextureRenderer(World world) : base(world.GetEntities().With<Sprite>().AsSet()) { }

		// protected override void Update()
	}
}
