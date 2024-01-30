using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using PillowFight.Shared.Components;
using PillowFight.Shared.Systems;
using MonoGame.Aseprite.Sprites;

namespace PillowFight.Shared.Screens
{
    internal class GameplayScreen : BaseScreen
    {
        private TiledMap _map;
        private TiledMapRenderer _tiledMapRenderer;
        private AsepriteRenderer _entityRenderSystem;
		private HUDHealthSystem _hudHealthSystem;
		private DebugSystem _debugSystem;
        private SequentialSystem<float> _mainSystem;

        // private new Game1 Game => (Game1)base.Game;

        private World _world;
        public Entity player;
        public GameplayScreen(Game game, int level) : base(game, true, true, true)
        { 
			_map = Assets.Maps["Lv1"];
			_tiledMapRenderer = new TiledMapRenderer(Game.GraphicsDevice, _map);
			
            _world = new World();
            _entityRenderSystem = new AsepriteRenderer(_world, Game.SpriteBatch, Game.GraphicsDevice, Game1.Camera);
			_hudHealthSystem = new HUDHealthSystem(_world, Game.GraphicsDevice, Game.SpriteBatch, Game._entityTarget);
			_debugSystem = new DebugSystem(_world, Game.SpriteBatch, Game.GraphicsDevice);
            _mainSystem = new SequentialSystem<float>(

				//Input first
                new PlayerInputSystem(_world),
                new AIInputSystem(_world),
                new AbilitySystem(_world),

				// new MapLoadSystem(_world, _tiledMap),
                new TimedActionsSystem(_world),
				new ValueGeneratorUpdater(_world),
                new ImpulseSystem(_world),
				new OnHold(_world),
				new OnThrow(_world),
                // new CollisionSystem(_world),
				new PillowSystem(_world),
                new CharacterControlSystem(_world),
                new PhysicsSystem(_world),
                new MoveSystem(_world, _map),
				new GhostMoveSystem(_world),
				new HoldingSystem(_world),
                new AnimationUpdateSystem(_world),
                new CameraSystem(_world, _map),
                new DamageSystem(_world),
				new DespawnSystem(_world),
				new KillSystem(_world)
            );

            player = _world.CreateEntity();
			Composer.CreateItem(player, new Rectangle(64, 64, 16, 16));
			Composer.CreateCharacter(player);
			Composer.CreatePlayer(player, 0);
            
            player.Set(new AbilityComponent(true, true, true, true));
            player.Set(new PlayerInputSource(Keyboard.GetState));
            
            _world.Set(new GameState());
            player.Set(new DebugComponent());
            player.Set(new AsepriteSprite {sprites = new AnimatedSprite[] {
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Stand"), 
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Jump"), 
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Die"),
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Turn"), 
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Run") 
			}});

            for (int i = 0; i < level; i++) {
                var ai = _world.CreateEntity();
                Composer.CreateEnemy(ai,0);
            }
   //          Composer.CreateItem(ai, new Rectangle(128, 128, 16, 16));
   //          Composer.CreateCharacter(ai);
   //          ai.Set(new AIComponent());
   //          ai.Set(new HealthHUD(1) { Scale = new Vector2(2, 2), Position = new Vector2(600, 0)});
   //          ai.Set(new AsepriteSprite {sprites = new AnimatedSprite[] {
			// 	Assets.Aseprites["Mario"].CreateAnimatedSprite("Stand"), 
			// 	Assets.Aseprites["Mario"].CreateAnimatedSprite("Jump"), 
			// 	Assets.Aseprites["Mario"].CreateAnimatedSprite("Die"),
			// 	Assets.Aseprites["Mario"].CreateAnimatedSprite("Turn"), 
			// 	Assets.Aseprites["Mario"].CreateAnimatedSprite("Run") 
			// }});
			// var platform = _world.CreateEntity();
			// Composer.CreateItem(platform, new Rectangle (128, 256, 32, 32));
			// Composer.CreateFloatingPlatform(platform, new Rectangle(128, 256, 16, 16) );
			// var anim = Assets.Aseprites["Cloud"].CreateAnimatedSprite("Cloud");
			// anim.Play(0);
			// platform.Set(new AsepriteSprite() { sprites = new AnimatedSprite[] { anim } });
            // platform.Set(Assets.Images["Cloud"]);

            Load();
        }

        public void Load()
        {
            // _tiledMap = Content.Load<TiledMap>("Maps/Demo");
            // _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.Input.WasKeyUp(Keys.Escape)) {
                Game.ActiveScreen = Game.PauseScreen;
            }
            _mainSystem.Update(gameTime.GetElapsedSeconds());
            _tiledMapRenderer.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {}

        public override void DrawMap()
        {
			_tiledMapRenderer.Draw(Game1.Camera.TransformationMatrix);
        }

        public override void DrawSprites(float gameTime)
        {
			_entityRenderSystem.Update(gameTime);
        }

        public override void DrawHUD(GameTime gameTime)
        {
			_hudHealthSystem.Update(gameTime.GetElapsedSeconds());
			// _debugSystem.Update(gameTime.GetElapsedSeconds());
        }
    }
}
