using DefaultEcs;
using Microsoft.Xna.Framework;
<<<<<<< HEAD
=======
using Microsoft.Xna.Framework.Graphics;
>>>>>>> UI
using MonoGame.Aseprite.Sprites;
using MonoGame.Extended;
using Myra.Graphics2D.UI;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Screens
{
	internal class MenuScreen : BaseScreen
	{
		private new Game1 Game => (Game1)base.Game;

		private World _world;
		private AnimatedSprite[] _player;
		private Desktop _desktop;
		private float _timer;
<<<<<<< HEAD
		private int _index;
=======
		private float _duration = 2;
		private int _index;
		private int _padding = 4;
		private Color[] _colors = new Color[] {
			Color.Aqua, Color.Green, Color.Orange, Color.Red, Color.Gray
		};
>>>>>>> UI

		public MenuScreen(Game game) : base(game, true, true, true)
		{
			_desktop = new();
			_desktop.Root = Assets.UIProjects["Menu"].Root;

			_world = new World();
<<<<<<< HEAD
			
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
			
=======

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

>>>>>>> UI
			// _desktop.Widgets.this
		}

		public override void Update(GameTime gameTime)
		{
			_timer += gameTime.GetElapsedSeconds();
<<<<<<< HEAD
			if (_timer > 2) {
=======
			if (_timer > _duration)
			{
>>>>>>> UI
				_timer = 0;
				_index++;
				_index %= 5;
			}
		}

		public override void Draw(GameTime gameTime)
		{ }

		public override void DrawMap()
		{
			GraphicsDevice.Clear(ClearOptions.Target, _colors[_index], 1.0f, 0);
		}

<<<<<<< HEAD
		public override void DrawSprites(float deltaTime) {
			for (int i = 0; i < 20; i++) {
				for (int j = 0; j < 12; j++) {
                    _player[_index].Draw(Game.SpriteBatch, new Vector2(i*32, j*32));
=======
		public override void DrawSprites(float deltaTime)
		{
			float ratio = _timer / _duration;
			for (int i = 0; i < 21; i++)
			{
				for (int j = 0; j < 12; j++)
				{
					_player[_index].Draw(Game.SpriteBatch, new Vector2((i + ratio - 1) * 48, (j + ratio - 1) * 48));
>>>>>>> UI
				}
			}
		}

		public override void DrawHUD(GameTime gameTime)
		{
			// _desktop.Render();
		}
	}
}
