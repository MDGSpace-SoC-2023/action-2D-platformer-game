
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems {
	public class DebugSystem : AEntitySetSystem<float> {

        private GraphicsDevice _graphicsDevice;
        public SpriteBatch _spriteBatch;

		public DebugSystem(World world, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) 
			: base(world.GetEntities().With<DebugComponent>().AsSet()) { 
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
		}

		protected override void Update (float deltaTime, in Entity entity) {
			
			ref var position = ref entity.Get<PositionComponent>();
            ref var velocity  = ref entity.Get<VelocityComponent>();
            ref var itemP  = ref entity.Get<ModifiableComponent<ItemPhysics>>();
			var bounds = Game1.Camera.Viewport.Bounds;
            _spriteBatch.DrawString(Assets.Fonts["Arial"], position.X.ToString() + " " +position.Y.ToString(), new Vector2(0,30), Color.White);
            _spriteBatch.DrawString(Assets.Fonts["Arial"], Game1.Camera.Position.X.ToString() + " " +Game1.Camera.Position.Y.ToString(), new Vector2(0,40), Color.White);
            _spriteBatch.DrawString(Assets.Fonts["Arial"], velocity.X.ToString(), new Vector2(0,0), Color.White);
            _spriteBatch.DrawString(Assets.Fonts["Arial"], velocity.Y.ToString(), new Vector2(0,10), Color.White);
            _spriteBatch.DrawString(Assets.Fonts["Arial"], World.GetEntities().AsSet().Count.ToString(), new Vector2(0,50), Color.White);
            _spriteBatch.DrawString(Assets.Fonts["Arial"], bounds.Top+" "+bounds.Bottom+" "+bounds.Left+" "+bounds.Right, new Vector2(0,60), Color.White);
            _spriteBatch.DrawString(Assets.Fonts["Arial"], World.GetEntities().AsSet().Count.ToString(), new Vector2(0,50), Color.White);
			
		}
	}
}
