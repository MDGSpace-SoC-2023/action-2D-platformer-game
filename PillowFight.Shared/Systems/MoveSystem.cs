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

        public MoveSystem(World world, Func<TiledMap> map)
            : base(world.GetEntities().With<PositionComponent>().With<StageCollider>().AsSet())
        {
            _map = map;
        }

        protected override void Update(float state, in Entity entity)
        {
            ref var position = ref entity.Get<PositionComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();
            ref var itemPhysics = ref entity.Get<ModifiableComponent<ItemPhysics>>();
            ref var itemStatus = ref entity.Get<ItemStatus>();
            ref var stageCollider = ref entity.Get<StageCollider>();

            _tileLayer = _map().GetLayer<TiledMapTileLayer>("Collision");

            position.XRemainder += velocity.X;
            position.YRemainder += velocity.Y;

            int moveX = (int)Math.Truncate(position.XRemainder);
            int moveY = (int)Math.Truncate(position.YRemainder);


            if (moveY != 0)
            {
                position.YRemainder -= moveY;
                int sign = Math.Sign(moveY);
                while (moveY != 0)
                {
                    bool shouldBreak = false;
                    if (sign > 0)
                    {
                        TiledMapTile collidingTile = CollidesWithMap(position.TopCenter - Vector2.UnitY);
                        bool collidesTop = shouldBreak = stageCollider.Head = collidingTile.GlobalIdentifier != 0;

                        if (collidesTop)
                        {
                            velocity.Y *= -itemPhysics.Modified.YRestitution;
                        }
                        else
                        {
                            itemStatus.Airborne = true;

                            position.Y -= sign;
                            moveY -= sign;
                        }
                    }
                    else
                    {
                        TiledMapTile collidingRightTile = CollidesWithMap(position.BottomRight + Vector2.UnitY);
                        TiledMapTile collidingLeftTile = CollidesWithMap(position.BottomLeft + Vector2.UnitY);
                        bool collidesBottomRight = stageCollider.RightFoot = collidingRightTile.GlobalIdentifier != 0;
                        bool collidesBottomLeft = stageCollider.LeftFoot = collidingLeftTile.GlobalIdentifier != 0;


                        if (collidesBottomRight)
                        {
                            itemStatus.Airborne = false;
                            shouldBreak = true;
                            velocity.Y *= -itemPhysics.Modified.YRestitution;
                        }
                        else if (collidesBottomLeft)
                        {
                            itemStatus.Airborne = false;
                            shouldBreak = true;
                            velocity.Y *= -itemPhysics.Modified.YRestitution;
                        }
                        else
                        {
                            itemStatus.Airborne = true;
                            position.Y -= sign;
                            moveY -= sign;

                            // itemStatus.Airborne = true;
                        }

                    }

                    if (shouldBreak) break;
                }
            }

            if (moveX != 0)
            {
                position.XRemainder -= moveX;
                int sign = Math.Sign(moveX);

                while (moveX != 0)
                {
                    TiledMapTile topTile = CollidesWithMap((moveX > 0 ? position.TopRight : position.TopLeft) + Vector2.UnitX * sign);
                    TiledMapTile bottomTile = CollidesWithMap((moveX > 0 ? position.BottomRight : position.BottomLeft) + Vector2.UnitX * sign);

                    stageCollider.BottomLeftSide = bottomTile.GlobalIdentifier != 0;
                    stageCollider.TopLeftSide = topTile.GlobalIdentifier != 0;

                    if (topTile.GlobalIdentifier == 0 && bottomTile.GlobalIdentifier == 0)
                    {
                        position.X += sign;
                        moveX -= sign;
                    }
                    else
                    {
                        velocity.X *= -itemPhysics.Modified.XRestitution;
                        break;
                    }
                }
            }
            base.Update(state, in entity);
        }

        private TiledMapTile CollidesWithMap(Vector2 position)
        {
            _tileLayer.TryGetTile((ushort) (position.X / 32), (ushort) ((position.Y / 32)), out var tile);
            return tile ?? default;
        }
    }
}
