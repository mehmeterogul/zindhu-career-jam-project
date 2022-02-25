using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStackPosition : MonoBehaviour
{
    [SerializeField] Transform stackPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = stackPosition.position;
    }
}
