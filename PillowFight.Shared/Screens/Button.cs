using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
// using MonoGame.Extended.Tweening;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
// using MonoGame.Extended.Input;

namespace PillowFight.Shared.Screens
{
    internal class Button
    {
        private Texture2D texture;
        private Texture2D pressedTexture;
        // private Animation animation;
        private Vector2 position;
        private Vector2 originalPosition;
        private Rectangle rectangle;
        private Action onClick;

        private string text;

        // private Tweener tweener;
        private bool tweening;
        private bool tweenDirection;

        private bool focused;

        private bool pressed => focused && Keyboard.GetState().IsKeyDown(Keys.Enter);

        public Button(Texture2D texture, Texture2D pressedTexture, Vector2 position, Action onClick, string? text = null)
        {
            this.texture = texture;
            this.pressedTexture = pressedTexture == null ? texture : pressedTexture;
            this.position = position;
            this.onClick = onClick;
            this.text = text;
            this.rectangle = new Rectangle((int) position.X, (int) position.Y, texture.Width, texture.Height);
        }

        public void Update(float deltaTime, bool pressed)
        {
            if (pressed && focused) onClick();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pressed ? pressedTexture : texture, position,  focused ? Color.Wheat : Color.White);
            spriteBatch.DrawString(Assets.Fonts["Arial"], text, position + new Vector2(20, 6), Color.Black);
        }

        public static void DrawAll(Button[] buttons, SpriteBatch spriteBatch)
        {
            foreach (var butt in buttons)
            {
                butt.Draw(spriteBatch);
            }
        }

        public static void UpdateAll(Button[] buttons, float deltaTime, bool pressed)
        {
            foreach (var butt in buttons)
            {
                butt.Update(deltaTime, pressed);
            }

        }

        public static void MoveFocus(Button[] butts, bool reverse)
        {
            
            if (!anyFocused(butts)) butts[0].focused = true;
            else
            {
                int index = Array.IndexOf(butts, butts.First(butt => butt.focused));
                butts[index].focused = false;
                butts[(index + (reverse ? -1 : 1) + butts.Length) % butts.Length].focused = true;
            }
        }
        private static bool anyFocused(Button[] butts)
        {
            foreach (var butt in butts)
            {
                if (butt.focused) return true;
            }
            return false;
        }
    } 
}
