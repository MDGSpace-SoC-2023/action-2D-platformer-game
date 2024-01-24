using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace PillowFight.Shared.Screens
{
    internal class MenuScreen : BaseScreen
    {
        private new Game1 Game => (Game1)base.Game;

		private Desktop _desktop;

        public MenuScreen(Game game) : base(game, false, false, true)
        {
			_desktop = new();
			_desktop.Root = Assets.UIProjects["Menu"].Root;
			// _desktop.Widgets.this
        }

        public override void Update(GameTime gameTime)
        {}

        public override void Draw(GameTime gameTime)
        {}

		public override void DrawMap() {}

		public override void DrawSprites(float deltaTime) {}

		public override void DrawHUD(GameTime gameTime) {
			_desktop.Render();
		}
    }
}
