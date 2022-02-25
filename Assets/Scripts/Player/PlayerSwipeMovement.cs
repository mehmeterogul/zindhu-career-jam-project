using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwipeMovement : MonoBehaviour
{
    Touch touch;
    [SerializeField] float speedModifier = 0.01f;
    [SerializeField] float horizontalBoundry = 2f;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * speedModifier, transform.position.y, transform.position.z);
            }
        }

        if (transform.position.x > horizontalBoundry)
        {
            transform.position = new Vector3(horizontalBoundry, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -horizontalBoundry)
        {
            transform.position = new Vector3(-horizontalBoundry, transform.position.y, transform.position.z);
        }
    }
}
