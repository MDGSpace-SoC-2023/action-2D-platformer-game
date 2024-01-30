using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite.Sprites;
using MonoGame.Extended;
// using Myra.Graphics2D.UI;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Screens
{
    internal class MenuScreen : BaseScreen
	{
		// private new Game1 Game => (Game1)base.Game;

		private World _world;
		private AnimatedSprite[] _player;
		// private Desktop _desktop;
		private float _timer;
		private float _duration = 2;
		private int _index;
		// private int _padding = 4;
		private Texture2D panel;
		private Color[] _colors = new Color[] {
			Color.Aqua, Color.Green, Color.Orange, Color.Red, Color.Gray
		};
		private Button[] _buttons;

		public MenuScreen(Game game) : base(game, true, true, true)
		{
			// _desktop = new();
			// _desktop.Root = Assets.UIProjects["Menu"].Root;
			panel = Assets.Images["Box"];
			_buttons = new Button[3];
			_buttons[0] = new Button(Assets.Images["Button"], Assets.Images["ButtonPress"], new Vector2(10, 10), () => Game.ActiveScreen = Game.GameplayScreen = new GameplayScreen(Game, 3), "Play");
			_buttons[1] = new Button(Assets.Images["Button"],Assets.Images["ButtonPress"] , new Vector2(10, 50), () => Game.ActiveScreen = Game.GameplayScreen, "Resume");
			_buttons[2] = new Button(Assets.Images["Button"],Assets.Images["ButtonPress"] , new Vector2(10, 100), () => Game.Exit(), "Exit");

			_world = new World();
			var input = _world.CreateEntity();
			input.Set(new PlayerInputSource(Keyboard.GetState));
			input.Set(new InputComponent());
			
            _player = new AnimatedSprite[] {
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Stand"), 
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Jump"), 
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Die"),
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Turn"), 
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Run") 
			};

			foreach (var sprite in _player) {
				sprite.Play();
				sprite.Scale = new Vector2(2,2);
			}
			

			_player = new AnimatedSprite[] {
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Stand"),
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Jump"),
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Die"),
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Turn"),
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Run")
			};

			foreach (var sprite in _player)
			{
				// sprite.Play();
				sprite.Scale = new Vector2(3, 3);
			}

			_player[0].Play();
			_player[1].Play();
			_player[2].Play();
			_player[3].Play();
			_player[4].Play(startingFrame: 1);
			// _player[4].Update(0.01);

			// _desktop.Widgets.this
		}

		public override void Update(GameTime gameTime)
		{
			_timer += gameTime.GetElapsedSeconds();
			if (_timer > _duration)
			{
				_timer = 0;
				_index++;
				_index %= 5;
			}
			if (Game.Input.WasKeyUp(Keys.Up)) Button.MoveFocus(_buttons, true);
			if (Game.Input.WasKeyUp(Keys.Down)) Button.MoveFocus(_buttons, false);
			Button.UpdateAll(_buttons, gameTime.GetElapsedSeconds(),Game.Input.WasKeyUp(Keys.Enter));
		}

		public void Draw(GameTime gameTime)
		{ }

		public override void DrawMap()
		{
			Game.GraphicsDevice.Clear(ClearOptions.Target, _colors[_index], 1.0f, 0);
		}

		public override void DrawSprites(float deltaTime)
		{
			float ratio = _timer / _duration;
			for (int i = 0; i < 21; i++)
			{
				for (int j = 0; j < 12; j++)
				{
					_player[_index].Draw(Game.SpriteBatch, new Vector2((i + ratio - 1) * 48, (j + ratio - 1) * 48));
				}
			}
		}

		public override void DrawHUD(GameTime gameTime)
		{
			Game.SpriteBatch.Draw(panel, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
			Button.DrawAll(_buttons, Game.SpriteBatch);
			// _desktop.Render();
		}
	}
}
