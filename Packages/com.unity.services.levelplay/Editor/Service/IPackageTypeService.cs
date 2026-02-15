namespace Unity.Services.LevelPlay.Editor
{
    enum PackageType
    {
        Upm = 0,
        DotUnityPackage = 1
    }

    interface IPackageTypeService
    {
        PackageType PackageType { get; }
    }
}
