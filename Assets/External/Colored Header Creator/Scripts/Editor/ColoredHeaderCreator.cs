// Created by Dedrick "Baedrick" Koh
// GUI to create Headers in Hierarchy Window
// Version 2.0.r1

using System;
using UnityEngine;
using UnityEditor;

namespace Baedrick.ColoredHeaders
{
    #region GUI Strings

    /// <summary>
    /// 
    /// Stores all strings for Editor Window
    /// 
    /// </summary>
    public static class Strings
	{
        public const string                 fileName                    = "ColoredHeaderSettings";

        public static string                windowTitle                 = "Colored Header Creator";
        public static string                logoText                    = "Baedrick | ColoredHeaders";

        public static string[]              tabHeader                   = { "Header Creator", "Header Presets" };

        public static string                headerTitleText             = "Header Name";
        public static string                headerTitleTooltip          = "Display text for the Header.";
        public static string                headerColorTitleText        = "Header Color";
        public static string                headerColorTitleTooltip     = "Header background color.";
        public static string                textAlignmentTitleText      = "Text Alignment";
        public static string                textAlignmentTitleToolTip   = "Header text alignment.";
        public static string                fontStyleTitleText          = "Font Style";
        public static string                fontStyleTitleToolTip       = "Header text style.";
        public static string                fontSizeTitleText           = "Font Size";
        public static string                fontSizeTitleTooltip        = "Display text for the Header.";
        public static string                fontColorTitleText          = "Font Color";
        public static string                fontColorTitleTooltip       = "Header text color.";
        public static string                dropShadowTitleText         = "Drop Shadow (Slow)";
        public static string                dropShadowTitleTooltip      = "Header text drop shadow. Warning it is slow.";

        public static string                createButtonText            = "Create Header";
        public static string                resetButtonText             = "Reset To Default";
        public static string                deleteButtonText            = "Delete All Headers";
        public static string                loadHeadersButtonText       = "Load Headers From File";
        public static string                saveHeaderPresetText        = "Save Scene Headers As Preset";

        public static string                headerSettingsFoldoutText   = "Header Settings";
        public static string                fontSettingsFoldoutText     = "Font Settings";
        public static string                loadPresetsFoldoutText      = "Load Header Preset";
        public static string                createPresetFoldoutText     = "Create Header Preset";

        public static string                headerPresetFileNameText    = "New Header Preset";

        public static string                errorText                   = "This Message Shouldn't Be Shown";
        public static string                noPresetSelectedText        = "Please Select A File To Load From";
        public static string                successText                 = "Success";

        public static string                headerIdentifierText        = "%$";
    }


	#endregion

	#region Window Settings
    
    /// <summary>
    /// 
    /// Stores window properties for Editor Window
    /// 
    /// </summary>
    public static class WindowProperties
	{
        public static Vector2               windowMinSize               = new Vector2(325, 390);
    }


	#endregion

	#region GUI Styles

	/// <summary>
	/// 
	/// Stores all styles for Editor Window
	/// 
	/// </summary>
	public static class Styles
	{
        public static GUIStyle              logoFont;
        public static GUILayoutOption       logoPosition;
        public static GUILayoutOption       tabsLayout;

        public static GUILayoutOption       createButtonLayout;
        public static GUILayoutOption       loadHeadersButtonLayout;

        public static GUIStyle              headerFontStyle;

        public static Color                 foldoutTintColor;

        static Styles()
		{
            logoFont = new GUIStyle(EditorStyles.label);
            logoFont.alignment = TextAnchor.MiddleCenter;
            logoFont.fontSize = 20;

            logoPosition = GUILayout.Height(50f);

            tabsLayout = GUILayout.Height(26f);

            createButtonLayout = GUILayout.MinHeight(50f);
            loadHeadersButtonLayout = GUILayout.MinHeight(30f);

            foldoutTintColor = EditorGUIUtility.isProSkin
                ? new Color(1f, 1f, 1f, 0.05f)
                : new Color(0f, 0f, 0f, 0.05f);
        }
	}


	#endregion

	#region GUI Implementation

	/// <summary>
	/// 
	/// GUI implementation
	/// 
	/// </summary>
	public class ColoredHeaderCreator : EditorWindow
	{
        #region Variables

        // Tabs
        static int tabSelected = 0;

