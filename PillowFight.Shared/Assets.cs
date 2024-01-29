using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.Sprites;
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
    }

    public class AssetLoader
    {
        public void Load(ContentManager content, GraphicsDevice graphicsDevice)
        {
            // Assets.Images["Mario"] = content.Load<Texture2D>("Sprites/Mario");
            Assets.Images["SMBTilesheet"] = content.Load<Texture2D>("Sprites/SMBTilesheet");
            Assets.Fonts["Arial"] = content.Load<SpriteFont>("Fonts/Arial");
            Assets.Images["Mario3Sheet"] = content.Load<Texture2D>("Sprites/PlumberFellasSpritesheet");
            // Assets.Aseprites["Mario"] = SpriteSheetProcessor.Process(graphicsDevice, AsepriteFile.Load(Path.Combine(content.RootDirectory, "Sprites/Mario.aseprite")));
            // Assets.Aseprites["Luigi"] = SpriteSheetProcessor.Process(graphicsDevice, AsepriteFile.Load(Path.Combine(content.RootDirectory, "Sprites/Luigi.aseprite")));
            // Assets.Aseprites["Cloud"] = SpriteSheetProcessor.Process(graphicsDevice, AsepriteFile.Load(Path.Combine(content.RootDirectory, "Sprites/Cloud.aseprite")));
            Assets.Aseprites["Mario"] = SpriteSheetProcessor.Process(graphicsDevice, content.Load<AsepriteFile>("Sprites/Mario"));
            Assets.Aseprites["Luigi"] = SpriteSheetProcessor.Process(graphicsDevice, content.Load<AsepriteFile>("Sprites/Luigi"));
            Assets.Aseprites["Cloud"] = SpriteSheetProcessor.Process(graphicsDevice, content.Load<AsepriteFile>("Sprites/Cloud"));
            Assets.Effects["HUDHealthShader"] = content.Load<Effect>("Effects/HUDHealth");

            string data = File.ReadAllText("PillowFight.Shared/Content/UI/Menu.xmml");
            Assets.UIProjects["Menu"] = Project.LoadFromXml(data);
        }
    }
}
