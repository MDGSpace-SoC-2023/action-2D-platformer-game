using System;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class MoveSystem : AEntitySetSystem<float>
    {
        private Func<TiledMap> _map;
        private TiledMapTileLayer _tileLayer;
        private Entity _entity;

        public MoveSystem(World world, Func<TiledMap> map)
            : base(world.GetEntities().With<PositionComponent>().With<SolidCollider>().AsSet())
        {
            _map = map;
        }

        protected override void Update(float deltaTime, in Entity entity)
        {
            _entity = entity;
            ref var position = ref entity.Get<PositionComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();
            ref var itemPhysics = ref entity.Get<ModifiableComponent<ItemPhysics>>();
            ref var itemStatus = ref entity.Get<ItemStatus>();
            ref var solidColliders = ref entity.Get<SolidCollider>();

            Vector2 Position = position.Position;
            _tileLayer = _map().GetLayer<TiledMapTileLayer>("Collision");

            position.XRemainder += velocity.X * deltaTime * 100;
            position.YRemainder += velocity.Y * deltaTime * 100;

            int moveX = (int)Math.Truncate(position.XRemainder);
            int moveY = (int)Math.Truncate(position.YRemainder);
            position.XRemainder -= moveX;
            position.YRemainder -= moveY;

            var solids = World.GetEntities().With<Solid>().AsSet().GetEntities();

            bool top = solidColliders.Top = CollidesDirection(position.Position, solidColliders.TopColliders, solids);
            // if (top) {
            //     velocity.Y *= -itemPhysics.Modified.YRestitution;
            // } else 
            while (moveY > 0)
            {
                if (top)
                {
                    velocity.Y *= -itemPhysics.Modified.YRestitution;
                    break;
                }
                else
                {
                    itemStatus.Airborne = true;
                    position.Y--;
                    moveY--;
                }
                top = solidColliders.Top = CollidesDirection(position.Position, solidColliders.TopColliders, solids);
            }

            Vector2? solidPosition = CollidesDirectionPosition(position.Position, solidColliders.BottomColliders, solids);
            bool bottom = solidPosition.HasValue;
            // bool bottom = solidColliders.Bottom = CollidesDirection(position.Position, solidColliders.BottomColliders, solids);

            if (bottom)
            {
                velocity.Y *= -itemPhysics.Modified.YRestitution;
                // if (solidPosition.Value.Y - position.Y < 16) position.Y = (int)solidPosition.Value.Y;
                if (position.Y % 32 < 16) position.Y -= position.Y % 32;
                // else if (moveX >= 0){
                //     while (bottom) {
                //         position.X -= itemStatus.Direction;
                //         bottom = CollidesDirection(position.Position, solidColliders.BottomColliders, solids);
                //     }
                // }
                itemStatus.Airborne = false;
            }
            else
                while (moveY < 0)
                {
                    if (bottom)
                    {
                        velocity.Y *= -itemPhysics.Modified.YRestitution;
                        if (position.Y % 32 < 16) position.Y -= position.Y % 32;
                        // if (solidPosition.Value.Y - position.Y < 8) position.Y = (int)solidPosition.Value.Y;
                        // else if (position.Y + solidColliders.BottomColliders[0].Y > solidPosition.Value.Y) {
                        // position.X = (int)(solidPosition.Value.X - solidColliders.RightColliders[0].X);
                        // position.X = (int) (solidPosition.Value.X - solidColliders.RightColliders[0].X);
                        // }
                        itemStatus.Airborne = false;
                        break;
                    }
                    else
                    {
                        itemStatus.Airborne = true;
                        position.Y++;
                        moveY++;
                    }
                    solidPosition = CollidesDirectionPosition(position.Position, solidColliders.BottomColliders, solids);
                    // bottom = solidColliders.Bottom = CollidesDirection(position.Position, solidColliders.BottomColliders, solids);
                }

            bool left = solidColliders.Left = CollidesDirection(position.Position, solidColliders.LeftColliders, solids);
            if (left)
            {
                velocity.X *= -itemPhysics.Modified.XRestitution;
                while (CollidesDirection(position.Position + Vector2.UnitX, solidColliders.LeftColliders, solids))
                {
                    position.X += 1;
                }
            }
            else while (moveX < 0)
                {
                    if (left)
                    {
                        velocity.X *= -itemPhysics.Modified.XRestitution;
                        while (CollidesDirection(position.Position + Vector2.UnitX, solidColliders.LeftColliders, solids))
                        {
                            position.X += 1;
                        }
                        break;
                    }
                    else
                    {
                        position.X--;
                        moveX++;
                    }
                }

            bool right = solidColliders.Right = CollidesDirection(position.Position, solidColliders.RightColliders, solids);

            if (right)
            {
                velocity.X *= -itemPhysics.Modified.XRestitution;
                while (CollidesDirection(position.Position - Vector2.UnitX, solidColliders.RightColliders, solids))
                {
                    // position.X -= (int) (Enums.Gameplay.BlockPushSpeed * deltaTime);
                    position.X -= 1;
                    // position.XRemainder -= .5f;
                }
            }
            else while (moveX > 0)
                {
                    if (right)
                    {
                        velocity.X *= -itemPhysics.Modified.XRestitution;
                        while (CollidesDirection(position.Position - Vector2.UnitX, solidColliders.RightColliders, solids))
                        {
                            // position.X -= (int) (Enums.Gameplay.BlockPushSpeed * deltaTime);
                            position.X -= 1;
                            // position.XRemainder -= .5f;
                        }
                        break;
                    }
                    else
                    {
                        position.X++;
                        moveX--;
                    }
                }

            base.Update(deltaTime, in entity);
        }

        private TiledMapTile CollidesWithMap(Vector2 position)
        {
            _tileLayer.TryGetTile((ushort)(position.X / 32), (ushort)((position.Y / 32)), out var tile);
            return tile ?? default;
        }

        private bool CollidesDirection(Vector2 position, Vector2[] offsets, ReadOnlySpan<Entity> solids)
        {
            foreach (var offset in offsets)
            {
                Vector2 Position = position + offset;
                _tileLayer.TryGetTile((ushort)(Position.X / 32), (ushort)(Position.Y / 32), out var tile);
                if (tile.HasValue && tile.Value.GlobalIdentifier != 0) return true;
                else foreach (var solid in solids)
                    {
                        if (solid.Get<PositionComponent>().Hitbox.Contains(Position) && _entity != solid) return true;
                    }
            }
            return false;
        }

        private Vector2? CollidesDirectionPosition(Vector2 position, Vector2[] offsets, ReadOnlySpan<Entity> solids)
        {

            foreach (var offset in offsets)
            {
                Vector2 Position = position + offset;
                _tileLayer.TryGetTile((ushort)(Position.X / 32), (ushort)(Position.Y / 32), out var tile);
                if (tile.HasValue && tile.Value.GlobalIdentifier != 0) return new Vector2(Position.X - Position.X % 32, Position.Y - Position.Y % 32);
                else foreach (var solid in solids)
                    {
                        ref var solidPosition = ref solid.Get<PositionComponent>();
                        if (solid.Has<HeldComponent>() && solid.Get<HeldComponent>().Holder == _entity) continue;
                        if (solidPosition.Hitbox.Contains(Position) && _entity != solid) return solidPosition.Position;
                    }
            }
            return null;
        }
    }
}
