using System;
using System.Collections.Generic;
using System.Text;
using DefaultEcs;

namespace PillowFight.Shared
{
    internal class Helper
    {
        public static Entity CreatePillow(World world)
        {
            var pillow = world.CreateEntity();
            return pillow;
        }
    }
}