        // Foldout
        static bool headerBoxFoldout = true;
        static bool headerFontFoldout = true;
        static bool loadPresetsFoldout = true;
        static bool createPresetsFoldout = true;

        // Header Settings
        static string headerText = "New Header";
        static Color headerColor = Color.gray;
        static TextAlignmentOptions textAlignmentOptions = TextAlignmentOptions.Center;
        static FontStyleOptions fontStyleOptions = FontStyleOptions.Bold;
        static float fontSize = 12f;
        static Color fontColor = Color.white;
        static bool dropShadow = false;

        // Preset Tab
        static UnityEngine.Object headerPreset;
        static string headerPresetFileName = Strings.headerPresetFileNameText;


        #endregion

        // Add menu named "Colored Header Creator" to the Tools menu
        [MenuItem("Tools/Colored Header Creator")]
        static void CreateWindow()
        {
            // Check if settings asset exists
            if (!LoadColoredHeaderSettings())
			{
                CreateSettingsAsset();
			}

            // Set settings asset
            LoadSettings(LoadColoredHeaderSettings());
            

            // Get existing open window or if none, make a new one
            EditorWindow window = GetWindow<ColoredHeaderCreator>(Strings.windowTitle);
            window.minSize = WindowProperties.windowMinSize;
        }

        void OnGUI()
		{
            DrawLogo();
            OptionsTabs();
        }

		#region Logo

        void DrawLogo()
		{
            Rect logoRect = EditorGUILayout.GetControlRect(Styles.logoPosition);
            if (Event.current.type == EventType.Repaint)
                Styles.logoFont.Draw(logoRect, Strings.logoText, false, false, false, false);
        }


		#endregion

		#region Display Tabs

		void OptionsTabs()
		{
            GUILayout.BeginHorizontal();

                GUILayout.Space(15f);

                EditorGUI.BeginChangeCheck();

                tabSelected = GUILayout.Toolbar(tabSelected, Strings.tabHeader, Styles.tabsLayout);

                // Unselect input fields if changed tabs
                if (EditorGUI.EndChangeCheck())
                {
                    GUI.FocusControl(null);
                }

                GUILayout.Space(10f);

            GUILayout.EndHorizontal();

            switch (tabSelected)
            {
                case 0:
                    HeaderCreator();
                    break;

                case 1:
                    PresetCreator();
                    break;

                default:
                    HeaderCreator();
                    Debug.LogError(Strings.errorText);
                    break;
            }
        }

        #endregion

        #region Header Creation Tab

        void HeaderCreator()
        {
            GUILayout.Space(8);

            headerBoxFoldout = Foldout(headerBoxFoldout, Strings.headerSettingsFoldoutText);

            if (headerBoxFoldout)
			{
                headerText              = EditorGUILayout.TextField(new GUIContent(Strings.headerTitleText, Strings.headerTitleTooltip), headerText);
                headerColor             = EditorGUILayout.ColorField(new GUIContent(Strings.headerColorTitleText, Strings.headerColorTitleTooltip), headerColor);
            }

            headerFontFoldout = Foldout(headerFontFoldout, Strings.fontSettingsFoldoutText);

            if (headerFontFoldout)
			{
                textAlignmentOptions    = (TextAlignmentOptions)EditorGUILayout.EnumPopup(new GUIContent(Strings.textAlignmentTitleText, Strings.textAlignmentTitleToolTip), textAlignmentOptions);
                fontStyleOptions        = (FontStyleOptions)EditorGUILayout.EnumPopup(new GUIContent(Strings.fontStyleTitleText, Strings.fontStyleTitleToolTip), fontStyleOptions);
                fontSize                = EditorGUILayout.Slider(new GUIContent(Strings.fontSizeTitleText, Strings.fontSizeTitleTooltip), fontSize, 1, 20);
                fontColor               = EditorGUILayout.ColorField(new GUIContent(Strings.fontColorTitleText, Strings.fontColorTitleTooltip), fontColor);
                dropShadow              = EditorGUILayout.Toggle(new GUIContent(Strings.dropShadowTitleText, Strings.dropShadowTitleTooltip), dropShadow);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(Strings.createButtonText, Styles.createButtonLayout))
            {
                CreateHeader();
                SaveSettings(LoadColoredHeaderSettings());
            }

            GUILayout.Space(2);

            if (GUILayout.Button(Strings.resetButtonText))
			{
                headerText = "New Header";
                dropShadow = false;
                headerColor = Color.gray;
                fontStyleOptions = FontStyleOptions.Bold;
                textAlignmentOptions = TextAlignmentOptions.Center;
                fontSize = 12f;
                fontColor = Color.white;

                SaveSettings(LoadColoredHeaderSettings());
            }

            GUILayout.Space(2);

            if (GUILayout.Button(Strings.deleteButtonText))
			{
				GameObject[] sceneGameObject = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
				foreach (GameObject obj in sceneGameObject)
				{
					if (obj.name.StartsWith(Strings.headerIdentifierText, StringComparison.Ordinal) && obj.GetComponent(typeof(ColoredHeaderObject)) != null)
                    {
                        // Destroy Object
                        obj.transform.DetachChildren();
                        DestroyImmediate(obj);
					}
				}
			}
		}

