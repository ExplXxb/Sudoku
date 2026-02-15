#if UNITY_EDITOR && LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.LevelPlay.Editor
{
    class PackageManagementSettingsView : VisualElement
    {
        const string k_PackageTextLabel = "Package: ";
        const string k_SdkTextLabel = "LevelPlay SDK: ";
        const string k_UnityAdsTextLabel = "Unity Ads SDK: ";
        const string k_DividerPackageLabel = "  |  ";
        const string k_Title = "Package Management";
        const string k_UpdateButtonText = "Update";
        const string k_UpdatedButtonText = "Updated";

        PackageManagementViewModel m_ViewModel;

        Label m_PackageStatusLabel;
        Label m_SdkStatusLabel;
        Label m_UnityAdsStatusLabel;

        internal PackageManagementSettingsView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        }

        async void OnAttachToPanel(AttachToPanelEvent evt)
        {
            try
            {
                UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);

                var networkManager = EditorServices.Instance.LevelPlayNetworkManager;
                var logger = EditorServices.Instance.LevelPlayLogger;
                m_ViewModel = await PackageManagementViewModel.CreateAsync(networkManager, logger);

                SetupUI();
            }
            catch (Exception ex)
            {
                EditorServices.Instance.LevelPlayLogger.LogError($"Failed to load installed package: {ex}");
            }
        }

        void SetupUI()
        {
            style.marginLeft = 8;
            style.marginRight = 8;
            style.marginBottom = 8;

            var packageManagementLabel = LevelPlaySettingsHelper.BuildTitleLabel(k_Title);

            var updateButton = new Button();
            updateButton.clicked += async() => {
                updateButton.SetEnabled(false);
                await m_ViewModel.UpdateAllAsync();
                CheckUpdateAvailability(updateButton);
            };

            CheckUpdateAvailability(updateButton);

            var packageManagementRow = LevelPlaySettingsHelper.DivideVisualElementInARow(packageManagementLabel, updateButton);
            Add(packageManagementRow);

            var packageText = new Label(k_PackageTextLabel);
            var dividerPackage = new Label(k_DividerPackageLabel);

            var sdkText = new Label(k_SdkTextLabel);
            var dividerSdk = new Label(k_DividerPackageLabel);

            var unityAdsText = new Label(k_UnityAdsTextLabel);

            RefreshStatusLabel();

            var statusRow = LevelPlaySettingsHelper.VisualElementsRow(
                packageText,
                m_PackageStatusLabel,
                dividerPackage,
                sdkText,
                m_SdkStatusLabel,
                dividerSdk,
                unityAdsText,
                m_UnityAdsStatusLabel);

            Add(statusRow);
        }

        void RefreshStatusLabel()
        {
            m_PackageStatusLabel = BuildStatusLabel(m_PackageStatusLabel, m_ViewModel.PackageStatus);
            m_SdkStatusLabel = BuildStatusLabel(m_SdkStatusLabel, m_ViewModel.SdkStatus);
            m_UnityAdsStatusLabel = BuildStatusLabel(m_UnityAdsStatusLabel, m_ViewModel.UnityAdsStatus);
        }

        static Label BuildStatusLabel(Label uiLabel, PackageManagementViewModel.Status status)
        {
            var label = uiLabel ?? new Label();

            switch (status)
            {
                case PackageManagementViewModel.Status.UpToDate:
                    label.text = "Up to date";
                    label.style.color = Color.white;
                    break;
                case PackageManagementViewModel.Status.UpdateAvailable:
                    label.text = "[Update available]";
                    label.style.color = Theme.GetColors().ProjectSettingsUpdateLabelColor;
                    break;
                case PackageManagementViewModel.Status.NotInstalled:
                    label.text = "Not installed";
                    label.style.color = Color.white;
                    break;
                case PackageManagementViewModel.Status.Error:
                default:
                    label.text = "Error";
                    label.style.color = Color.red;
                    break;
            }

            return label;
        }

        void CheckUpdateAvailability(Button updateButton)
        {
            if (m_ViewModel.HasUpdate)
            {
                EditorServices.Instance.EditorAnalyticsService.SendUpdateAvailable();
                updateButton.SetEnabled(true);
                updateButton.text = k_UpdateButtonText;
            }
            else
            {
                updateButton.SetEnabled(false);
                updateButton.text = k_UpdatedButtonText;
            }

            RefreshStatusLabel();
        }
    }
}
#endif
