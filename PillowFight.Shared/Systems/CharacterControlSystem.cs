using System;
using DefaultEcs;
using DefaultEcs.System;
using MonoGame.Extended.Sprites;
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
                .With<AnimatedSprite>()
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
            ref var sprite = ref entity.Get<AnimatedSprite>();

            if (input.CurrentState.IsKeyDown(keys.LeftKey))
            {
                status.Direction = -1;
                bool shouldAccelerate = velocity.X > -characterPhysics.Modified.RunVelocity;

                if (status.Airborne)
                {
                    // sprite.Play("jump");
                    if (shouldAccelerate)
                        velocity.X = Math.Max(-characterPhysics.Modified.RunVelocity,
                            velocity.X - characterPhysics.Modified.AirRunAcceleration * deltaTime);
                }
                else
                {
                    if (velocity.X * acceleration.X < 0)
                        sprite.Play("turn");
                    else 
                        sprite.Play("run");

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
                        sprite.Play("turn");
                    else 
                        sprite.Play("run");

                    if (shouldAccelerate)
                        velocity.X = Math.Min(characterPhysics.Modified.RunVelocity,
                            velocity.X + characterPhysics.Modified.RunAcceleration * deltaTime);
                }
            }
            else
            {
                sprite.Play("stand");
            }

            if (status.Airborne) sprite.Play("jump");

        } 
    }
}
