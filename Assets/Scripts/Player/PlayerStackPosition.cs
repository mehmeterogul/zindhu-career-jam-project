using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStackPosition : MonoBehaviour
{
    [SerializeField] Transform stackPosition;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = stackPosition.position;
    }
}
