namespace Unity.Services.LevelPlay
{
    sealed class UnsupportedLevelPlayAdSize : IPlatformLevelPlayAdSize
    {
        internal UnsupportedLevelPlayAdSize()
        {
        }

        public int Width
        {
            get
            {
                return 0;
            }
        }

        public int Height
        {
            get
            {
                return 0;
            }
        }

        public PlatformLevelPlayAdSizeType AdSizeType
        {
            get
            {
                return PlatformLevelPlayAdSizeType.Custom;
            }
        }
    }
}
