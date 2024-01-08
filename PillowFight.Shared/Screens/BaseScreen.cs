using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;

namespace PillowFight.Shared.Screens
{
    internal abstract class BaseScreen : GameScreen
    {
        public delegate void HandleKey(Keys key);

        public event HandleKey HandleA;
        
        public BaseScreen(Game game) : base(game)
        {
        }
    }
}
