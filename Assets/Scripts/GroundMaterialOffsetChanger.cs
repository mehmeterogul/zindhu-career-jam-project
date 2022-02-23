using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMaterialOffsetChanger : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] float offsetValueChangeSpeed;

    // Update is called once per frame
    void Update()
    {
        mat.mainTextureOffset += new Vector2(0, -offsetValueChangeSpeed * Time.deltaTime);
    }
}
