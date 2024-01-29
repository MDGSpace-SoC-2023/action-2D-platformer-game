using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    public class EntityRenderSystem : AEntitySetSystem<float>
    {
        private GraphicsDevice _graphicsDevice;
        public SpriteBatch _spriteBatch;
        private Camera _camera;

        public EntityRenderSystem(World world, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Camera camera)
            : base(world.GetEntities().With<AsepriteSprite>().With<PositionComponent>().AsSet())
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _camera = camera;
        }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var sprite = ref entity.Get<AsepriteSprite>();
            ref var position = ref entity.Get<PositionComponent>();
            ref var itemS = ref entity.Get<ItemStatus>();
            ref var modifier = ref entity.Get<RenderModifier>();

            Vector2 Position = Vector2.Transform(position.Position, _camera.TransformationMatrix);

            var activeSprite = sprite.Sprite;
            activeSprite.FlipHorizontally = itemS.Direction == -1;
            activeSprite.Color = modifier.Color;
            activeSprite.Draw(_spriteBatch, Position);
            if (modifier.ShouldCycle) modifier.Update(deltaTime);
        }
    }
}
