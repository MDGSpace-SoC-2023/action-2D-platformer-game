using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace PillowFight.Shared.Screens
{
	internal class PauseScreen : BaseScreen
	{
		private Button[] _buttons;
		public PauseScreen(Game1 game) : base(game, false, false, true)
		{
			_buttons = new Button[2];
			_buttons[0] = new Button(Assets.Images["Button"], Assets.Images["ButtonPress"], new Vector2(100, 10), () => Game.ActiveScreen = Game.GameplayScreen, "Resume");
			_buttons[1] = new Button(Assets.Images["Button"], Assets.Images["ButtonPress"], new Vector2(100, 50), () => Game.ActiveScreen = Game.MenuScreen, "Main");
		}

		public override void DrawHUD(GameTime gameTime)
		{
			Game.SpriteBatch.Draw(Assets.Images["Box"], Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
			Button.DrawAll(_buttons, Game.SpriteBatch);
		}

		public override void DrawMap()
		{ }

		public override void DrawSprites(float deltaTime)
		{ }

		public override void Update(GameTime gameTime)
		{
			if (Game.Input.WasKeyUp(Keys.Up)) Button.MoveFocus(_buttons, true);
			if (Game.Input.WasKeyUp(Keys.Down)) Button.MoveFocus(_buttons, false);
			Button.UpdateAll(_buttons, gameTime.GetElapsedSeconds(), Game.Input.WasKeyUp(Keys.Enter));
 		}
	}
}
