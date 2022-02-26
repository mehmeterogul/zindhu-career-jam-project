using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.AddForce(ForceVector(), ForceMode.Impulse);
            rb.AddTorque(ForceVector(), ForceMode.Impulse);
            Destroy(gameObject.GetComponent<BoxCollider>());
            Invoke("DestroyObject", 2f);
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    Vector3 ForceVector()
    {
        return new Vector3(RandomNumber() * MinusCounter(), RandomNumber(), -RandomNumber());
    }

    float RandomNumber()
    {
        return Random.Range(5, 15);
    }

    float MinusCounter()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
            return 1;
        else
            return -1;
    }
}
