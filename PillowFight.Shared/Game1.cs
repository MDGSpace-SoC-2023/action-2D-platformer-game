using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled.Renderers;
using Myra;
using Myra.Graphics2D.UI;
using PillowFight.Shared.Screens;

namespace PillowFight.Shared
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		public SpriteBatch SpriteBatch;

		public TiledMapRenderer TiledMapRenderer;
		public static Camera Camera;
		public RenderTarget2D _backgroundTarget;
		public RenderTarget2D _entityTarget;
		private RenderTarget2D _hudTarget;

		private readonly ScreenManager _screenManager;
		private BaseScreen _activeScreen;

		private Desktop _desktop;
		// public static float Scale { get; private set; }
		// public static int BarHeight { get; private set; }
		// public static int BarWidth { get; private set; }
		// public int presentWidth;
		// public int presentHeight;
		// private float outputAspect;
		// private float preferredAspect;
		private bool _isResizing;

		//  Screen size
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int ViewWidth { get; private set; }
		public int ViewHeight { get; private set; }

		//  Screen scale matrix
		public Matrix ScreenScaleMatrix { get; private set; }

		//  Screen Viewport
		public Viewport Viewport { get; private set; }

		//  View padding, amount to apply for letter/pillar boxing
		private int _viewPadding;
		public int ViewPadding
		{
			get => _viewPadding;
			set
			{
				//  Only perform view update if the value is changed
				if (_viewPadding != value)
				{
					_viewPadding = value;
					UpdateView();
				}
			}
		}

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			// Graphics.IsFullScreen = true;

			_graphics.PreferredBackBufferWidth = Enums.Screen.WidthDefault;
			_graphics.PreferredBackBufferHeight = Enums.Screen.HeightDefault;
			Window.AllowUserResizing = true;
			// Window.IsBorderless = true;

			_graphics.DeviceCreated += OnGraphicsDeviceCreated;
			_graphics.DeviceReset += OnGraphicsDeviceReset;
			Window.ClientSizeChanged += OnWindowSizeChanged;

			_graphics.ApplyChanges();

			_screenManager = new ScreenManager();
			Components.Add(_screenManager);
		}



		private void OnGraphicsDeviceCreated(object sender, EventArgs e)
		{
			//  When graphics device is created, call UpdateView to recalculate the screen scale matrix
			UpdateView();
		}

		private void OnGraphicsDeviceReset(object sender, EventArgs e)
		{
			//  When graphics device is reset, call UpdateView to recalculate the screen scale matrix
			UpdateView();
		}

		private void OnWindowSizeChanged(object sender, EventArgs e)
		{
			//  Window size changing is a little different, we only want to call UpdateView when it's finished resizing
			//  for instance, if the user clicks and drags the window border, during each size change we don't want
			//  to call UpdateView, so we use the _isResizing flag
			if (Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0 && !_isResizing)
			{
				_isResizing = true;

				//  Set the backbuffer width and height to the window bounds
				_graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
				_graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

				//  Now update the view
				UpdateView();

				_isResizing = false;
			}
		}

		private void UpdateView()
		{
			float screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
			float screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

			// get View Size
			if (screenWidth / Width > screenHeight / Height)
			{
				ViewWidth = (int)(screenHeight / Height * Width);
				ViewHeight = (int)screenHeight;
			}
			else
			{
				ViewWidth = (int)screenWidth;
				ViewHeight = (int)(screenWidth / Width * Height);
			}

			// apply View Padding
			var aspect = ViewHeight / (float)ViewWidth;
			ViewWidth -= ViewPadding * 2;
			ViewHeight -= (int)(aspect * ViewPadding * 2);

			// update screen matrix
			ScreenScaleMatrix = Matrix.CreateScale(ViewWidth / (float)Width);

			// update viewport
			Viewport = new Viewport
			{
				X = (int)(screenWidth / 2 - ViewWidth / 2),
				Y = (int)(screenHeight / 2 - ViewHeight / 2),
				Width = ViewWidth,
				Height = ViewHeight,
				MinDepth = 0,
				MaxDepth = 1
			};
		}

		protected override void Initialize()
		{
			MyraEnvironment.Game = this;

			AssetLoader ass = new();
			ass.Load(Content, GraphicsDevice);

			SpriteBatch = new SpriteBatch(GraphicsDevice);
			_backgroundTarget = new RenderTarget2D(GraphicsDevice, Enums.Screen.Width, Enums.Screen.Height);
			_entityTarget = new RenderTarget2D(GraphicsDevice, Enums.Screen.Width, Enums.Screen.Height);
			_hudTarget = new RenderTarget2D(GraphicsDevice, Enums.Screen.Width, Enums.Screen.Height);


			// Camera = new Camera(Viewport);
			Camera = new Camera(GraphicsDevice.Viewport);
			Camera.CenterOrigin();

			_activeScreen = new GameplayScreen(this, 1);
			// _activeScreen = new MenuScreen(this);
			_screenManager.LoadScreen(_activeScreen);
			// _screenManager.LoadScreen(new MenuScreen(this));


			base.Initialize();
		}

		protected override void LoadContent()
		{ }

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (_activeScreen.CanDrawMap)
			{
				DrawStart(_backgroundTarget);
				_activeScreen.DrawMap();
				SpriteBatch.End();
			}

			if (_activeScreen.CanDrawSprites)
			{
				DrawStart(_entityTarget);
				_activeScreen.DrawSprites(gameTime.GetElapsedSeconds());
				SpriteBatch.End();
			}

			if (_activeScreen.CanDrawHUD)
			{
				DrawStart(_hudTarget);
				_activeScreen.DrawHUD(gameTime);
				DrawEnd(gameTime);
			}
		}

		public void LoadScreen(Screen screen)
		{
			_screenManager.LoadScreen(screen);
		}

		public void DrawStart(RenderTarget2D target)
		{
			this.GraphicsDevice.SetRenderTarget(target);
			this.GraphicsDevice.Clear(Color.Transparent);
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
			// outputAspect = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;
			// // outputAspect = GraphicsDevice.Adapter.CurrentDisplayMode.Width / (float)GraphicsDevice.Adapter.CurrentDisplayMode.Height;
			// preferredAspect = Enums.Screen.Width / (float)Enums.Screen.Height;
			// BarHeight = 0;
			// BarWidth = 0;
			// Rectangle dst;

			// if (outputAspect <= preferredAspect)
			// {
			//     // bars on top/bottom
			//     presentHeight = (int)(this.Window.ClientBounds.Width / preferredAspect);
			//     BarHeight = (this.Window.ClientBounds.Height - presentHeight) / 2;
			//     dst = new Rectangle(0, BarHeight, this.Window.ClientBounds.Width, presentHeight);
			//     Scale = 1f / ((float)Enums.Screen.Width / this.Window.ClientBounds.Width);
			// }
			// else
			// {
			//     // bars left/right
			//     presentWidth = (int)(this.Window.ClientBounds.Height * preferredAspect);
			//     BarWidth = (this.Window.ClientBounds.Width - presentWidth) / 2;
			//     dst = new Rectangle(BarWidth, 0, presentWidth, this.Window.ClientBounds.Height);
			//     Scale = 1f / ((float)Enums.Screen.Height / this.Window.ClientBounds.Height);
			// }

			this.GraphicsDevice.SetRenderTarget(null);
			this._graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
			// GraphicsDevice.Viewport = Viewport;

			this.SpriteBatch.Begin(
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				SamplerState.PointClamp
				// ,transformMatrix: ScreenScaleMatrix
				);
			SpriteBatch.Draw(_backgroundTarget, Vector2.Zero, Color.White);
			SpriteBatch.Draw(_entityTarget, Vector2.Zero, Color.White);
			SpriteBatch.Draw(_hudTarget, Vector2.Zero, Color.White);
			this.SpriteBatch.End();

			// Assets.Effects["HUDHealthShader"].Parameters["Height"].SetValue(0.25f);
			// this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp
			// , effect:Assets.Effects["HUDHealthShader"]
			// );
			// Assets.Effects["HUDHealthShader"].CurrentTechnique.Passes[0].Apply();
			// this.SpriteBatch.End();
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
