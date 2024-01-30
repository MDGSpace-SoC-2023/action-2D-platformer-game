using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace PillowFight.Shared.Screens
{
    internal abstract class BaseScreen
    {
		private Game _game;
		protected Game1 Game => (Game1) _game;
        public readonly bool CanDrawMap;
        public readonly bool CanDrawSprites;
        public readonly bool CanDrawHUD;
        public BaseScreen(Game game, bool drawMap, bool drawSprite, bool drawHUD)
        {
			_game = game;
            CanDrawMap = drawMap;
            CanDrawSprites = drawSprite;
            CanDrawHUD = drawHUD;
        }

        
		public abstract void Update(GameTime gameTime);
        public abstract void DrawMap();
        public abstract void DrawSprites(float deltaTime);
        public abstract void DrawHUD(GameTime gameTime);
    }
}
