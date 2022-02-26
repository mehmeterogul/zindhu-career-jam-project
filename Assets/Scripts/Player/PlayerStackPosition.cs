using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStackPosition : MonoBehaviour
{
    [SerializeField] Transform stackPosition;
    [SerializeField] Transform playerModel;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = stackPosition.position;
    }

    public void FinishLevel()
    {
        Destroy(GetComponent<Animator>());
        transform.SetParent(playerModel);
        Destroy(GetComponent<PlayerStackPosition>());
    }
}
