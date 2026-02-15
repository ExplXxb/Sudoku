using System;
using UnityEditor;

namespace Unity.Services.LevelPlay.Editor
{
    class AssetDatabaseService : IAssetDatabaseService
    {
        public bool DeleteAsset(string path)
        {
            return AssetDatabase.DeleteAsset(path);
        }
    }
}
