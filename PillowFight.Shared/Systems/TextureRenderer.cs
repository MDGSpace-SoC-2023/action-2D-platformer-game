using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class TextureRenderer : AEntitySetSystem<float>
	{
        private GraphicsDevice _graphicsDevice;
        public SpriteBatch _spriteBatch;
        private Camera _camera;

		public TextureRenderer(World world, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Camera camera) : base(world.GetEntities().With<Texture2D>().AsSet()) {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _camera = camera;
		}

		protected override void Update(float deltaTime, in Entity entity) {
			
            ref var sprite = ref entity.Get<Texture2D>();
            ref var position = ref entity.Get<PositionComponent>();
            ref var itemS = ref entity.Get<ItemStatus>();
            ref var modifier = ref entity.Get<RenderModifier>();

            Vector2 Position = Vector2.Transform(position.Position, _camera.TransformationMatrix);

			_spriteBatch.Draw(sprite, Position, null, modifier.Color, 0, Vector2.Zero, 1, itemS.Direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            if (modifier.ShouldCycle) modifier.Update(deltaTime);
		}
	}
}
