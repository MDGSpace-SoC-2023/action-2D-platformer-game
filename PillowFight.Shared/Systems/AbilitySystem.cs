using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class AbilitySystem : AEntitySetSystem<float>
    {
        public AbilitySystem(World world)
            : base(world.GetEntities().With<AbilityComponent>().With<InputComponent>().With<ControlKeys>().With<ModifiableComponent<CharacterPhysics>>().AsSet()) { }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var item = ref entity.Get<ItemStatus>();
            ref var input = ref entity.Get<InputComponent>();
            ref var keys = ref entity.Get<ControlKeys>();
            ref var ability = ref entity.Get<AbilityComponent>();
            ref var colliders = ref entity.Get<Colliders>();
            ref var charPhy = ref entity.Get<ModifiableComponent<CharacterPhysics>>();
            ref var holder = ref entity.Get<HolderComponent>();

            ref var position = ref entity.Get<PositionComponent>();
            if (holder.Holding.HasValue)
            {
                holder.Holding.Value.Get<PositionComponent>().Position = new Vector2(position.X, position.Y - 32);
            }

            // if (input.CurrentState.IsKeyDown(keys.Ability2Key))
            if (input.CurrentState.IsKeyDown(keys.Ability1Key))
            {

                //Todo: Implementation looks kinda jank

                if (holder.Holding == null)
                {
                    Entity? collider = null;

                    if (colliders.ColliderList.Count != 0)
                    {
                        foreach (var c in colliders.ColliderList)
                        {
                            // if (c.Has<Holdable>())
                            if (c.Get<ItemProperties>().Holdable)
                            {
                                collider = c;
                                // c.Get<Holdable>().Holder = entity;
                                break;
                            }
                        }
                    }

                    Entity pillow = collider ?? SpawnPillow(input.CurrentState.IsKeyDown(keys.Ability3Key), input.CurrentState.IsKeyDown(keys.Ability4Key));
                    
                    holder.Holding = pillow;
                    pillow.Set(new PositionComponent(new Rectangle(position.X, position.Y - 32, 32, 32)));
                    // pillow.Get<PillowComponent>().State = Enums.PillowState.Held;
                    pillow.Disable<ItemProperties>();
                    pillow.Disable<ItemStatus>();
                    pillow.Disable<VelocityComponent>();
                    // pillow.Disable<AccelerationComponent>();
                    pillow.Disable<ModifiableComponent<ItemPhysics>>();
                    pillow.Disable<CollisionComponent>();
                    pillow.Disable<Colliders>();
                    pillow.Disable<HolderComponent>();
                    
                }
            }
            else
            {
                if (holder.Holding != null)
                {
                    var pillow  = holder.Holding.Value;
                    bool left = input.CurrentState.IsKeyDown(keys.LeftKey);
                    bool right = input.CurrentState.IsKeyDown(keys.RightKey);
                    bool up = input.CurrentState.IsKeyDown(keys.UpKey);
                    bool down = input.CurrentState.IsKeyDown(keys.DownKey);

                    pillow.Enable<ItemProperties>();
                    pillow.Enable<ItemStatus>();
                    pillow.Enable<VelocityComponent>();
                    pillow.Set(new VelocityComponent(down && !(left || right) ? entity.Get<VelocityComponent>().Velocity : Vector2.Zero));
                    // pillow.Enable<AccelerationComponent>();
                    pillow.Enable<ModifiableComponent<ItemPhysics>>();
                    // pillow.Get<TimedActions>().Add(e => e.Enable<ModifiableComponent<ItemPhysics>>(), 2);
                    pillow.Get<PillowComponent>().State = Enums.PillowState.Stationery;
                    pillow.Enable<CollisionComponent>();
                    pillow.Enable<Colliders>();
                    pillow.Enable<HolderComponent>();
                    colliders.ColliderList.Add(pillow);

                    float throwXDirection = left ? -1 : right ? 1 : (up || down) ? 0 : item.Direction;
                    float throwYDirection = up ? 1 : (down && (left || right)) ? -.5f : 0;
                    Vector2 impulse = new Vector2(throwXDirection, throwYDirection);
                    if (impulse != Vector2.Zero) impulse.Normalize();
                    impulse *= holder.ThrowImpulse;

                    holder.Holding.Value.Set(new ImpulseComponent(impulse));
                    entity.Set(new ImpulseComponent(-impulse));

                    // pillow.Get<Holdable>().Holder = null;
                    holder.Holding = null;
                }
            }

            if (input.WasKeyUp(keys.Ability2Key))
            {
                if (item.Airborne)
                {
                    if (colliders.ColliderList.Count != 0)
                    {
                        Entity? collider = null;
                        foreach (var c in colliders.ColliderList)
                        {
                            if (c.Get<ItemProperties>().Kickable)
                            {
                                collider = c;
                                break;
                            }
                        }
                        if (collider != null)
                        {
                            bool left = input.CurrentState.IsKeyDown(keys.LeftKey);
                            bool right = input.CurrentState.IsKeyDown(keys.RightKey);
                            bool up = input.CurrentState.IsKeyDown(keys.UpKey);
                            bool down = input.CurrentState.IsKeyDown(keys.DownKey);

                            if (!collider.Value.Get<ItemStatus>().Airborne)
                            {
                                collider.Value.Set(new ImpulseComponent(Vector2.UnitX * charPhy.Modified.KickImpulse * entity.Get<ItemStatus>().Direction, true));
                                collider.Value.Get<HolderComponent>().Holding = entity;
                                // collider.Value.Get<PillowComponent>().State = Enums.PillowState.Running;
                            }
                            else
                            {
                                Vector2 impulse = new Vector2(left ? -1 : 1, up ? 1 : down ? -1 : 0).NormalizedCopy() * charPhy.Modified.KickImpulse;

                                collider.Value.Set(new ImpulseComponent(impulse, true));
                                entity.Set(new ImpulseComponent(-impulse));
                            }
                        }
                    }
                }
                else
                {
                    ref var vel = ref entity.Get<VelocityComponent>(); 
                    vel.Y = charPhy.Modified.JumpVelocity;
                }
            }
            else
            {
                if (!item.Falling && input.WasKeyDown(keys.Ability2Key)) entity.Get<VelocityComponent>().Y /= 2;
            }

            if (input.WasKeyUp(keys.Ability3Key))
            {
                // SpawnPillow(true);
            }

        }

        private Entity SpawnPillow(bool freeze = false, bool floating = false)
        {
            var anim = new AnimatedSprite(Assets.SpriteSheets["Cloud"]);
            // anim.Play("run");

            Entity pillow = World.CreateEntity();
            pillow.Set(anim);

            pillow.Set(new ItemProperties());
            pillow.Set(new ItemStatus());
            pillow.Set(new VelocityComponent());
            // pillow.Set(new VelocityComponent(down && !(left || right) ? entity.Get<VelocityComponent>().Velocity : Vector2.Zero));
            if (!floating) pillow.Set(new AccelerationComponent());
            pillow.Set(new ModifiableComponent<ItemPhysics>(new ItemPhysics(){XRestitution = .7f, YRestitution = .5f}));
            pillow.Set(new CollisionComponent());
            pillow.Set(new Colliders());
            // pillow.Set(new Colliders(new List<Entity>{entity}));
            pillow.Set(new HolderComponent());
            pillow.Set(new StageCollider());
            pillow.Set(new TimedActions());
            pillow.Set(new HealthComponent());

            if (freeze)
            {
                pillow.Set(new PillowComponent()
                {
                    State = Enums.PillowState.Held,
                    OnExplode = e =>
                        {
                            e.Disable<VelocityComponent>();
                            e.Get<TimedActions>().Add(e => e.Enable<VelocityComponent>(), 3);
                        }
                });
                pillow.Set(new RenderModifier(Color.Blue, 0));
            }
            else
            {
                pillow.Set(new PillowComponent()
                {
                    State = Enums.PillowState.Held
                });
                pillow.Set(new RenderModifier(Color.White, 0));


            }
            // pillow.Set(new Holdable());

            
            return pillow;
        }
    }
}
