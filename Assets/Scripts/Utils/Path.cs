using UnityEngine;

public class Path : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position + (transform.forward * 2f),
            transform.position,
            Color.magenta);

        Debug.DrawLine(transform.position + (transform.forward * 2f),
            transform.position - (transform.right * 2f),
            Color.magenta);

        Debug.DrawLine(transform.position + (transform.forward * 2f),
            transform.position + (transform.right * 2f),
            Color.magenta);
    }
}
