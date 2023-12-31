using System;
using System.Collections.Generic;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

using static PillowFight.Shared.Enums;

namespace PillowFight.Shared.Components
{
    internal struct ModifiableComponent<T> where T : struct
    {
        public T Base = new();
        public T Modified = new();
        public ModifiableComponent() { }

        public ModifiableComponent(T bass)
        {
            Base = bass;
            Modified = bass;
        }
    }

    internal struct TimedActions
    {
        public List<Action<Entity>> Actions = new List<Action<Entity>>();
        public List<float> Times = new List<float>();
        public TimedActions() { }
        public void Add(Action<Entity> action, float time)
        {
            Actions.Add(action);
            Times.Add(time);
        }
    }

    internal struct PositionComponent
    {
        
        public int X
        {
            get => Hitbox.X;
            set => Hitbox.X = value;
        }

        public int Y
        {
            get => Hitbox.Y;
            set => Hitbox.Y = value;
        }
        public float XRemainder = 0;
        public float YRemainder = 0;

        public Vector2 Position
        {
            get => new Vector2(X, Y);

            set => Hitbox.Location = value.ToPoint();
        }

        public Rectangle Hitbox;

        public int Top => Hitbox.Y;
        public int Bottom => Hitbox.Y + Hitbox.Height;
        public int Left => Hitbox.X;
        public int Right => Hitbox.X + Hitbox.Width;
        public Vector2 Center => new Vector2(Right / 2f, Bottom / 2f);
        public Vector2 TopCenter => new Vector2((Left + Right)/2f, Top);
        public Vector2 BottomCenter => new Vector2((Left + Right)/2f, Bottom);
        public Vector2 LeftCenter => new Vector2(Left, (Top + Bottom)/2f);
        public Vector2 RightCenter => new Vector2(Right, (Top + Bottom)/2f);
        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        public PositionComponent(Rectangle hitbox, Vector2 velocity)
        {
            Hitbox = hitbox;
        }

        public PositionComponent(Rectangle position)
            : this(position, Vector2.Zero) 
        { }
    }

    internal struct VelocityComponent
    {
        public float X;
        public float Y;

        public int HDirection => X < 0 ? -1 : 1;
        public int VDirection => Y < 0 ? -1 : 1;

        public Vector2 Velocity
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public VelocityComponent(Vector2 velocity)
        {
            Velocity = velocity;
        }

    }

    internal struct AccelerationComponent
    {
        public float X = 0;
        public float Y = 0;
        public int HDirection => X < 0 ? -1 : 1;
        public int VDirection => Y < 0 ? -1 : 1;

        public AccelerationComponent() { }
    }

    internal struct CollisionComponent
    {
        public float Mass = 1.0f;
        public bool DealImpulse = true;
        public bool ReceiveImpulse = true;

        public CollisionComponent() { }
        public CollisionComponent(float mass, bool dealImpulse, bool receiveImpulse)
        {
            Mass = mass;
            DealImpulse = dealImpulse;
            ReceiveImpulse = receiveImpulse;
        }
    }

    internal struct StageCollider
    {
        public bool Head = false;
        public bool LeftFoot = false;
        public bool RightFoot = false;
        public bool BottomLeftSide = false;
        public bool BottomRightSide = false;
        public bool TopLeftSide = false;
        public bool TopRightSide = false;

        public Vector2 HeadOffset = new Vector2(8, 0);
        public Vector2 LeftFootOffset = new Vector2(8, 0);
        public Vector2 RightFootOffset = new Vector2(8, 0);
        public Vector2 BottomLeftSideOffset = new Vector2(8, 0);
        public Vector2 BottomRightSideOffset = new Vector2(8, 0);
        public Vector2 TopLeftSideOffset = new Vector2(8, 0);
        public Vector2 TopRightSideOffset = new Vector2(8, 0);

        public StageCollider() { }
    }

    internal struct Colliders{
        public List<Entity> ColliderList = new();
        public Colliders() { }
        public Colliders(List<Entity> list)
        {
            ColliderList = list;
        }
    }

    internal struct ItemProperties
    {
        public bool Holdable = true;
        public bool Kickable = true;

        public ItemProperties()
        { }
    }

    internal struct ItemStatus
    {
        public bool Airborne;
        public bool Falling;
        public int Direction;
    }

    internal struct CharacterProperties
    { }

    internal struct CharacterStatus
    {
        public bool Ducking = true;
        public bool Jumping = true;
        public CharacterStatus() { }
    }

    internal struct HolderComponent
    {
        public Entity? Holding = null;
        public float ThrowImpulse = 8.0f;

        public HolderComponent() { }
    }

    internal struct HeldComponent
    {
        public Entity Holder;

        public HeldComponent(Entity holder)
        {
            Holder = holder;
        }
    }
    internal struct Holdable { }

    internal struct AbilityComponent
    {
        public bool Ability1 = false;   //Jump/Kick
        public bool Ability2 = false;   //Spawn/Pick and Throw pillow
        public bool Ability3 = false;
        public bool Ability4 = false;

        public AbilityComponent() { }
        public AbilityComponent(
            bool ability1,
            bool ability2,
            bool ability3,
            bool ability4
        )
        {
            Ability1 = ability1;
            Ability2 = ability2;
            Ability3 = ability3;
            Ability4 = ability4;

        }
    }

