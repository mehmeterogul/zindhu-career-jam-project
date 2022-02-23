using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToScreen : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
