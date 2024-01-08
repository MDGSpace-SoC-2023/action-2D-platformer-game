using System;
using System.Collections.Generic;
using System.Text;
using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    public class RenderSystem : AEntitySetSystem<float>
    {
        private GraphicsDevice _graphicsDevice;
        public SpriteBatch _spriteBatch;

        public RenderSystem(World world, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) 
            : base(world.GetEntities()
                .With<AnimatedSprite>().With<PositionComponent>().AsSet())
            // : base(world.GetEntities().With<Sprite>().With<PositionComponent>().AsSet())
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
        }

        protected override void Update(float deltaTime, in Entity entity)
        { 
            // Sprite sprite = entity.Has<AnimatedSprite>() ? entity.Get<AnimatedSprite>() : entity.Get<Sprite>() ;
            ref var sprite = ref entity.Get<AnimatedSprite>();
            ref PositionComponent position = ref entity.Get<PositionComponent>();
            ref var itemS  = ref entity.Get<ItemStatus>();

            ref var modifier = ref entity.Get<RenderModifier>();
            if (modifier.OnCycle)
                _spriteBatch.Draw(sprite.TextureRegion.Texture, position.Position, sprite.TextureRegion.Bounds, modifier.Color, 0, Vector2.Zero, new Vector2(1,1), itemS.Direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1);
            modifier.Update(deltaTime);
            _spriteBatch.DrawRectangle(position.Hitbox, Color.White, 1);

            if (entity.Has<DebugComponent>())
            {
                ref var velocity  = ref entity.Get<VelocityComponent>();
                ref var itemP  = ref entity.Get<ModifiableComponent<ItemPhysics>>();

                _spriteBatch.DrawString(Assets.Fonts["Arial"], position.Y.ToString(), new Vector2(0,80), Color.White);
                _spriteBatch.DrawString(Assets.Fonts["Arial"], velocity.X.ToString(), new Vector2(0,0), Color.White);
                _spriteBatch.DrawString(Assets.Fonts["Arial"], velocity.Y.ToString(), new Vector2(0,10), Color.White);
                _spriteBatch.DrawString(Assets.Fonts["Arial"], itemP.Base.UniversalAcceleration.Y.ToString(), new Vector2(0,20), Color.White);
                _spriteBatch.DrawString(Assets.Fonts["Arial"], itemP.Modified.UniversalAcceleration.Y.ToString(), new Vector2(0,30), Color.White);
                _spriteBatch.DrawString(Assets.Fonts["Arial"], entity.Get<ItemStatus>().Airborne.ToString(), new Vector2(0,50), Color.White);
                _spriteBatch.DrawString(Assets.Fonts["Arial"], entity.Get<Colliders>().ColliderList.Count.ToString(), new Vector2(0,60), Color.White);
                _spriteBatch.DrawString(Assets.Fonts["Arial"], World.GetEntities().AsSet().Count.ToString(), new Vector2(0,70), Color.White);
            }
        }

    }
}
