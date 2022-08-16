using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSoftSphere : MonoBehaviour
{
    public float springStrength, springDamper,radius;

    float squashAmount;
    Rigidbody rb;
    private Vector3 closestPointFromCentre, normalDir;
    Collider[] ContactColliders;
    bool notColliding;

    public squashStretch squashStretchBall;

    void Start()
    {
        notColliding = true;
        squashAmount = 0;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (notColliding)
        {
            squashAmount = Mathf.MoveTowards(squashAmount, 0, 0.005f);
        }
        else
        {
            squashAmount = 1 - (Vector3.Distance(transform.position, closestPointFromCentre) / radius);
        }

        squashStretchBall.squashAmount = -squashAmount;
        squashStretchBall.transform.up = normalDir;
    }


    void FixedUpdate()
    {
        ContactColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider contactCol in ContactColliders)
        {
            AddPointSpring(contactCol.ClosestPoint(transform.position));
        }
    }

    void AddPointSpring(Vector3 vertexPos)
    {
        RaycastHit VetexHit = new RaycastHit();
        Vector3 vertexDir = transform.position - vertexPos;
        float vertexMaxDistance = radius;

        Vector3 VertexWorldVel = rb.GetPointVelocity(transform.position);
        float offset = vertexMaxDistance-0.1f - VetexHit.distance;
        float vel = Vector3.Dot(vertexDir, VertexWorldVel);
        float force = (offset * springStrength) - (vel * springDamper);
        rb.AddForceAtPosition(vertexDir.normalized * force, vertexPos);

    }

    private void OnTriggerStay(Collider other)
    {
        notColliding = false;
        closestPointFromCentre = other.ClosestPoint(transform.position);
        Debug.DrawLine(transform.position, closestPointFromCentre,Color.green);
        normalDir = (transform.position - closestPointFromCentre).normalized;
    }
    private void OnTriggerExit(Collider other)
    {
        squashAmount = -0.1f;
        notColliding = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            foreach (Collider contactCol in ContactColliders)
            {
                Gizmos.DrawSphere(contactCol.ClosestPoint(transform.position), 0.05f);
            }
        }
    }

}
