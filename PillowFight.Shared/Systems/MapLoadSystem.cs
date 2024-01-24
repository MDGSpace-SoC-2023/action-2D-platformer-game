using DefaultEcs;
using DefaultEcs.System;
using MonoGame.Extended.Tiled;

namespace PillowFight.Shared.Systems {
	internal class MapLoadSystem : AComponentSystem<float, Camera> {
		
		private TiledMap _map;
		private int _xCache = -1;
		private int _yCache = -1;
		
		public MapLoadSystem(World world, TiledMap map) : base(world) {
			_map = map;
		}

		protected override void Update(float state, ref Camera component) {
			int x = (int)(component.Position.X / 32);
			int y = (int)(component.Position.Y / 32);

			if (x != _xCache && y != _yCache) {
				_xCache = x;
				_yCache = y;
				
				int width = component.Viewport.Width / 32;
				int height = component.Viewport.Height / 32;
				
				
			}

			
		}
	}
}
