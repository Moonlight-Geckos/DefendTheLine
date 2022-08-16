using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squashStretch : MonoBehaviour
{
    public float squashAmount;

    void Update()
    {
        transform.localScale = new Vector3(1-squashAmount, 1 + squashAmount, 1- squashAmount);
    }
}
