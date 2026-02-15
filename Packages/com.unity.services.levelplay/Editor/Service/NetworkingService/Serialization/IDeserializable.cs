namespace Unity.Services.LevelPlay.Editor
{
    internal interface IDeserializable
    {
        bool CanDeserialize(string mediaType);
    }
}
