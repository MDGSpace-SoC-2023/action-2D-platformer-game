
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

		protected override void Update(float deltaTime, in Entity entity)
		{
			ref var position = ref entity.Get<PositionComponent>();
			ref var sprite = ref entity.Get<AsepriteSprite>();
			ref var hud = ref entity.Get<HealthHUD>();
			ref var health = ref entity.Get<HealthComponent>();

			float lostHealthRatio = 1 - health.Health / health.MaxHealth;
			Vector2 imagePosition = Vector2.Transform(position.Position, Game1.Camera.TransformationMatrix);
			int hudYOffset = (int)(position.Hitbox.Height * lostHealthRatio);
			Color hudColor = Color.Lerp(Color.Green, Color.Red, lostHealthRatio);

			_spriteBatch.Draw(_renderTarget, hud.Position,
			new Rectangle((int)imagePosition.X, (int)imagePosition.Y, position.Hitbox.Width, position.Hitbox.Height),
					 Color.DarkSlateGray, 0, Vector2.Zero, hud.Scale, SpriteEffects.None, 0);

			_spriteBatch.Draw(_renderTarget, new Vector2(hud.Position.X, hud.Position.Y + hudYOffset * hud.Scale.Y),
			new Rectangle((int)imagePosition.X, (int)(imagePosition.Y + hudYOffset), position.Hitbox.Width, position.Hitbox.Height - hudYOffset),
					 hudColor, 0, Vector2.Zero, hud.Scale, SpriteEffects.None, 0);
		}
	}
}
