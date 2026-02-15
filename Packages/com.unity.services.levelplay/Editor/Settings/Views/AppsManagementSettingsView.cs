#if UNITY_EDITOR && LEVELPLAY_DEPENDENCIES_INSTALLED
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Services.LevelPlay.Editor
{
    class AppsManagementSettingsView : VisualElement
    {
        const string k_Title = "Apps and Ad Units Management (Alpha)";
        const string k_CreateAppsDescription = "Manage your account and create apps and ad units in the dashboard.";
        const string k_CreateAppsErrorDescription = "This feature is in Alpha and currently unavailable for your project. Apps and Ad Units management is only available in the LevelPlay dashboard at this time.";
        const string k_CreateAppsText = "Create an ad unit in LevelPlay Dashboard to start monetizing.";
        const string k_CreateAppsButtonText = "Open LevelPlay Dashboard";
        const string k_CreateAppsUrl = "http://platform.ironsrc.com/";
        const string k_AdUnitColumnName = "Ad Unit";
        const string k_PlatformColumnName = "Platform";
        const string k_AdFormatColumnName = "Ad Format";
        const string k_AdUnitIdColumnName = "ID";
        const string k_AdUnitLabelName = "AdUnitLabel";
        const string k_PlatformLabelName = "PlatformLabel";
        const string k_AdFormatLabelName = "AdformatLabel";
        const string k_AdUnitIdLabelName = "IDLabel";
        const string k_IconName = "TreeEditor.Duplicate";
        const string k_ClipBoardIconName = "ClipBoard";

        AppsManagementViewModel m_ViewModel;

        DropdownField m_AppListDropDown;
        VisualElement m_AdUnitListView;
        Label m_AppKeyLabel;
        VisualElement m_AdUnitsContainer;

        void SetupAppListDropDown()
        {
            m_AppListDropDown = new DropdownField(m_ViewModel.AppNamesByPlatform, m_ViewModel.SelectedAppIndex)
            {
                style = { width = 250, height = 25, marginTop = 3, marginLeft = 0}
            };
            m_AppListDropDown.RegisterValueChangedCallback(e =>
            {
                var index = m_ViewModel.AppNamesByPlatform.IndexOf(e.newValue);
                m_ViewModel.SelectAppBy(index);
                EditorServices.Instance.EditorAnalyticsService.SendNavigateApps();
            });
        }

        void SetupAppKeyLabel() => m_AppKeyLabel =
            LevelPlaySettingsHelper.BuildNoFlexLabel($"AppKey: {m_ViewModel.SelectedApp?.Key ?? "Not Found"}");

        VisualElement SetupClipboardIcon()
        {
            var appKeyClipboardIcon = new VisualElement
            {
                style =
                {
                    width = 25,
                    height = 25,
                    backgroundImage =
                        new StyleBackground(EditorGUIUtility.IconContent(k_IconName).image as Texture2D)
                }
            };

            appKeyClipboardIcon.RegisterCallback<ClickEvent>(_ =>
            {
                GUIUtility.systemCopyBuffer = m_ViewModel.SelectedApp?.Key ?? string.Empty;
                EditorServices.Instance.EditorAnalyticsService.SendCopyAppKey();
            });

            return appKeyClipboardIcon;
        }

        static VisualElement SetupAppInfoTable()
        {
            var container = new VisualElement { style = { flexDirection = FlexDirection.Column } };

            var columnHeader = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    justifyContent = Justify.SpaceBetween,
                    alignItems = Align.Center
                }
            };

            var adUnitHeader = LevelPlaySettingsHelper.BuildLabel(k_AdUnitColumnName);
            columnHeader.Add(adUnitHeader);

            var adPlatformHeader = LevelPlaySettingsHelper.BuildLabel(k_PlatformColumnName);
            columnHeader.Add(adPlatformHeader);

            var adFormatHeader = LevelPlaySettingsHelper.BuildLabel(k_AdFormatColumnName);
            columnHeader.Add(adFormatHeader);

            var spacingForIcon = new VisualElement { style = { width = 25, height = 25 } };

            var adUnitIdHeader = LevelPlaySettingsHelper.BuildLabel(k_AdUnitIdColumnName);
            var lastHeaderTest = LevelPlaySettingsHelper.DivideVisualElementInARow(adUnitIdHeader, spacingForIcon);
            lastHeaderTest.style.flexGrow = 1;
            lastHeaderTest.style.flexShrink = 0;
            lastHeaderTest.style.flexBasis = 0;

            columnHeader.Add(lastHeaderTest);

            container.Add(columnHeader);

            return container;
        }

        VisualElement SetupAdUnitListView(App app)
        {
            m_AdUnitListView = new ListView
            {
                itemsSource = app.AdUnits,
                style =
                {
                    flexGrow = 1,
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderBottomColor = Color.black,
                    borderLeftColor = Color.black,
                    borderRightColor = Color.black,
                    borderTopColor = Color.black
                },
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                makeItem = () =>
                {
                    var itemContainer = new VisualElement
                    {
                        style =
                        {
                            flexDirection = FlexDirection.Row,
                            justifyContent = Justify.SpaceBetween,
                            alignItems = Align.Center
                        }
                    };

                    var adUnitLabel = LevelPlaySettingsHelper.BuildLabel(string.Empty);
                    adUnitLabel.name = k_AdUnitLabelName;

                    itemContainer.Add(adUnitLabel);

                    var platformLabel = LevelPlaySettingsHelper.BuildLabel(string.Empty);
                    platformLabel.name = k_PlatformLabelName;

                    itemContainer.Add(platformLabel);

                    var adFormatLabel = LevelPlaySettingsHelper.BuildLabel(string.Empty);
                    adFormatLabel.name = k_AdFormatLabelName;

                    itemContainer.Add(adFormatLabel);

                    var adUnitIdLabel = LevelPlaySettingsHelper.BuildLabel(string.Empty);
                    adUnitIdLabel.name = k_AdUnitIdLabelName;

                    var clipboardIcon = new VisualElement
                    {
                        name = k_ClipBoardIconName,
                        style =
                        {
                            width = 25,
                            height = 25,
                            backgroundImage =
                                new StyleBackground(EditorGUIUtility.IconContent(k_IconName).image as Texture2D)
                        }
                    };

                    var adUnitRow = LevelPlaySettingsHelper.DivideVisualElementInARow(adUnitIdLabel, clipboardIcon);
                    adUnitRow.style.flexGrow = 1;
                    adUnitRow.style.flexShrink = 0;
                    adUnitRow.style.flexBasis = 0;
                    itemContainer.Add(adUnitRow);

                    return itemContainer;
                },
                bindItem = (element, index) =>
                {
                    var currentApp = m_ViewModel.SelectedApp;
                    var item = currentApp.AdUnits[index];

                    var adUnitLabel = element.Q<Label>(k_AdUnitLabelName);
                    var platformLabel = element.Q<Label>(k_PlatformLabelName);
                    var adFormatLabel = element.Q<Label>(k_AdFormatLabelName);
                    var adUnitIdLabel = element.Q<Label>(k_AdUnitIdLabelName);

                    var clipboardIcon = element.Q(k_ClipBoardIconName);

                    adUnitLabel.text = item.AdUnitName;
                    platformLabel.text = currentApp.Platform;
                    adFormatLabel.text = item.Format;
                    adUnitIdLabel.text = item.AdUnitId;

                    clipboardIcon.RegisterCallback<ClickEvent>(_ =>
                    {
                        GUIUtility.systemCopyBuffer = adUnitIdLabel.text;
                        EditorServices.Instance.EditorAnalyticsService.SendCopyAdUnit();
                    });
                }
            };

            return m_AdUnitListView;
        }

        void SetupBlankAppsUI(bool isFromErrors = false)
        {
            style.marginLeft = 8;
            style.marginRight = 8;
            style.marginBottom = 4;

            var createAppsDescriptionLabel = new Label(isFromErrors ? k_CreateAppsErrorDescription : k_CreateAppsDescription)
            {
                style =
                {
                    whiteSpace = WhiteSpace.Normal,
                    flexWrap = Wrap.Wrap,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    width = Length.Percent(100),
                    marginTop = 4
                }
            };

            Add(createAppsDescriptionLabel);
        }

        void SetupOpenDashboardButton(string analyticsAction)
        {
            var openDashboardButton = CreateOpenDashboardButton(analyticsAction);
            openDashboardButton.style.marginTop = 10;
            openDashboardButton.style.marginLeft = 0;
            Add(openDashboardButton);
        }

        static VisualElement SetupBlankAdUnitsUI()
        {
            var frame = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    alignItems = Align.Center,
                    justifyContent = Justify.Center,
                    paddingTop = 15,
                    paddingBottom = 15,
                    paddingLeft = 10,
                    paddingRight = 10,
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopColor = new Color(0, 0, 0),
                    borderBottomColor = new Color(0, 0, 0),
                    borderLeftColor = new Color(0, 0, 0),
                    borderRightColor = new Color(0, 0, 0),
                }
            };

            var descriptionLabel = new Label(k_CreateAppsText)
            {
                style = { unityTextAlign = TextAnchor.MiddleCenter, marginBottom = 8 }
            };

            var openDashboardButton = CreateOpenDashboardButton(EditorAnalyticsService.LevelPlayAction.OpenDashboardWithNoAdUnits);

            frame.Add(descriptionLabel);
            frame.Add(openDashboardButton);

            return frame;
        }

        static Button CreateOpenDashboardButton(string analyticsAction) =>
            new(() =>
            {
                EditorServices.Instance.EditorAnalyticsService.SendOpenDashboard(analyticsAction);
                Application.OpenURL(k_CreateAppsUrl);
            })
            {
                text = k_CreateAppsButtonText,
                style =
                {
                    width = 166,
                    fontSize = 12,
                    paddingTop = 4,
                    paddingBottom = 4,
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };

        void SetupStaticUI()
        {
            Clear();

            style.marginLeft = 8;
            style.marginRight = 8;
            style.marginBottom = 8;

            var titleLabel = LevelPlaySettingsHelper.BuildTitleLabel(k_Title);
            Add(titleLabel);
        }

        void SetupUI()
        {
            SetupStaticUI();

            if (m_ViewModel.HasApps)
            {
                SetupAppListDropDown();
                SetupAppKeyLabel();
                var appKeyClipboardIcon = SetupClipboardIcon();
                var appsRow = LevelPlaySettingsHelper.VisualElementsRow(m_AppListDropDown, m_AppKeyLabel, appKeyClipboardIcon);
                Add(appsRow);

                m_AdUnitsContainer = SetupAppInfoTable();
                RefreshAdUnitsView(m_ViewModel.SelectedApp);
                Add(m_AdUnitsContainer);
            }
            else
            {
                EditorServices.Instance.EditorAnalyticsService.SendAppsNotAvailable();
                SetupBlankAppsUI();
                SetupOpenDashboardButton(EditorAnalyticsService.LevelPlayAction.OpenDashboardWithNoApps);
            }
        }

        void RefreshAdUnitsView(App app)
        {
            if (m_AdUnitsContainer == null) return;

            while (m_AdUnitsContainer.childCount > 1)
            {
                m_AdUnitsContainer.RemoveAt(1);
            }

            if (app?.AdUnits == null || app.AdUnits.Count == 0)
            {
                m_AdUnitListView = null;
                var blankUI = SetupBlankAdUnitsUI();
                m_AdUnitsContainer.Add(blankUI);
                EditorServices.Instance.EditorAnalyticsService.SendAdUnitsNotAvailable();
            }
            else
            {
                var adUnitView = SetupAdUnitListView(app);
                m_AdUnitsContainer.Add(adUnitView);
                EditorServices.Instance.EditorAnalyticsService.SendAdUnitsAvailable();
            }
        }


        public AppsManagementSettingsView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        }

        async void OnAttachToPanel(AttachToPanelEvent evt)
        {
            Label label = null;

            try
            {
                label = new Label("Retrieving data...");
                SetupStaticUI();
                Add(label);

                UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);

                m_ViewModel = await AppsManagementViewModel.CreateAsync();
                m_ViewModel.OnSelectAppChanged = Refresh;

                Remove(label);
                SetupUI();
            }
            catch
            {
                if (label?.parent != null) label.RemoveFromHierarchy();
                SetupBlankAppsUI(true);
                SetupOpenDashboardButton(EditorAnalyticsService.LevelPlayAction.OpenDashboardNotMapped);
                EditorServices.Instance.EditorAnalyticsService.SendProjectNotMapped();
            }
        }

        void Refresh(App app)
        {
            if (m_AppListDropDown != null)
            {
                m_AppListDropDown.choices = m_ViewModel.AppNamesByPlatform;
                m_AppListDropDown.index = m_ViewModel.SelectedAppIndex;
            }

            if (m_AppKeyLabel != null)
                m_AppKeyLabel.text = $"AppKey: {app.Key ?? "Not Found"}";

            RefreshAdUnitsView(app);
        }
    }
}
#endif
