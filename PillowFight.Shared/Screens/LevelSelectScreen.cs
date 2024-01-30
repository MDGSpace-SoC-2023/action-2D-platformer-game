using Microsoft.Xna.Framework;

namespace PillowFight.Shared.Screens {
    internal class LevelSelectScreen : BaseScreen
    {
        public LevelSelectScreen(Game game) : base(game, false, false, true)
        {
        }

        public override void DrawHUD(GameTime gameTime)
        {
			Game.SpriteBatch.Draw(Assets.Images["Box"], Vector2.Zero, Color.White);
		}


        public override void DrawMap()
        {}

        public override void DrawSprites(float deltaTime)
        {}

        public override void Update(GameTime gameTime)
        {}
    }
}
