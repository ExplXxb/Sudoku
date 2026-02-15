#if UNITY_2023_2_OR_NEWER
using System;
using UnityEngine.Analytics;

namespace Unity.Services.LevelPlay.Editor
{
    [Serializable]
    struct LevelPlayAnalyticData : IAnalytic.IData
    {
        internal const int Version = 1;
        internal const string EventName = "editorgameserviceeditor";

        public string action;
        public string component;
        public string package;
        public string package_ver;
    }
}
#endif
