#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Baedrick.ColoredHeaders
{

	public class ColoredHeaderObject : MonoBehaviour
	{
		public ColorSettings colorDesigns = new ColorSettings();

		// If values change when in Edit Mode
		void OnValidate()
		{
			EditorApplication.RepaintHierarchyWindow();
		}
	}

}
#endif