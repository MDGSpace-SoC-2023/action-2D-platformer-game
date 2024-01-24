﻿using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using PillowFight.Shared.Components;
using System;

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
            // ref var colliders = ref entity.Get<Colliders>();
            ref var charPhy = ref entity.Get<ModifiableComponent<CharacterPhysics>>();
            ref var holder = ref entity.Get<HolderComponent>();
            ref var position = ref entity.Get<PositionComponent>();

            if (input.CurrentState.IsKeyDown(keys.Ability1Key)) {
                if (holder.Holding == null) {
                    Entity? collider = null;
                    ReadOnlySpan<Entity> holdables = World.GetEntities().With<Holdable>().AsSet().GetEntities();
                    foreach (var holdable in holdables) {
                        Vector2 distance = holdable.Get<PositionComponent>().Position - entity.Get<PositionComponent>().Position;
                        if (distance.Length() < ability.PickRadius) {
                            collider = holdable;
                            break;
                        }                        
                    }                    

                    Entity pillow = World.CreateEntity();
                    Helper.CreatePillow(pillow);
                    
                    pillow.Set(new HeldComponent(entity));
                    holder.Holding = pillow;
                    pillow.Set(new PositionComponent(new Rectangle(position.X, position.Y - 32, 32, 32)));
                }
            } else {
                if (holder.Holding != null) {
                    var pillow  = holder.Holding.Value;
                    bool left = input.CurrentState.IsKeyDown(keys.LeftKey);
                    bool right = input.CurrentState.IsKeyDown(keys.RightKey);
                    bool up = input.CurrentState.IsKeyDown(keys.UpKey);
                    bool down = input.CurrentState.IsKeyDown(keys.DownKey);


                    float throwXDirection = left ? -1 : right ? 1 : (up || down) ? 0 : item.Direction;
                    float throwYDirection = up ? 1 : (down && (left || right)) ? -.5f : 0;
                    Vector2 impulse = new Vector2(throwXDirection, throwYDirection);
                    if (impulse != Vector2.Zero) impulse.Normalize();
                    impulse *= holder.ThrowImpulse;

                    pillow.Remove<HeldComponent>();
                    pillow.Set(new VelocityComponent(down && !(left || right) ? entity.Get<VelocityComponent>().Velocity : Vector2.Zero));
                    pillow.Set(new ImpulseComponent(impulse));
                    entity.Set(new ImpulseComponent(-impulse));

                    holder.Holding = null;
                }
            }

            if (input.WasKeyUp(keys.Ability2Key)) {
                if (item.Airborne) {
                    ReadOnlySpan<Entity> kickables = World.GetEntities().With<Kickable>().AsSet().GetEntities();
                    foreach (var kickable in kickables) {
                        Vector2 distance = kickable.Get<PositionComponent>().Position - entity.Get<PositionComponent>().Position;
                        Vector2 impulse;
                        if (distance.Length() < ability.KickRadius) {
                            if (kickable.Get<ItemStatus>().Airborne) {
                                bool left = input.CurrentState.IsKeyDown(keys.LeftKey);
                                bool right = input.CurrentState.IsKeyDown(keys.RightKey);
                                bool up = input.CurrentState.IsKeyDown(keys.UpKey);
                                bool down = input.CurrentState.IsKeyDown(keys.DownKey);

                                impulse = new Vector2(left ? -1 : 1, up ? 1 : down ? -1 : 0).NormalizedCopy() * charPhy.Modified.KickImpulse;
                            } else {
                                impulse = Vector2.UnitX * charPhy.Modified.KickImpulse * entity.Get<ItemStatus>().Direction;
                            }
                            kickable.Set(new ImpulseComponent(impulse, 0.5f));
                            entity.Set(new ImpulseComponent(-impulse, 0.5f));
                        }
                    }
                } else {
                    ref var vel = ref entity.Get<VelocityComponent>(); 
                    vel.Y = charPhy.Modified.JumpVelocity;
                }
            } else {
                if (!item.Falling && input.WasKeyDown(keys.Ability2Key)) entity.Get<VelocityComponent>().Y /= 2;
            }
        }

        private Entity SpawnPillow(bool freeze = false, bool floating = false)
        {

            Entity pillow = World.CreateEntity();
            if (freeze) {
                pillow.Set(new PillowComponent() {
                    State = Enums.PillowState.Held,
                    OnExplode = e =>
                        {
                            e.Disable<VelocityComponent>();
                            e.Get<TimedActions>().Add(e => e.Enable<VelocityComponent>(), 3);
                        }
                });
                pillow.Set(new RenderModifier(Color.Blue, 0));
            } else {
                pillow.Set(new PillowComponent() {
                    State = Enums.PillowState.Held
                });
                pillow.Set(new RenderModifier(Color.White, 0));
            }
            return pillow;
        }
    }
}
