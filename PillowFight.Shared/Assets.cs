using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Content;

namespace PillowFight.Shared
{
    public static class Assets
    {
        public static Dictionary<string, Texture2D> Images = new();
        public static Dictionary<string, SpriteFont> Fonts = new();
        public static Dictionary<string, SpriteSheet> SpriteSheets = new();
        public static Dictionary<string, Effect> Effects = new();
    }

    public class AssetLoader
    {
        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Assets.Images["Mario"] = content.Load<Texture2D>("Sprites/Mario");
            Assets.Images["SMBTilesheet"] = content.Load<Texture2D>("Sprites/SMBTilesheet");
            Assets.Fonts["Arial"] = content.Load<SpriteFont>("Fonts/Arial");
            Assets.Images["Mario3Sheet"] = content.Load<Texture2D>("Sprites/PlumberFellasSpritesheet");
            Assets.SpriteSheets["Mario3"] = content.Load<SpriteSheet>("Sprites/Mario.sf", new JsonContentLoader());
            Assets.SpriteSheets["Luigi3"] = content.Load<SpriteSheet>("Sprites/Luigi.sf", new JsonContentLoader());
            Assets.SpriteSheets["Cloud"] = content.Load<SpriteSheet>("Sprites/Cloud.sf", new JsonContentLoader());
            Assets.Effects["HUDHealthShader"] = content.Load<Effect>("Effects/HUDHealth");
        }
    }
}
