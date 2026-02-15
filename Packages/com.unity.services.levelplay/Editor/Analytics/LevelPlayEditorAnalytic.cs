#if UNITY_2023_2_OR_NEWER
using System;
using UnityEngine.Analytics;

namespace Unity.Services.LevelPlay.Editor
{
    [AnalyticInfo(
        eventName: LevelPlayAnalyticData.EventName,
        vendorKey: "unity.services.levelplay",
        version: LevelPlayAnalyticData.Version)]
    internal class LevelPlayEditorAnalytic : IAnalytic
    {
        LevelPlayAnalyticData m_Data;

        public LevelPlayEditorAnalytic(string component, string action, string package, string package_ver)
        {
            m_Data = new LevelPlayAnalyticData
            {
                component = component,
                action = action,
                package = package,
                package_ver = package_ver
            };
        }

        public bool TryGatherData(out IAnalytic.IData data, out Exception error)
        {
            error = null;
            data = m_Data;
            return true;
        }
    }
}
#endif
