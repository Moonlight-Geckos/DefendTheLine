using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void Awake()
    {
        var rectTrans = GetComponent<RectTransform>();
        var safeArea = Screen.safeArea;
        var minAnchor = safeArea.position;
        var maxAnchor = minAnchor + safeArea.size;
        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;
        rectTrans.anchorMin = minAnchor;
        rectTrans.anchorMax = maxAnchor;
    }
}
