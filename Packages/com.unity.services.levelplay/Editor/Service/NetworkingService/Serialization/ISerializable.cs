namespace Unity.Services.LevelPlay.Editor
{
    internal interface ISerializable
    {
        bool CanSerialize(string mediaType);
    }
}