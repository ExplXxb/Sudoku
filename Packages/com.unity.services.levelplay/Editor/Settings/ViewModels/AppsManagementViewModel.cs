#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;


namespace Unity.Services.LevelPlay.Editor
{
    internal class AppsManagementViewModel
    {
        const string k_UsgBaseUrl = "https://services.unity.com";
        const string k_ServiceAccountEndpoint = "/api/auth/v1/genesis-token-exchange/unity";

        public Apps Apps { get; internal set; }

        public List<string> AppNamesByPlatform => Apps.Select(appKey =>
        {
            var platform = appKey.Platform;
            return string.IsNullOrEmpty(platform) ? appKey.AppName : $"{ appKey.AppName } - { platform }";
        }).ToList();

        public bool HasApps => Apps != null && Apps.Count > 0;

        public int SelectedAppIndex { get; private set; }

        public App SelectedApp => SelectedAppIndex >= 0 && SelectedAppIndex < Apps.Count
        ? Apps[SelectedAppIndex]
        : null;

        public Action<App> OnSelectAppChanged { get; set; }

        public static async Task<AppsManagementViewModel> CreateAsync()
        {
            var viewModel = new AppsManagementViewModel();
            var token = await TokenExchangeAsync();
            await viewModel.LoadAppsAsync(token);
            return viewModel;
        }

        async Task LoadAppsAsync(GatewayToken token)
        {
            var endpoint = GetAppsEndpoint();
            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {token.Token}" }
            };

            var request = new Request(new Uri(endpoint), HttpMethod.Get, headers:headers);
            Apps = await EditorServices.Instance.NetworkingService.SendAsync<Apps>(request);

            SelectedAppIndex = Apps.FindIndex(app => app.AdUnits != null && app.AdUnits.Count > 0);
            if (SelectedAppIndex < 0) SelectedAppIndex = 0;

            OnSelectAppChanged?.Invoke(Apps[SelectedAppIndex]);
        }

        static async Task<GatewayToken> TokenExchangeAsync()
        {
            var genesisToken = CloudProjectSettings.accessToken;
            var request = new Request(new Uri($"{k_UsgBaseUrl}{k_ServiceAccountEndpoint}"), HttpMethod.Post, contentProvider: new JsonContentProvider(new { token = genesisToken }));
            return await EditorServices.Instance.NetworkingService.SendAsync<GatewayToken>(request);
        }

        static string GetAppsEndpoint()
        {
#if ENABLE_EDITOR_GAME_SERVICES
            var organizationId = CloudProjectSettings.organizationKey;
#else
            var organizationId = GetFallbackOrganizationKey();
#endif

            var projectId = CloudProjectSettings.projectId;
            return $"{k_UsgBaseUrl}/api/grow/levelplay/internal/v1/organizations/{organizationId}/projects/{projectId}/apps";
        }

        static string GetFallbackOrganizationKey()
        {
            try
            {
                var connect = typeof(CloudProjectSettings).Assembly.GetType("UnityEditor.Connect.UnityConnect");
                var instance = connect?
                    .GetProperty("instance", BindingFlags.Public | BindingFlags.Static)?
                    .GetValue(null);
                if (instance == null) return string.Empty;

                var method = connect.GetMethod("GetOrganizationForeignKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                return method?.Invoke(instance, Array.Empty<object>()) as string ?? string.Empty;
            }
            catch (Exception ex)
            {
                Services.LevelPlay.LevelPlayLogger.LogWarning($"Failed to get organization key: {ex.Message}");
                return string.Empty;
            }
        }

        public void SelectAppBy(int index)
        {
            if (index < 0 || index >= Apps.Count) return;
            SelectedAppIndex = index;
            OnSelectAppChanged?.Invoke(Apps[SelectedAppIndex]);
        }
    }
}
#endif