    internal struct ItemPhysics
    {
        public Vector2 UniversalAcceleration = Vector2.UnitY * -18.0f;

        public float Friction = 3.0f;
        public float AirFriction = 1.5f;
        public float XRestitution = 0.0f;
        public float YRestitution = 0.0f;

        public float MinXVelocity = .5f;
        public float MaxXVelocity = 100.0f;
        public float MaxYVelocity = 100.0f;

        public ItemPhysics() { }

        public ItemPhysics(Vector2 acceleration, float friction, float airFriction, float minXVelocity,
            float xRestitution, float yRestitution)
        {
            UniversalAcceleration = acceleration;
            Friction = friction;
            AirFriction = airFriction;
            MinXVelocity = minXVelocity;
            XRestitution = xRestitution;
            YRestitution = yRestitution;
        }
    }

    internal struct CharacterPhysics
    {
        public float RunVelocity = 4.0f;
        // public float AirRunVelocity = 2.5f;
        public float JumpVelocity = 10.0f;
        public float RunAcceleration = 35.0f;
        public float AirRunAcceleration = 35.0f;

        public float JumpGravityMultiplier = 0.55f;

        public float KickImpulse = 12.0f;


        public CharacterPhysics() { }
        public CharacterPhysics(
            float runVelocity,
            // float airRunVelocity,
            float jumpVelocity,
            float runAcceleration
        )
        {
            RunVelocity = runVelocity;
            // AirRunVelocity = airRunVelocity;
            JumpVelocity = jumpVelocity;
            RunAcceleration = runAcceleration;
        }
    }

    internal struct ControlKeys
    {
        public Keys LeftKey = Keys.A;
        public Keys RightKey = Keys.D;
        public Keys UpKey = Keys.W;
        public Keys DownKey = Keys.S;

        public Keys Ability1Key = Keys.J;
        public Keys Ability2Key = Keys.K;
        public Keys Ability3Key = Keys.L;
        public Keys Ability4Key = Keys.I;

        public ControlKeys() { }
    }

    internal struct InputComponent
    {
        public KeyboardState CurrentState = new();
        public KeyboardState PreviousState = new();

        public bool WasKeyUp(Keys key) => CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
        public bool WasKeyDown(Keys key) => CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
        public InputComponent() { }
    }
    internal struct PlayerInputSource
    {
        public Func<KeyboardState> InputSource = () => new KeyboardState();
        public PlayerInputSource(Func<KeyboardState> inputSource)
        {
            InputSource = inputSource;
        }
    }

    internal struct AIComponent
    {
        public int AILevel = 0;
        public float LastThrowTime = 0.0f;
        public float PlayerDistance = 240.0f;
        public float PlayerDistanceTolerance = 32.0f;
        public float ThrowDelay = 4.0f;
        public AIComponent() { }
    }

    internal struct PillowComponent
    {
        public PillowState State = PillowState.Stationery;
        public Vector2 Velocity = Vector2.Zero;
        public float ExplosionThreshold = 10.0f;
        public float ExplosionDamage = 3.0f;
        public float ProjectileThreshold = 10.0f;
        public float ExplosionImpulse = 5.0f;

        public float DecayVelocityThreshold = 1.0f;
        public float DecayTimer = 3.0f;
        public float DecayTime = 3.0f;

        public Action<Entity> OnExplode = null;
        public PillowComponent() { }
    }
    internal struct DamageComponent
    {
        public readonly float Damage;
        public Color Color = Color.White;

        public DamageComponent(float damage, Color color)
        {
            Damage = damage;
            Color = color;
        }
        
    }

    internal struct RenderModifier
    {
        public Color Color = Color.White;
        public float Flicker = .2f;
        public float FlickerTimer = 0.0f;
        public bool OnCycle => FlickerTimer > Flicker;

        public RenderModifier() { }
        public RenderModifier(Color color, float flicker)
        {
            Color = color;
            Flicker = flicker;
        }

        public void Update(float dt)
        {
            if (FlickerTimer > 2 * Flicker) FlickerTimer = 0;
            FlickerTimer += dt;
        }
    }

    internal readonly struct ImpulseComponent
    {
        public readonly Vector2 Impulse;
        public readonly bool overrideVel;
        // public readonly List<Vector2> Impulses = new();
        public ImpulseComponent(Vector2 impulse, bool over = false)
        {
            Impulse = impulse;
            overrideVel = over;
        }
    }


    internal struct HealthComponent
    {
        public float Health = 1;
        public float DamageMultiplier = 1.0f;
        public Action<Entity> OnDamage = null;
        public Action<Entity> OnDeath = null;
        public RenderModifier DamageModifier = new RenderModifier(Color.Red, .3f);
        public RenderModifier DeathModifier = new RenderModifier(Color.Maroon, .2f);
        public bool IsDead => Health <= 0;

        public HealthComponent() { }
        public HealthComponent(float health)
        {
            Health = health;
        }
        public HealthComponent(float health, Action<Entity> onDamage, Action<Entity> onDeath)
        {
            Health = health;
            OnDamage = onDamage;
            OnDeath = onDeath;
        }
    }

    // internal struct 
    internal struct CameraComponent
    {
        public OrthographicCamera Camera;
        public GameWindow Window;
        public CameraComponent(OrthographicCamera camera, GameWindow window)
        {
            Camera = camera;
            Window = window;
        }
    }

    internal struct DebugComponent { }
}