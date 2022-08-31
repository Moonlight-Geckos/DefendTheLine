using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerIndicator : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
