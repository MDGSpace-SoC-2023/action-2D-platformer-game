using System;
using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class CharacterControlSystem : AEntitySetSystem<float>
    {
        public CharacterControlSystem(World world) 
            : base(world.GetEntities()
                .With<ControlKeys>()
                .With<InputComponent>()
                .With<VelocityComponent>()
                .With<AccelerationComponent>()
                .With<ModifiableComponent<ItemPhysics>>()
                .With<ModifiableComponent<CharacterPhysics>>()
                .With<ItemStatus>()
                .AsSet())
        {
        }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var keys = ref entity.Get<ControlKeys>();
            ref var input = ref entity.Get<InputComponent>();
            ref var status = ref entity.Get<ItemStatus>();
            ref var velocity = ref entity.Get<VelocityComponent>();
            ref var acceleration = ref entity.Get<AccelerationComponent>();
            ref var itemPhysics = ref entity.Get<ModifiableComponent<ItemPhysics>>();
            ref var characterPhysics = ref entity.Get<ModifiableComponent<CharacterPhysics>>();
            ref var sprite = ref entity.Get<AsepriteSprite>();

            if (input.CurrentState.IsKeyDown(keys.LeftKey))
            {
                status.Direction = -1;
                bool shouldAccelerate = velocity.X > -characterPhysics.Modified.RunVelocity;

                if (status.Airborne)
                {
                    sprite.Play(1);
                    if (shouldAccelerate)
                        velocity.X = Math.Max(-characterPhysics.Modified.RunVelocity,
                            velocity.X - characterPhysics.Modified.AirRunAcceleration * deltaTime);
                }
                else
                {
                    if (velocity.X * acceleration.X < 0)
                        sprite.Play(3);
                    else 
                        sprite.Play(4);

                    if (shouldAccelerate)
                        velocity.X = Math.Max(-characterPhysics.Modified.RunVelocity,
                            velocity.X - characterPhysics.Modified.RunAcceleration * deltaTime);
                }
            }
            else if (input.CurrentState.IsKeyDown(keys.RightKey))
            {
                status.Direction = 1;
                bool shouldAccelerate = velocity.X < characterPhysics.Modified.RunVelocity;

                if (status.Airborne)
                {
                    // sprite.Play("jump");

                    if (shouldAccelerate)
                        velocity.X = Math.Min(characterPhysics.Modified.RunVelocity,
                            velocity.X + characterPhysics.Modified.AirRunAcceleration * deltaTime);
                }
                else
                {
                    if (velocity.X * acceleration.X < 0)
                        sprite.Play(3);
                    else 
                        sprite.Play(4);

                    if (shouldAccelerate)
                        velocity.X = Math.Min(characterPhysics.Modified.RunVelocity,
                            velocity.X + characterPhysics.Modified.RunAcceleration * deltaTime);
                }
            }
            else
            {
                sprite.Play(0);
            }

            if (input.CurrentState.IsKeyDown(keys.Ability2Key) && status.Falling) {
                itemPhysics.Modified.UniversalAcceleration.Y = itemPhysics.Base.UniversalAcceleration.Y * characterPhysics.Modified.JumpGravityMultiplier;
            } else {
                itemPhysics.Modified.UniversalAcceleration.Y = itemPhysics.Base.UniversalAcceleration.Y;
            }
            if (status.Airborne) sprite.Play(1);

        } 
    }
}
