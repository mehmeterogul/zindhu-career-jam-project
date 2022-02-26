using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    bool isLevelFinished = false;

    // Update is called once per frame
    void Update()
    {
        if (isLevelFinished) return;

        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
    }

    public void FinishLevel()
    {
        isLevelFinished = true;
    }
}
