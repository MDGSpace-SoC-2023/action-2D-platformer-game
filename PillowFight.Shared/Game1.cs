using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled.Renderers;
using Myra;
using Myra.Graphics2D.UI;
using PillowFight.Shared.Components;
using PillowFight.Shared.Screens;

namespace PillowFight.Shared
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		internal SpriteBatch SpriteBatch;

		public TiledMapRenderer TiledMapRenderer;
		public static Camera Camera;
		public RenderTarget2D _backgroundTarget;
		public RenderTarget2D _entityTarget;
		private RenderTarget2D _hudTarget;

		private EffectParameter LSDTime;

		private readonly ScreenManager _screenManager;
		internal BaseScreen ActiveScreen;
		internal GameplayScreen GameplayScreen;
		internal MenuScreen MenuScreen;
		internal LevelSelectScreen LevelSelectScreen;
		internal PauseScreen PauseScreen;

		private Desktop _desktop;


		public static float Scale { get; private set; }
		public static int BarHeight { get; private set; }
		public static int BarWidth { get; private set; }
		public int presentWidth;
		public int presentHeight;
		private float outputAspect;
		private float preferredAspect;

		internal InputComponent Input = new();

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			// IsMouseVisible = true;
			_graphics.IsFullScreen = true;

			_graphics.PreferredBackBufferWidth = Enums.Screen.WidthDefault;
			_graphics.PreferredBackBufferHeight = Enums.Screen.HeightDefault;
			Window.AllowUserResizing = true;
			// Window.IsBorderless = true;

			_graphics.ApplyChanges();

			_screenManager = new ScreenManager();
			// Components.Add(_screenManager);
		}

		protected override void Initialize()
		{
			MyraEnvironment.Game = this;

			AssetLoader ass = new();
			ass.Load(Content, GraphicsDevice);
			LSDTime = Assets.Effects["LSD"].Parameters["time"];

			SpriteBatch = new SpriteBatch(GraphicsDevice);
			_backgroundTarget = new RenderTarget2D(GraphicsDevice, Enums.Screen.Width, Enums.Screen.Height);
			_entityTarget = new RenderTarget2D(GraphicsDevice, Enums.Screen.Width, Enums.Screen.Height);
			_hudTarget = new RenderTarget2D(GraphicsDevice, Enums.Screen.Width, Enums.Screen.Height);


			// Camera = new Camera(Viewport);
			Camera = new Camera(GraphicsDevice.Viewport);
			Camera.CenterOrigin();

			GameplayScreen = new(this, 3);
			MenuScreen = new(this);
			PauseScreen = new(this);
			ActiveScreen = MenuScreen;


			base.Initialize();
		}

		protected override void LoadContent()
		{ }

		protected override void Update(GameTime gameTime)
		{
			// if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				// Exit();

			Input.PreviousState = Input.CurrentState;
			Input.CurrentState = Keyboard.GetState();
			ActiveScreen.Update(gameTime);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (ActiveScreen.CanDrawMap)
			{
				DrawStart(_backgroundTarget, Color.Transparent);
				ActiveScreen.DrawMap();
				SpriteBatch.End();
			}

			if (ActiveScreen.CanDrawSprites)
			{
				DrawStart(_entityTarget, Color.Transparent);
				ActiveScreen.DrawSprites(gameTime.GetElapsedSeconds());
				SpriteBatch.End();
			}

			if (ActiveScreen.CanDrawHUD)
			{
				DrawStart(_hudTarget, Color.Transparent);
				ActiveScreen.DrawHUD(gameTime);
				DrawEnd(gameTime);
			}
		}

		public void LoadScreen(Screen screen)
		{
			_screenManager.LoadScreen(screen);
		}

		public void DrawStart(RenderTarget2D target, Color color)
		{
			this.GraphicsDevice.SetRenderTarget(target);
			this.GraphicsDevice.Clear(color);
			this.SpriteBatchStart();
		}


		public void EffectStart(Effect effect = null)
		{
			this.SpriteBatch.End();
			this.SpriteBatchStart(effect: effect);
		}

		public void EffectEnd()
		{
			this.SpriteBatch.End();
			this.SpriteBatchStart();
		}

		// draw it to screen
		public void DrawEnd(GameTime gameTime)
		{
			this.SpriteBatch.End();

			// calculate Scale and bars
			outputAspect = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
			// outputAspect = GraphicsDevice.Adapter.CurrentDisplayMode.Width / (float)GraphicsDevice.Adapter.CurrentDisplayMode.Height;
			preferredAspect = Enums.Screen.Width / (float)Enums.Screen.Height;
			BarHeight = 0;
			BarWidth = 0;
			Rectangle dst;

			if (outputAspect <= preferredAspect)
			{
			    // bars on top/bottom
			    presentHeight = (int)(this.Window.ClientBounds.Width / preferredAspect);
			    BarHeight = (this.Window.ClientBounds.Height - presentHeight) / 2;
			    dst = new Rectangle(0, BarHeight, this.Window.ClientBounds.Width, presentHeight);
			    Scale = 1f / ((float)Enums.Screen.Width / this.Window.ClientBounds.Width);
			}
			else
			{
			    // bars left/right
			    presentWidth = (int)(this.Window.ClientBounds.Height * preferredAspect);
			    BarWidth = (this.Window.ClientBounds.Width - presentWidth) / 2;
			    dst = new Rectangle(BarWidth, 0, presentWidth, this.Window.ClientBounds.Height);
			    Scale = 1f / ((float)Enums.Screen.Height / this.Window.ClientBounds.Height);
			}

			this.GraphicsDevice.SetRenderTarget(null);
			this._graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
			LSDTime.SetValue((float)(gameTime.TotalGameTime.TotalMilliseconds/1000));

			this.SpriteBatch.Begin(
				SpriteSortMode.Immediate,
				BlendState.AlphaBlend,
				SamplerState.PointClamp
				);
			Assets.Effects["LSD"].CurrentTechnique.Passes[0].Apply();
			SpriteBatch.Draw(_backgroundTarget,dst,  Color.White);
			this.SpriteBatch.End();

			this.SpriteBatch.Begin(
				SpriteSortMode.Immediate,
				BlendState.AlphaBlend,
				SamplerState.PointClamp
				);
			SpriteBatch.Draw(_backgroundTarget,dst,  Color.White);
			// Assets.Effects["Whiteout"].CurrentTechnique.Passes[0].Apply();
			SpriteBatch.Draw(_entityTarget, dst,  Color.White);
			this.SpriteBatch.End();

			this.SpriteBatch.Begin(
				SpriteSortMode.Immediate,
				BlendState.AlphaBlend,
				SamplerState.PointClamp
				);
			SpriteBatch.Draw(_hudTarget, dst,  Color.White);
			this.SpriteBatch.End();
		}

		private void SpriteBatchStart(Effect? effect = null)
		{
			this.SpriteBatch.Begin(
				transformMatrix: null,
				samplerState: SamplerState.PointClamp,
				effect: effect);
		}
	}
}
