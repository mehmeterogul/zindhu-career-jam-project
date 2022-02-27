using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    bool isLevelFinished = true;

    // Update is called once per frame
    void Update()
    {
        if (isLevelFinished) return;

        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
    }

    public void StartLevel()
    {
        isLevelFinished = false;
    }

    public void FinishLevel()
    {
        isLevelFinished = true;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
