using System;
using System.Collections.Generic;
using System.Text;
using DefaultEcs;
using DefaultEcs.System;
using PillowFight.Shared.Components;

namespace PillowFight.Shared.Systems
{
    internal class TimedActionsSystem : AEntitySetSystem<float>
    {
        public TimedActionsSystem(World world) : base(world.GetEntities().With<TimedActions>().AsSet()) { }

        protected override void Update(float deltaTime, in Entity entity)
        {
            ref var actions = ref entity.Get<TimedActions>();
            if (actions.Actions.Count != 0)
            {
                for (var i = 0; i < actions.Times.Count; i++)
                {
                    if (actions.Times[i] < 0)
                    {
                        actions.Actions[i].Invoke(entity);
                        actions.Times.RemoveAt(i);
                        actions.Actions.RemoveAt(i);
                    }
                    else
                    {
                        actions.Times[i] -= deltaTime;
                    }
                }
            }
            base.Update(deltaTime, in entity);
        }
    }
}