        void CreateHeader()
		{
            string gameObjectName = Strings.headerIdentifierText + " " + "Header";

            GameObject obj = new GameObject(gameObjectName);

            // Add header component with properties
            obj.AddComponent<ColoredHeaderObject>();
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerText = headerText;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerColor = headerColor;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.textAlignmentOptions = textAlignmentOptions;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontStyleOptions = fontStyleOptions;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontSize = fontSize;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontColor = fontColor;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.dropShadow = dropShadow;

            // Unity removes all objects with Editor Only tag during Build
            obj.tag = "EditorOnly";

            obj.transform.position = Vector3.zero;

            // Lets Unity know that something in the scene changed
            EditorUtility.SetDirty(obj);
        }

        void UpdateHeader(GameObject obj)
		{
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerText = headerText;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerColor = headerColor;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.textAlignmentOptions = textAlignmentOptions;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontStyleOptions = fontStyleOptions;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontSize = fontSize;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontColor = fontColor;
            obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.dropShadow = dropShadow;

            // Lets Unity know that something in the scene changed
            EditorUtility.SetDirty(obj);
        }


        #endregion

        #region Header Presets Tab

        void PresetCreator()
		{
            loadPresetsFoldout = Foldout(loadPresetsFoldout, Strings.loadPresetsFoldoutText);

            if (loadPresetsFoldout)
            {
                headerPreset = EditorGUILayout.ObjectField("Header Preset File", headerPreset, typeof(ColoredHeaderPresets), false);

                if (GUILayout.Button(Strings.loadHeadersButtonText, Styles.loadHeadersButtonLayout))
                {
                    if (headerPreset != null)
                        CreateHeadersFromPreset((ColoredHeaderPresets)headerPreset);
                    else
                        Debug.LogError(Strings.noPresetSelectedText);
                }
            }

            createPresetsFoldout = Foldout(createPresetsFoldout, Strings.createPresetFoldoutText);

            if (createPresetsFoldout)
            {
                headerPresetFileName = EditorGUILayout.TextField("File Name", headerPresetFileName);

                if (GUILayout.Button(Strings.saveHeaderPresetText, Styles.loadHeadersButtonLayout))
                {
                    CreatePresetFile(headerPresetFileName);
                }
            }
        }

        // TODO simplify header creation function and use it here
        void CreateHeadersFromPreset(ColoredHeaderPresets headerPreset)
		{
			foreach (ColorSettings header in headerPreset.coloredHeaderPresets)
			{
                string gameObjectName = Strings.headerIdentifierText + " " + "Header";

                GameObject obj = new GameObject(gameObjectName);

                // Add header component with properties
                obj.AddComponent<ColoredHeaderObject>();
                obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerText = header.headerText;
                obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerColor = header.headerColor;
                obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.textAlignmentOptions = header.textAlignmentOptions;
                obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontStyleOptions = header.fontStyleOptions;
                obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontSize = header.fontSize;
                obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontColor = header.fontColor;
                obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.dropShadow = header.dropShadow;

                // Unity removes all objects with Editor Only tag during Build
                obj.tag = "EditorOnly";

                obj.transform.position = Vector3.zero;

                // Lets Unity know that something in the scene changed
                EditorUtility.SetDirty(obj);
            }

            Debug.Log(Strings.successText);
        }

