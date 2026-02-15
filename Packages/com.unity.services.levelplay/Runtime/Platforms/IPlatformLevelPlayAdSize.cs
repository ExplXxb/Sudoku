namespace Unity.Services.LevelPlay
{
    enum PlatformLevelPlayAdSizeType
    {
        Unknown = 0,
        Banner = 1,
        Large = 2,
        MediumRectangle = 3,
        Custom = 4,
        LeaderBoard = 5,
        Adaptive = 6
    }

    interface IPlatformLevelPlayAdSize
    {
        int Width { get; }
        int Height { get; }
        PlatformLevelPlayAdSizeType AdSizeType { get; }
    }
}
