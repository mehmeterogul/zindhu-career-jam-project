using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePizzaBox : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 45;
    [SerializeField] float amplitude = 0.1f;
    [SerializeField] float frequency = 3;

    // Start is called before the first frame update
    void Start()
    {
        // frequency = Random.Range(2,3);
        SetRandomRotation();
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float y = Mathf.Sin(Time.time * frequency) * amplitude + 0.5f;
        float z = transform.position.z;

        // Move up and down the object
        transform.position = new Vector3(x, y, z);

        // Rotate the object
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    void SetRandomRotation()
    {
        float yRotation = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
