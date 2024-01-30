using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class CameraSystem : AEntitySetSystem<float>
    {
        private readonly Vector2 VelocityCamera = new Vector2(10, 3);
        // private TiledMapTileLayer _cameraBounds;
        private List<(int X, int Y)>[] _cameraBounds;
        public CameraSystem(World world, TiledMap bounds) : base(world.GetEntities().With<Camera>().With<PositionComponent>().AsSet())
        {
            // _cameraBounds = new List<(int, int)>[4];
            _cameraBounds = new[]{new List<(int, int)>(), new List<(int, int)>(), new List<(int, int)>(), new List<(int, int)>()};
            LoadBounds(bounds.GetLayer<TiledMapTileLayer>("Camera"));
        }

        protected override void Update(float state, in Entity entity)
        {
            ref var camera = ref entity.Get<Camera>();
            ref var position = ref entity.Get<PositionComponent>();
            ref var velocity = ref entity.Get<VelocityComponent>();
            Vector2 targetPos = Vector2.Lerp(position.Position + velocity.Velocity * VelocityCamera, camera.Position, 0.55f);

            foreach (var bound in _cameraBounds[0]) {
                int xDiff = (int)targetPos.X/16 - bound.X;
                int yDiff = (int)targetPos.Y/16 - bound.Y;
                // if (xDiff == 0 && yDiff == 0) 
                targetPos.X = Math.Max(targetPos.X, (bound.X) * 16); 
            }

            foreach (var bound in _cameraBounds[1]) {
                int xDiff = (int)(targetPos.X + camera.Viewport.Width)/16 - bound.X;
                int yDiff = (int)(targetPos.Y + camera.Viewport.Height)/16 - bound.Y;
                // if (xDiff == 0 && yDiff == 0)
                 targetPos.X = Math.Min(targetPos.X, (bound.X) * 16); 
            }

            foreach (var bound in _cameraBounds[2]) {
                int xDiff = (int)(targetPos.X + camera.Viewport.Width)/16 - bound.X;
                int yDiff = (int)(targetPos.Y + camera.Viewport.Height)/16 - bound.Y;
                // if (xDiff == 0 && yDiff == 0)
                 targetPos.Y = Math.Min(targetPos.Y, (bound.Y) * 16); 
            }
            camera.Position = targetPos;
            // camera.CenterOrigin();
            base.Update(state, in entity);
        }

        private void LoadBounds(TiledMapTileLayer layer)
        {
            foreach (var tile in layer.Tiles)
            {
                switch (tile.GlobalIdentifier)
                {
                    case 76: _cameraBounds[0].Add((tile.X, tile.Y)); break;
                    case 74: _cameraBounds[1].Add((tile.X, tile.Y)); break;
                    case 6: _cameraBounds[2].Add((tile.X, tile.Y)); break;
                    case 4: _cameraBounds[3].Add((tile.X, tile.Y)); break;
                }
            }
        }
    }
}
