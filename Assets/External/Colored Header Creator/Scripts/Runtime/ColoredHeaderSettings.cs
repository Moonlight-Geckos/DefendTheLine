using UnityEngine;

namespace Baedrick.ColoredHeaders
{
	#region Header Settings

	/// <summary>
	/// 
	/// Stores settings for Headers
	/// 
	/// </summary>
	[System.Serializable]
    public class ColorSettings
    {
        [Tooltip("Display text for the Header.")]
        public string headerText;
        [Tooltip("Header background color.")]
        public Color headerColor;

        [Space(15)]

        [Tooltip("Header text alignment.")]
        public TextAlignmentOptions textAlignmentOptions;
        [Tooltip("Header text style.")]
        public FontStyleOptions fontStyleOptions;
        [Tooltip("Header text size.")]
        public float fontSize;
        [Tooltip("Header text color.")]
        public Color fontColor;
        [Tooltip("Header text drop shadow. Warning it is slow.")]
        public bool dropShadow;
    }

    public enum FontStyleOptions
    {
        Bold = 0,
        Normal = 1,
        Italic = 2,
        BoldAndItalic = 3
    }

    public enum TextAlignmentOptions
    {
        Center = 0,
        Left = 1,
        Right = 2
    }


    #endregion

    #region Scriptable Object

    /// <summary>
    /// 
    /// Colored Header Editor Window Settings
    /// 
    /// </summary>
    public class ColoredHeaderSettings : ScriptableObject
	{
        // Header Settings
        public string                       headerText                  = "New Header";
        public Color                        headerColor                 = Color.gray;

        // Text Settings
        public TextAlignmentOptions         textAlignmentOptions        = TextAlignmentOptions.Center;
        public FontStyleOptions             fontStyleOptions            = FontStyleOptions.Bold;
        public float                        fontSize                    = 12f;
        public Color                        fontColor                   = Color.white;
        public bool                         dropShadow                  = false;
    }


	#endregion

}