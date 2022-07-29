using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private void Start()
    {
        var camera = GetComponent<Camera>();

        float height = camera.transform.position.y;
        float frustumWidth = GameObject.FindGameObjectWithTag("Grid").transform.localScale.x * 12;

        var frustumHeight = frustumWidth / camera.aspect;
        camera.fieldOfView = 2.0f * Mathf.Atan(frustumHeight * 0.5f / height) * Mathf.Rad2Deg;

        var LeftBottomCorner = MathHelper.GetPointAtHeight(camera.ViewportPointToRay(new Vector3(0, 0, 0)), 0);
        var direction = LeftBottomCorner - new Vector3(5, 0, -4.5f);
        direction.x = 0;
        direction.y = 0;
        camera.transform.position -= direction;
    }
}