using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace PillowFight.Shared.Screens
{
    internal abstract class BaseScreen : GameScreen
    {
        public readonly bool CanDrawMap;
        public readonly bool CanDrawSprites;
        public readonly bool CanDrawHUD;
        public BaseScreen(Game game, bool drawMap, bool drawSprite, bool drawHUD) : base(game)
        {
            CanDrawMap = drawMap;
            CanDrawSprites = drawSprite;
            CanDrawHUD = drawHUD;
        }

        

        public abstract void DrawMap();
        public abstract void DrawSprites(float deltaTime);
        public abstract void DrawHUD(GameTime gameTime);
    }
}
