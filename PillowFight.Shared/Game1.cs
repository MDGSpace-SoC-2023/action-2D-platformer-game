using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Misc;
using MLEM.Ui;
using MLEM.Ui.Style;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using PillowFight.Shared.Components;
using PillowFight.Shared.Screens;

namespace PillowFight.Shared
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        private RenderTarget2D _renderTarget;
        private RenderTarget2D _hudTarget;

        private readonly ScreenManager _screenManager;
        private GameplayScreen _gameplayScreen;

        public OrthographicCamera Camera;


        public UiSystem Ui;

        public static float Scale { get; private set; }
        public static int BarHeight { get; private set; }
        public static int BarWidth { get; private set; }
        public int presentWidth;
        public int presentHeight;
        private float outputAspect;
        private float preferredAspect;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            // Graphics.IsFullScreen = true;

            this.Graphics.PreferredBackBufferWidth = Enums.Screen.WidthDefault;
            this.Graphics.PreferredBackBufferHeight = Enums.Screen.HeightDefault;
            Window.AllowUserResizing = true;
            // Window.IsBorderless = true;

            Graphics.ApplyChanges();

            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        protected override void Initialize()
        {
            MlemPlatform.Current = new MlemPlatform.DesktopGl<TextInputEventArgs>((w, c) => w.TextInput += c);
            
            // var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 640, 480);
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1280, 720);
            Camera = new OrthographicCamera(viewportAdapter);

            AssetLoader ass = new();
            ass.Load(Content, GraphicsDevice);

            this.SpriteBatch = new SpriteBatch(GraphicsDevice);
            this._renderTarget = new RenderTarget2D(this.GraphicsDevice, Enums.Screen.Width, Enums.Screen.Height);
            this._hudTarget = new RenderTarget2D(this.GraphicsDevice, 128, 128*4);

            Ui = new UiSystem(this, new UntexturedStyle(SpriteBatch){Font = new GenericSpriteFont(Assets.Fonts["Arial"])});

            _gameplayScreen = new GameplayScreen(this);
            _screenManager.LoadScreen(_gameplayScreen);
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

            DrawStart();

            // SpriteBatch.DrawString(Assets.Fonts["Arial"], presentHeight.ToString(), new Vector2(40,40), Color.Green);
            // SpriteBatch.DrawString(Assets.Fonts["Arial"], presentWidth.ToString(), new Vector2(40,50), Color.Green);
            // SpriteBatch.DrawString(Assets.Fonts["Arial"], preferredAspect.ToString(), new Vector2(40,60), Color.Green);
            // SpriteBatch.DrawString(Assets.Fonts["Arial"], outputAspect.ToString(), new Vector2(40,70), Color.Green);
            // SpriteBatch.DrawString(Assets.Fonts["Arial"], BarHeight.ToString(), new Vector2(40,80), Color.Green);
            // SpriteBatch.DrawString(Assets.Fonts["Arial"], BarWidth.ToString(), new Vector2(40,90), Color.Green);
            // SpriteBatch.DrawString(Assets.Fonts["Arial"], Window.ClientBounds.Height.ToString(), new Vector2(40,100), Color.Green);
            // SpriteBatch.DrawString(Assets.Fonts["Arial"], Window.ClientBounds.Width.ToString(), new Vector2(40,110), Color.Green);
            // SpriteBatch.DrawString(Assets.Fonts["Arial"], GraphicsDevice.Viewport.Height.ToString(), new Vector2(40,120), Color.Green);
            // SpriteBatch.DrawString(Assets.Fonts["Arial"], GraphicsDevice.Viewport.Width.ToString(), new Vector2(40,130), Color.Green);

            base.Draw(gameTime);
            DrawEnd(gameTime);
        }

        public void LoadScreen(Screen screen)
        {
            _screenManager.LoadScreen(screen);
        }
        public void DrawStart()
        {
            this.GraphicsDevice.SetRenderTarget(this._renderTarget);
            this.GraphicsDevice.Clear(Color.Black);

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
            this.Graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 1.0f, 0);

            this.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp
                ,transformMatrix: Camera.GetViewMatrix()
                );
            this.SpriteBatch.Draw(texture: this._renderTarget, destinationRectangle: dst, color: Color.White);
            // _gameplayScreen._tiledMapRenderer.Draw();
            // this.SpriteBatch.Draw(texture: this._uiTarget, destinationRectangle: dst, color: Color.White);
            this.SpriteBatch.End();

            // Texture2D tex = new Texture2D()
            Assets.Effects["HUDHealthShader"].Parameters["Height"].SetValue(0.25f);
            // Assets.Effects["HUDHealthShader"].Parameters["Position"].SetValue(_gameplayScreen.player.Get<PositionComponent>().Y);

            this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp
                // , effect:Assets.Effects["HUDHealthShader"]
                );
            // this.SpriteBatch.Draw(_renderTarget, new Rectangle(Window.ClientBounds.Width - 128,128, 128, 128), _gameplayScreen.player.Get<PositionComponent>().Hitbox, Color.Green);

            Assets.Effects["HUDHealthShader"].CurrentTechnique.Passes[0].Apply();
            // this.SpriteBatch.Draw(_renderTarget, new Rectangle(Window.ClientBounds.Width - 128,0, 128, 128), _gameplayScreen.player.Get<PositionComponent>().Hitbox, Color.White);
            this.SpriteBatch.Draw(_gameplayScreen.playerSprite.TextureRegion.Texture,new Rectangle(Window.ClientBounds.Width - 128,0, 128, 128), _gameplayScreen.playerSprite.TextureRegion.Bounds, Color.White);
            // this.SpriteBatch.Draw(_gameplayScreen.playerSprite.TextureRegion.Texture,new Rectangle(256,256, 128, 128), _gameplayScreen.playerSprite.TextureRegion.Bounds, Color.White);;
            this.SpriteBatch.End();
            // Ui.Draw(gameTime, SpriteBatch);

        }

        private void SpriteBatchStart(Effect? effect = null)
        {
            this.SpriteBatch.Begin(
                // transformMatrix: Camera.GetViewMatrix(),
                transformMatrix: null,
                effect: effect);
        }
    }
}
