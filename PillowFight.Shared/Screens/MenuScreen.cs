using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MLEM.Font;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;
using MonoGame.Extended.Screens;

namespace PillowFight.Shared.Screens
{
    internal class MenuScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private UiSystem ui;

        public MenuScreen(Game game) : base(game)
        {
            ui = Game.Ui;

            var butt = new Button(Anchor.AutoLeft, new Vector2(100, 50), "Hi", "PlayGame") {OnPressed = element => Game.LoadScreen(new GameplayScreen(Game))};
            var panel = new Panel(Anchor.CenterLeft, size: new Vector2(200, 300), positionOffset: Vector2.Zero);
            panel.AddChild(butt);
            panel.AddChild(new Button(Anchor.AutoLeft, new Vector2(100, 50), "exit") { OnPressed = e => Game.Exit() });

            ui.Add("pan", panel);
            // ui.Remove("pan");
        }

        public override void Update(GameTime gameTime)
        {
            ui.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // ui.DrawEarly(gameTime, Game.SpriteBatch);
            // ui.Draw(gameTime, this.Game.SpriteBatch);
        }
    }
}
