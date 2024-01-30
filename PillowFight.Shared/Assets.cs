using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
using MonoGame.Extended.Tiled;
using Myra.Graphics2D.UI;

namespace PillowFight.Shared
{
    public static class Assets
    {
        public static Dictionary<string, Texture2D> Images = new();
        public static Dictionary<string, SpriteFont> Fonts = new();
        public static Dictionary<string, SpriteSheet> Aseprites = new();
        public static Dictionary<string, Effect> Effects = new();
        public static Dictionary<string, Project> UIProjects = new();
        public static Dictionary<string, TiledMap> Maps = new();
    }

    public class AssetLoader
    {
        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Assets.Images["Tilesheet"] = content.Load<Texture2D>("Sprites/smb3");
            Assets.Images["Cloud"] = FromTilesheet(1, 3, graphicsDevice);
            Assets.Images["Box"] = content.Load<Texture2D>("UI/Setting menu");
            Assets.Images["Button"] = content.Load<Texture2D>("UI/Button");
            Assets.Images["ButtonPress"] = content.Load<Texture2D>("UI/ButtonPress");
            Assets.Fonts["Arial"] = content.Load<SpriteFont>("Fonts/Arial");
            // Assets.Images["Mario3Sheet"] = content.Load<Texture2D>("Sprites/PlumberFellasSpritesheet");
            Assets.Aseprites["Mario"] = SpriteSheetProcessor.Process(graphicsDevice, content.Load<AsepriteFile>("Sprites/Mario3"));
            Assets.Aseprites["Luigi"] = SpriteSheetProcessor.Process(graphicsDevice, content.Load<AsepriteFile>("Sprites/Luigi"));
            Assets.Aseprites["Cloud"] = SpriteSheetProcessor.Process(graphicsDevice, content.Load<AsepriteFile>("Sprites/Cloud"));
            // Assets.Effects["HUDHealthShader"] = content.Load<Effect>("Effects/HUDHealth");
            Assets.Effects["Whiteout"] = content.Load<Effect>("Effects/Whiteout");
            Assets.Effects["LSD"] = content.Load<Effect>("Effects/LSD");
            Assets.Maps["Lv1"] = content.Load<TiledMap>("Maps/Tutorial3");
            string data = File.ReadAllText("PillowFight.Shared/Content/UI/Menu.xmml");
        }
        private Texture2D FromTilesheet(int x, int y, GraphicsDevice gd) {
            int xSize = x * 17 + 1;
            int ySize = y * 17 + 1;
            Texture2D slice = new Texture2D(gd, 16, 16);
            Color[] data = new Color[16 *  16];
            Assets.Images["Tilesheet"].GetData(0, new Rectangle(xSize, ySize, 16, 16), data, 0, 16*16);
            slice.SetData(data);
            return slice;
        }
    }
}
