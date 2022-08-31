using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private void Awake()
    {
        var camera = GetComponent<Camera>();
        var camera2 = transform.GetChild(0).GetComponent<Camera>();

        float height = camera.transform.position.y;
        var castle = GameObject.FindGameObjectWithTag("Destination");
        float frustumWidth = (Mathf.Abs(castle.transform.position.x)  + castle.GetComponent<BoxCollider>().size.x * castle.transform.localScale.x) * 2;

        var frustumHeight = frustumWidth / camera.aspect;
        var fov = 2.0f * Mathf.Atan(frustumHeight * 0.5f / height) * Mathf.Rad2Deg;
        camera.fieldOfView = fov;
        camera2.fieldOfView = fov;
    }
}