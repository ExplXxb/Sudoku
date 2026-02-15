namespace Unity.Services.LevelPlay.Editor
{
    internal interface IHttpContentSerializable : ISerializable
    {
        byte[] Serialize<T>(T model);
        string SerializeToString<T>(T model);
    }
}