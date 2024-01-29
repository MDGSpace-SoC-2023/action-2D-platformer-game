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
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private EntityRenderSystem _entityRenderSystem;
		private HUDHealthSystem _hudHealthSystem;
		private DebugSystem _debugSystem;
        private SequentialSystem<float> _mainSystem;

        private new Game1 Game => (Game1)base.Game;

        private World _world;
        public Entity player;
        public AnimatedSprite playerSprite;
        public GameplayScreen(Game game) : base(game, true, true, true)
        { 
            _world = new World();
            _entityRenderSystem = new EntityRenderSystem(_world, Game.SpriteBatch, Game.GraphicsDevice, Game1.Camera);
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
                new MoveSystem(_world, () => _tiledMap),
				new GhostMoveSystem(_world),
				new HoldingSystem(_world),
                new AnimationUpdateSystem(_world),
                new CameraSystem(_world),
                new DamageSystem(_world),
				new DespawnSystem(_world),
				new KillSystem(_world)
            );

            player = _world.CreateEntity();
			Composer.CreateItem(player, new Rectangle(64, 64, 32, 32));
			Composer.CreateCharacter(player);
			Composer.CreatePlayer(player, 0);
            
            player.Set(new AbilityComponent(true, true, true, true));
            player.Set(new PlayerInputSource(Keyboard.GetState));
            
            player.Set(new DebugComponent());
            player.Set(new AsepriteSprite {sprites = new AnimatedSprite[] {
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Stand"), 
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Jump"), 
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Die"),
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Turn"), 
				Assets.Aseprites["Mario"].CreateAnimatedSprite("Run") 
			}});

			var platform = _world.CreateEntity();
			Composer.CreateItem(platform, new Rectangle (128, 256, 32, 32));
			Composer.CreateFloatingPlatform(platform);
			var anim = Assets.Aseprites["Cloud"].CreateAnimatedSprite("Cloud");
			anim.Play(0);
			platform.Set(new AsepriteSprite() { sprites = new AnimatedSprite[] { anim } });

            Load();
        }

        public void Load()
        {
            _tiledMap = Content.Load<TiledMap>("Maps/Demo");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        }

        public override void Update(GameTime gameTime)
        {
            _mainSystem.Update(gameTime.GetElapsedSeconds());
            _tiledMapRenderer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
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
			_debugSystem.Update(gameTime.GetElapsedSeconds());
        }
    }
}
