
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    public class HUDHealthSystem : AEntitySetSystem<float>
    {
		private GraphicsDevice _graphicsDevice;
		private SpriteBatch _spriteBatch;
		private RenderTarget2D _renderTarget;
        public HUDHealthSystem(World world, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, RenderTarget2D renderTarget, bool useBuffer = false) 
			: base(world.GetEntities().With<HealthHUD>().AsSet())
        {
			this._graphicsDevice = graphicsDevice;
			this._spriteBatch = spriteBatch;
			this._renderTarget = renderTarget;
        }

		protected override void Update(float deltaTime, in Entity entity) {
			ref var position = ref entity.Get<PositionComponent>();
			ref var sprite = ref entity.Get<AsepriteSprite>();
			ref var hud = ref entity.Get<HealthHUD>();

			if (hud.HealthChanged) hud.Mask = new Texture2D(_graphicsDevice, position.Hitbox.Width, (int)hud.Health);
			// _spriteBatch.Draw(sprite, )
			Vector2 imagePosition = Vector2.Transform(position.Position, Game1.Camera.TransformationMatrix);
			_spriteBatch.Draw(_renderTarget, Microsoft.Xna.Framework.Vector2.Zero, 
			new Rectangle((int)imagePosition.X, (int)imagePosition.Y, position.Hitbox.Width, position.Hitbox.Height),
					 Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 0);
		}
    }
}
