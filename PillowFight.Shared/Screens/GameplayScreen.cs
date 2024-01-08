using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using PillowFight.Shared.Components;
using PillowFight.Shared.Systems;
using MonoGame.Extended.Sprites;

namespace PillowFight.Shared.Screens
{
    internal class GameplayScreen : GameScreen
    {
        public TiledMap _tiledMap;
        public TiledMap TiledMap => _tiledMap;

        public TiledMapRenderer _tiledMapRenderer;
        private RenderSystem _renderSystem;
        private SequentialSystem<float> _mainSystem;

        private new Game1 Game => (Game1)base.Game;

        private World _world;
        public Entity player;
        public AnimatedSprite playerSprite;
        public GameplayScreen(Game game) : base(game)
        { 
            _world = new World();
            _renderSystem = new RenderSystem(_world, Game.SpriteBatch, Game.GraphicsDevice);
            _mainSystem = new SequentialSystem<float>(

                new TimedActionsSystem(_world),
                new ImpulseSystem(_world),
                new PillowSystem(_world),
                new DamageSystem(_world),
                new PlayerInputSystem(_world),
                new AIInputSystem(_world),
                new CollisionSystem(_world),
                new AbilitySystem(_world),
                new CharacterControlSystem(_world),
                new PhysicsSystem(_world),
                new MoveSystem(_world, () => _tiledMap),
                new AnimationUpdateSystem(_world),
                // new DamageSystem(_world),
                new CameraSystem(_world)
            );
            playerSprite = new AnimatedSprite(Assets.SpriteSheets["Mario3"]);

            player = _world.CreateEntity();
            player.Set(playerSprite);
            player.Set(new ItemProperties());
            player.Set(new ItemStatus());
            player.Set(new CharacterProperties());
            player.Set(new CharacterStatus());
            player.Set(new PositionComponent(new Rectangle(64, 64, 32, 32)));
            player.Set(new VelocityComponent());
            player.Set(new AccelerationComponent());
            player.Set(new ModifiableComponent<ItemPhysics>(new ItemPhysics()));
            player.Set(new ModifiableComponent<CharacterPhysics>(new CharacterPhysics()));
            player.Set(new CollisionComponent());
            player.Set(new Colliders());
            player.Set(new StageCollider());
            player.Set(new HolderComponent());
            player.Set(new HealthComponent(10));
            player.Set(new AbilityComponent(true, true, true, true));
            player.Set(new TimedActions());
            player.Set(new ControlKeys());
            player.Set(new InputComponent());
            player.Set(new PlayerInputSource(Keyboard.GetState));
            player.Set(new RenderModifier(Color.White, 0));
            // player.Set(new CameraComponent(Game.Camera, Game.Window));
            player.Set(new DebugComponent());

            // var enemy = _world.CreateEntity();
            // enemy.Set(new AnimatedSprite(Assets.SpriteSheets["Luigi3"]) );
            // enemy.Set(new ItemProperties());
            // enemy.Set(new ItemStatus());
            // enemy.Set(new CharacterProperties());
            // enemy.Set(new CharacterStatus());
            // enemy.Set(new PositionComponent(new Rectangle(240, 64, 32, 32)));
            // enemy.Set(new VelocityComponent());
            // enemy.Set(new AccelerationComponent());
            // enemy.Set(new ModifiableComponent<ItemPhysics>(new ItemPhysics()));
            // enemy.Set(new ModifiableComponent<CharacterPhysics>(new CharacterPhysics()));
            // enemy.Set(new CollisionComponent());
            // enemy.Set(new Colliders());
            // enemy.Set(new StageCollider());
            // enemy.Set(new TimedActions());
            // enemy.Set(new HolderComponent());
            // enemy.Set(new HealthComponent(10));
            // enemy.Set(new AbilityComponent(true, true, true, true));
            // enemy.Set(new ControlKeys());
            // enemy.Set(new InputComponent());
            // enemy.Set(new AIComponent());
            // enemy.Set(new RenderModifier(Color.White, 0));

            Load();
        }

        public void Load()
        {
            _tiledMap = Content.Load<TiledMap>("Maps/DemoMap");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        }

        public override void Update(GameTime gameTime)
        {
            _mainSystem.Update(gameTime.GetElapsedSeconds());
            _tiledMapRenderer.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            _tiledMapRenderer.Draw();
            // _tiledMapRenderer.Draw(Game.Camera.GetViewMatrix());
            _renderSystem.Update(gameTime.GetElapsedSeconds());
        }
    }
}
