using System;
using UnityEditor;

namespace Unity.Services.LevelPlay.Editor
{
    class EditorAnalyticsSender : IEditorAnalyticsSender
    {
        const string k_EventName = "editorgameserviceeditor";
        const int k_EventVersion = 1;
        public void Send(string component, string action)
        {
            try
            {
#if UNITY_2023_2_OR_NEWER
                EditorAnalytics.SendAnalytic(new LevelPlayEditorAnalytic(component, action, Constants.k_PackageAnalyticsIdentifier, Constants.k_AnnotatedPackageVersion));
#else
                EditorAnalytics.SendEventWithLimit(k_EventName,
                    new EventBody()
                    {
                        component = component,
                        action = action,
                        package = Constants.k_PackageAnalyticsIdentifier,
                        package_ver = Constants.k_AnnotatedPackageVersion
                    },
                    k_EventVersion);
#endif
            }
            catch (Exception)
            {
                // Silent catch because error in analytics shouldn't prevent users from executing their action
            }
        }

        [Serializable]
        internal class EventBody
        {
            public string action;
            public string component;
            public string package;
            public string package_ver;
        }
    }
}
