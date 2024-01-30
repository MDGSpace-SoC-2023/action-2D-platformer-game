using System;
using System.Collections.Generic;
using DefaultEcs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite.Sprites;

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

        // public int Top => Hitbox.Y;
        // public int Bottom => Hitbox.Y + Hitbox.Height;
        // public int Left => Hitbox.X;
        // public int Right => Hitbox.X + Hitbox.Width;
        // public Vector2 Center => new Vector2(Right / 2f, Bottom / 2f);
        // public Vector2 TopCenter => new Vector2((Left + Right)/2f, Top);
        // public Vector2 BottomCenter => new Vector2((Left + Right)/2f, Bottom);
        // public Vector2 LeftCenter => new Vector2(Left, (Top + Bottom)/2f);
        // public Vector2 RightCenter => new Vector2(Right, (Top + Bottom)/2f);
        // public Vector2 TopLeft => new Vector2(Left, Top);
        // public Vector2 TopRight => new Vector2(Right, Top);
        // public Vector2 BottomLeft => new Vector2(Left, Bottom);
        // public Vector2 BottomRight => new Vector2(Right, Bottom);

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

    internal struct CollisionIgnore
    {
        public List<Entity> IgnoreEntity = new();
        public EntityQueryBuilder CollideType = null;
        public EntityQueryBuilder IgnoreType = null;
        public CollisionIgnore()
        {
        }
    }

    internal struct SolidCollider
    {
        public bool Top = false;
        public bool Bottom = false;
        public bool Left = false;
        public bool Right = false;

        public bool Colliding => Top || Bottom || Left || Right;

        public Vector2[] TopColliders = new Vector2[] { new Vector2(8, 0) };
        public Vector2[] BottomColliders = new Vector2[] { new Vector2(1, 16), new Vector2(15, 16) };
        public Vector2[] LeftColliders = new Vector2[] {
            new Vector2(0, 8)
            // , new Vector2(0, 24)
        };
        public Vector2[] RightColliders = new Vector2[] {
            new Vector2(16, 8)
            // , new Vector2(32, 24) 
        };

        public SolidCollider() { }

        public SolidCollider(Vector2[] top, Vector2[] bottom, Vector2[] left, Vector2[] right)
        {
            TopColliders = top;
            BottomColliders = bottom;
            LeftColliders = left;
            RightColliders = right;
        }
    }

    internal struct StageCollider
    {
        public int Top;
        public int Bottom;
        public int Left;
        public int Right;

        public bool Colliding => Top != 0 && Bottom != 0 && Left != 0 && Right != 0;
    }

    internal struct ItemCollider
    {
        public Entity? Top;
        public Entity? Bottom;
        public Entity? Left;
        public Entity? Right;

        public bool Colliding => Top.HasValue && Bottom.HasValue && Left.HasValue && Right.HasValue;
    }

    internal struct Solid
    {
        public Rectangle Hitbox = new Rectangle(0, 0, 32, 32);

        public Solid()
        { }
    }

    internal struct Colliders
    {
        public List<Entity> ColliderList = new();
        public Colliders() { }
        public Colliders(List<Entity> list)
        {
            ColliderList = list;
        }
    }

    internal struct Hardbody
    {
        public Rectangle Hitbox;
    }

    internal struct Holdable
    {
        public Action<Entity> OnHold;
        public Action<Entity> OnThrow;
        public Entity HolderCache;
    }

    internal struct Kickable { }

    internal struct ItemProperties
    {
        public bool Holdable = true;
        public bool Kickable = true;

        public ItemProperties()
        { }
    }

    internal struct ItemStatus
    {
        public bool Airborne = true;
        public bool Falling = true;
        public int Direction = 1;

        public ItemStatus()
        {
        }
    }

    internal struct CharacterProperties { }

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
        public Vector2 HoldPosition = Vector2.Zero;

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

    internal struct AbilityComponent
    {
        public bool Ability1 = false;   //Jump/Kick
        public bool Ability2 = false;   //Spawn/Pick and Throw pillow
        public bool Ability3 = false;
        public bool Ability4 = false;

        public float KickRadius = 64.0f;
        public float PickRadius = 64.0f;
        public float ThrowImpulse = 12.0f;
        public float KickImpulse = 12.0f;

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
        public Vector2 UniversalAcceleration = Vector2.UnitY * -14.0f;

        public float Friction = 3.0f;
        public float AirFriction = 1.5f;
        public float XRestitution = 0.0f;
        public float YRestitution = 0.0f;

        public float MinXVelocity = .5f;
        public float MaxXVelocity = 20.0f;
        public float MaxYVelocity = 20.0f;

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
        public float RunVelocity = 2.5f;
        // public float AirRunVelocity = 2.5f;
        public float JumpVelocity = 5.0f;
        public float RunAcceleration = 35.0f;
        public float AirRunAcceleration = 35.0f;

        public float JumpGravityMultiplier = 0.5f;

        public float KickImpulse = 7.0f;


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
        public Keys Left = Keys.A;
        public Keys Right = Keys.D;
        public Keys Up = Keys.W;
        public Keys Down = Keys.S;

        public Keys Ability1 = Keys.J;
        public Keys Ability2 = Keys.K;
        public Keys Ability3 = Keys.L;
        public Keys Ability4 = Keys.I;

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
        public float StageExplosionRadius = 32.0f;
        public float CharacterExplosionRadius = 32.0f;
        public float DecayVelocityThreshold = 1.0f;
        public float PreDecayTimer = 5.0f;
        public float PreDecayTime = 5.0f;
        public float DecayTimer = 3.0f;
        public float DecayTime = 3.0f;

        public float ProjectileSpeed = 12.0f;
        public Action<Entity> OnExplode = null;
        public PillowComponent() { }
    }

    internal struct DamageComponent
    {
        public float Damage;
        public Color Color = Color.Green;

        public DamageComponent() { }

        public DamageComponent(float damage, Color color)
        {
            Damage = damage;
            Color = color;
        }
    }

    internal struct RenderModifier
    {
        public Color OnColor = Color.White;
        public Color OffColor = Color.Green;
        public float Flicker = 0;
        public float FlickerTimer = 0;
        public bool OnCycle => FlickerTimer >= Flicker;
        public Color Color => FlickerTimer >= Flicker ? OnColor : OffColor;
        public bool ShouldCycle => Flicker > 0;

        public RenderModifier() { }

        public RenderModifier(Color onColor, float flicker)
        {
            OnColor = onColor;
            Flicker = flicker;
        }

        public RenderModifier(Color onColor, float flicker, Color offColor)
        {
            OnColor = onColor;
            Flicker = flicker;
            OffColor = offColor;
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
        public readonly float VelocityRatio;
        // public readonly List<Vector2> Impulses = new();
        public ImpulseComponent(Vector2 impulse, float velocityRatio = 1)
        {
            Impulse = impulse;
            VelocityRatio = velocityRatio;
        }
    }


    internal struct HealthComponent
    {
        public float Health = 1;
        public float MaxHealth = 1;
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
            MaxHealth = health;
        }
    }

    internal struct HealthHUD
    {
        public readonly int Index;
        public Vector2 Scale = new Vector2(1, 1);
        public Vector2 Position = new Vector2(0, 0);
        public HealthHUD(int index)
        {
            Index = index;
        }
    }

    internal struct DebugComponent { }

    internal readonly struct GameOverConditions
    {
        public readonly Func<bool>[] Checks;
        public GameOverConditions(Func<bool>[] checks)
        {
            Checks = checks;
        }
    }

    internal struct LevelCompleteConditions
    {
        public Func<bool>[] Checks;
        public LevelCompleteConditions(Func<bool>[] checks)
        {
            Checks = checks;
        }
    }

    internal struct AsepriteSprite
    {
        public AnimatedSprite[] sprites;
        public int Index;
        public AnimatedSprite Sprite => sprites[Index];
        public void Play(int i)
        {
            Index = Math.Min(i, sprites.Length);
            sprites[i].Play(startingFrame: 0);
        }
    }

    internal struct KillComponent { }

    internal struct GameState { }

    internal struct NoOffscreenDespawn { }

    internal struct SineGenerator
    {
        public float Frequency = 1.0f;
        public float Amplitude = 1.0f;
        public float Time = 0;
        public Action<Entity, float> Target;

        public SineGenerator()
        { }

        public SineGenerator(float freq, float amp)
        {
            Frequency = freq;
            Amplitude = amp;
        }

        public float Value => Amplitude * MathF.Cos(Time / Frequency);
    }

    internal struct Follower { }
}
