using UnityEngine;

public class BlobZone : MonoBehaviour
{
    private static Bounds _bounds;

    public static Bounds ZoneBounds
    { get { return _bounds; } }
    private void Awake()
    {
        _bounds = GetComponent<Renderer>().bounds;
        Debug.DrawLine(transform.position + _bounds.size/2, transform.position - _bounds.size/2, Color.magenta, 20);
    }
}