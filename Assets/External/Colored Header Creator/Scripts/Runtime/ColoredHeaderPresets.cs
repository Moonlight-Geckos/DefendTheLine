using System.Collections.Generic;
using UnityEngine;

namespace Baedrick.ColoredHeaders
{

	/// <summary>
	/// 
	/// Colored Header Preset Template
	/// 
	/// </summary>
	[System.Serializable]
	[CreateAssetMenu(fileName = "New Header Preset", menuName = "Colored Header Creator/Header Preset")]
	public class ColoredHeaderPresets : ScriptableObject
	{
		public List<ColorSettings> coloredHeaderPresets = new List<ColorSettings>();
	}

}