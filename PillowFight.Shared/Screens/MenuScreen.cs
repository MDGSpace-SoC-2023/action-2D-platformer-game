using DefaultEcs;
using Microsoft.Xna.Framework;
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
		private int _index;

		public MenuScreen(Game game) : base(game, true, true, true)
		{
			_desktop = new();
			_desktop.Root = Assets.UIProjects["Menu"].Root;

			_world = new World();
			
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
			
			// _desktop.Widgets.this
		}

		public override void Update(GameTime gameTime)
		{
			_timer += gameTime.GetElapsedSeconds();
			if (_timer > 2) {
				_timer = 0;
				_index++;
				_index %= 5;
			}
		}

		public override void Draw(GameTime gameTime)
		{ }

		public override void DrawMap() { }

		public override void DrawSprites(float deltaTime) {
			for (int i = 0; i < 20; i++) {
				for (int j = 0; j < 12; j++) {
                    _player[_index].Draw(Game.SpriteBatch, new Vector2(i*32, j*32));
				}
			}
		}

		public override void DrawHUD(GameTime gameTime)
		{
			// _desktop.Render();
		}
	}
}