        void CreatePresetFile(string fileName)
		{
            string path = "Assets/Baedrick/Colored Header Creator/Presets";

            // Creates preset file
            ScriptableObject asset = CreateInstance<ColoredHeaderPresets>();
            AssetDatabase.CreateAsset(asset, path + $"/{fileName}.asset");

            // Loads preset file
            var result = EditorGUIUtility.Load(path + $"/{fileName}.asset") as ColoredHeaderPresets;
            result.coloredHeaderPresets.Clear();

            // Get all headers in the scene
            GameObject[] sceneGameObject = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject obj in sceneGameObject)
            {
                // Saves header settings to preset file
                if (obj.name.StartsWith(Strings.headerIdentifierText, StringComparison.Ordinal) && obj.GetComponent(typeof(ColoredHeaderObject)) != null)
                {
                    ColorSettings colorSettings = new ColorSettings();

                    colorSettings.headerText = obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerText;
                    colorSettings.headerColor = obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerColor;
                    colorSettings.textAlignmentOptions = obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.textAlignmentOptions;
                    colorSettings.fontStyleOptions = obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontStyleOptions;
                    colorSettings.fontSize = obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontSize;
                    colorSettings.fontColor = obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontColor;
                    colorSettings.dropShadow = obj.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.dropShadow;

                    result.coloredHeaderPresets.Add(colorSettings);
                }
            }

            EditorUtility.SetDirty(result);
            AssetDatabase.SaveAssets();

            Debug.Log(Strings.successText);
        }


        #endregion

        #region Extending The Editor

        /// <summary>
        /// 
        /// Extending Unity's foldout because FoldoutHeaderGroup doesn't do what I want
        /// 
        /// </summary>
        static bool Foldout(bool foldout, string content)
        {
            Rect rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight, EditorStyles.foldout);

            EditorGUI.DrawRect(EditorGUI.IndentedRect(rect), Styles.foldoutTintColor);

            Rect foldoutRect = rect;
            foldoutRect.width = EditorGUIUtility.singleLineHeight;
            foldout = EditorGUI.Foldout(rect, foldout, "", true);

            // Show Foldout Triangle
            rect.x += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rect, content, EditorStyles.boldLabel);

            return foldout;
        }

        /// <summary>
        /// 
        /// Load settings
        /// Credits to Federico Bellucci for asset loading
        /// febucci.com
        /// 
        /// </summary>
        static ColoredHeaderSettings LoadColoredHeaderSettings()
        {
            var result = EditorGUIUtility.Load($"Baedrick/Colored Header Creator/Scripts/Runtime/{Strings.fileName}.asset") as ColoredHeaderSettings;
            if (result != null)
                return result;

            var guids = AssetDatabase.FindAssets("t:" + nameof(ColoredHeaderSettings));
            if (guids.Length == 0)
                return null;

            return AssetDatabase.LoadAssetAtPath<ColoredHeaderSettings>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        static void CreateSettingsAsset()
        {
            string path = "Assets/Baedrick/Colored Header Creator/Scripts/Runtime";

            // Creates setting asset
            ScriptableObject asset = CreateInstance<ColoredHeaderSettings>();
            AssetDatabase.CreateAsset(asset, path + $"/{Strings.fileName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static void LoadSettings(ColoredHeaderSettings coloredHeaderSettings)
		{
            headerText                  = coloredHeaderSettings.headerText;
            headerColor                 = coloredHeaderSettings.headerColor;
            textAlignmentOptions        = coloredHeaderSettings.textAlignmentOptions;
            fontStyleOptions            = coloredHeaderSettings.fontStyleOptions;
            fontSize                    = coloredHeaderSettings.fontSize;
            fontColor                   = coloredHeaderSettings.fontColor;
            dropShadow                  = coloredHeaderSettings.dropShadow;
        }

        static void SaveSettings(ColoredHeaderSettings coloredHeaderSettings)
        {
            coloredHeaderSettings.headerText            = headerText;
            coloredHeaderSettings.headerColor           = headerColor;
            coloredHeaderSettings.textAlignmentOptions  = textAlignmentOptions;
            coloredHeaderSettings.fontStyleOptions      = fontStyleOptions;
            coloredHeaderSettings.fontSize              = fontSize;
            coloredHeaderSettings.fontColor             = fontColor;
            coloredHeaderSettings.dropShadow            = dropShadow;
        }


        #endregion
    }


#endregion
}