namespace PillowFight.Shared
{
    internal class Enums
    {
        public static class Screen
        {
            // default window size
            public const int WidthDefault = 1280;
            public const int HeightDefault = 720;

            // internal screen resolution
            public const int Width = 640;
            public const int Height = 360;
        }

        public static class Camera
        {
            public const float CameraXOffsetRatio = 0.25f;
            public const float CameraYOffsetRatio = 0.25f;
        }

        public static class Gameplay
        {
            public const int DespawnDistance = 4 * 32;
            public const int SpawnDistance = 2 * 32;
            public const float BlockPushSpeed = 16.0f;
        }

        public enum Player
        {
            Player1,
            Player2,
            Player3,
            Player4,
        }


        public enum HDirection
        {
            Left,
            Right
        }
        public enum VDirection
        {
            Up,
            Down,
        }

        public enum PillowState
        {
            Held,
            Stationery,
            Running,
            Projectile
        }
    }
}
