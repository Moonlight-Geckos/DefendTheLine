// Created by Dedrick "Baedrick" Koh
// Draws Headers in Hierarchy Window
// Version 2.0.r1

using UnityEngine;
using UnityEditor;

namespace Baedrick.ColoredHeaders
{

	[InitializeOnLoad]
	public static class ColoredHeaderDisplay
	{
		static ColoredHeaderDisplay()
		{
			// Performs action for every visible item in Hierarchy Window
			EditorApplication.hierarchyWindowItemOnGUI += RenderObjects;
		}

		/// <summary>
		/// 
		/// Hierarchy Renderer
		/// 
		/// </summary>
		static void RenderObjects(int instanceID, Rect selectionRect)
		{
			// Stops drawing headers when in Play Mode
			if (Application.isPlaying)
				return;

			GameObject sceneGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

			// Skips when Object is not valid
			if (sceneGameObject == null)
				return;

			if (sceneGameObject.name.StartsWith(Strings.headerIdentifierText, System.StringComparison.Ordinal) && sceneGameObject.GetComponent(typeof(ColoredHeaderObject)) != null)
			{
				//EditorGUI.DrawRect(selectionRect, Color.cyan);
				RenderHeaders(sceneGameObject, selectionRect);
			}
		}

		// Unpacks header settings and draws header
		static void RenderHeaders(GameObject gameObject, Rect selectionRect)
		{
			// Header Properties
			string headerText							= gameObject.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerText;
			Color headerColor							= gameObject.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.headerColor;
			TextAlignmentOptions textAlignmentOptions	= gameObject.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.textAlignmentOptions;
			FontStyleOptions fontStyleOptions			= gameObject.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontStyleOptions;
			float fontSize								= gameObject.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontSize;
			Color fontColor								= gameObject.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.fontColor;
			bool dropShadow								= gameObject.GetComponent(typeof(ColoredHeaderObject)).GetComponent<ColoredHeaderObject>().colorDesigns.dropShadow;

			Styles.headerFontStyle = new GUIStyle(EditorStyles.label);

			switch ((int)textAlignmentOptions)
			{
				case 0:
					Styles.headerFontStyle.alignment = TextAnchor.MiddleCenter;
					break;
				case 1:
					Styles.headerFontStyle.alignment = TextAnchor.MiddleLeft;
					break;
				case 2:
					Styles.headerFontStyle.alignment = TextAnchor.MiddleRight;
					break;
			}

			switch ((int)fontStyleOptions)
			{
				case 0:
					Styles.headerFontStyle.fontStyle = FontStyle.Bold;
					break;
				case 1:
					Styles.headerFontStyle.fontStyle = FontStyle.Normal;
					break;
				case 2:
					Styles.headerFontStyle.fontStyle = FontStyle.Italic;
					break;
				case 3:
					Styles.headerFontStyle.fontStyle = FontStyle.BoldAndItalic;
					break;
			}

			Styles.headerFontStyle.fontSize = Mathf.RoundToInt(fontSize);
			Styles.headerFontStyle.normal.textColor = fontColor;

			if (!dropShadow)
			{
				EditorGUI.DrawRect(selectionRect, headerColor);
				EditorGUI.LabelField(selectionRect, headerText.ToUpperInvariant(), Styles.headerFontStyle);
			}
			else
			{
				EditorGUI.DrawRect(selectionRect, headerColor);
				EditorGUI.DropShadowLabel(selectionRect, headerText.ToUpperInvariant(), Styles.headerFontStyle);
			}
		}
	}
}
