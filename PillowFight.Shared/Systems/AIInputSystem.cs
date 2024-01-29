using System;
using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;
using Microsoft.Xna.Framework.Input;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class AIInputSystem : AEntitySetSystem<float>
    {
        public AIInputSystem(World world, bool useBuffer = false)
            : base(world.GetEntities().With<AIComponent>().With<InputComponent>().AsSet()) { }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var ai = ref entity.Get<AIComponent>();
            ref var input = ref entity.Get<InputComponent>();

            var players = World.GetEntities().With<PlayerInputSource>().AsSet().GetEntities();
            var pillows = World.GetEntities().With<PillowComponent>().AsSet().GetEntities();

            input.PreviousState = input.CurrentState;
            input.CurrentState = ai.AILevel switch
            {
                1 => AI0(entity, players, pillows, deltaTime),
                _ => AI0(entity, players, pillows, deltaTime),
            };

            base.Update(deltaTime, in entity);
        }

        private KeyboardState AI0(Entity ai, ReadOnlySpan<Entity> players, ReadOnlySpan<Entity> pillows, float deltaTime)
        {
            ref var AI = ref ai.Get<AIComponent>();

            List<Keys> keys = new();

            if (players.Length > 0)
            {
                var player = players[0];
                float distance = player.Get<PositionComponent>().X - ai.Get<PositionComponent>().X;
                int sign = Math.Sign(distance);
                distance = Math.Abs(distance);

                if (Math.Abs(distance - AI.PlayerDistance) > AI.PlayerDistanceTolerance)
                {
                    if (sign > 0 && distance > AI.PlayerDistance || sign < 0 && distance < AI.PlayerDistance)
                    {
                        keys.Add(Keys.D);
                    }
                    else
                    {
                        keys.Add(Keys.A);
                    }
                }

                if (AI.LastThrowTime < 0)
                {
                    keys.Add(Keys.J);
                    // keys.Add(Keys.K);
                    keys.Add(sign == -1 ? Keys.A : Keys.D);
                    AI.LastThrowTime = AI.ThrowDelay;
                }
                else
                {
                    AI.LastThrowTime -= deltaTime;
                }
            }

            return new KeyboardState(keys.ToArray());
        }
    }
}
