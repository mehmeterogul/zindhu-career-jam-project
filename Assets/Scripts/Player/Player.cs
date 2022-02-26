using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] List<GameObject> pizzaGameObjectsInStack;
    private int pizzaCountInStack;
    private int currentPizzaCount;

    [Header("Crash Obstacle Prefabs")]
    [SerializeField] GameObject emptyPizzaBox;
    [SerializeField] GameObject pizzaSlice;

    [Header("Engine Audio Source")]
    [SerializeField] AudioSource engineAudioSource;
    [SerializeField] AudioClip engineAudio;
    [SerializeField] AudioClip engineCrashAudio;
    [SerializeField] AudioClip engineSlowingAudio;

    [Header("Main Audio Source")]
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioClip crashSound;

    private void Start()
    {
        pizzaCountInStack = pizzaGameObjectsInStack.Count;
        mainAudioSource = GetComponent<AudioSource>();

        StartEngine();
    }

    void StartEngine()
    {
        engineAudioSource.clip = engineAudio;
        engineAudioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pizza"))
        {
            if(currentPizzaCount + 1 < pizzaCountInStack)
            {
                currentPizzaCount++;
                UpdatePizzaStackActiveStatus();
                StartCoroutine(PizzaAnimation());
                Destroy(other.gameObject);
            }
        }

        if(other.gameObject.CompareTag("Obstacle"))
        {
            mainAudioSource.PlayOneShot(crashSound, 1f);

            if (currentPizzaCount - 1 >= 0)
            {
                currentPizzaCount--;
                UpdatePizzaStackActiveStatus();

                StartCoroutine(ThrowPizzaBox(other.gameObject));
            }
        }

        if(other.gameObject.CompareTag("Area"))
        {
            Area area = other.GetComponent<Area>();
            OPERATION OPR;
            OPR = area.GetOperation();
            int value = area.GetOperationValue();

            DoOperation(OPR, value);
        }
    }

    IEnumerator ThrowPizzaBox(GameObject other)
    {
        GameObject temp = Instantiate(emptyPizzaBox, pizzaGameObjectsInStack[currentPizzaCount - 1].transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(ForceVector(), ForceMode.Impulse);
        temp.GetComponent<Rigidbody>().AddTorque(ForceVector() * 5, ForceMode.Impulse);
        Destroy(temp, 2f);

        temp = Instantiate(pizzaSlice, pizzaGameObjectsInStack[currentPizzaCount - 1].transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(ForceVector(), ForceMode.Impulse);
        temp.GetComponent<Rigidbody>().AddTorque(ForceVector() * 5, ForceMode.Impulse);
        Destroy(temp, 2f);

        temp = Instantiate(pizzaSlice, pizzaGameObjectsInStack[currentPizzaCount - 1].transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(ForceVector(), ForceMode.Impulse);
        temp.GetComponent<Rigidbody>().AddTorque(ForceVector() * 5, ForceMode.Impulse);
        Destroy(temp, 2f);

        yield return new WaitForSeconds(0.12f);

        animator.SetTrigger("hit");
        FindObjectOfType<PlayerStackPosition>().Hit();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            Invoke("Finish", 0.25f);
        }
    }

    void Finish()
    {
        FindObjectOfType<GameManager>().FinishLevel();
        FindObjectOfType<PlayerStackPosition>().FinishLevel();
        animator.SetTrigger("levelFinished");
        engineAudioSource.Stop();
    }

    void DoOperation(OPERATION opr, int value)
    {
        if (opr == OPERATION.ADDITION) currentPizzaCount += value;
        else if (opr == OPERATION.SUBTRACTION) currentPizzaCount -= value;
        else if (opr == OPERATION.MULTIPLICATION) currentPizzaCount *= value;
        else
        {
            if (value == 0) return; Debug.Log(value);
            float temp = (float)currentPizzaCount / value; Debug.Log(temp);
            currentPizzaCount = Mathf.RoundToInt(temp); Debug.Log(currentPizzaCount);
        }

        if (currentPizzaCount < 0) currentPizzaCount = 0;

        UpdatePizzaStackActiveStatus();
        StartCoroutine(PizzaAnimation());
    }

    void UpdatePizzaStackActiveStatus()
    {
        for(int i = 0; i < pizzaCountInStack; i++)
        {
            if(i < currentPizzaCount)
                pizzaGameObjectsInStack[i].SetActive(true);
            else
                pizzaGameObjectsInStack[i].SetActive(false);
        }
    }

    IEnumerator PizzaAnimation()
    {
        int temp = currentPizzaCount;
        if (temp > pizzaCountInStack) temp = pizzaCountInStack;

        for (int i = temp - 1; i >= 0; i--)
        {
            pizzaGameObjectsInStack[i].transform.DORewind();
            pizzaGameObjectsInStack[i].transform.DOPunchScale(new Vector3(0.3f, 0.5f, 0.3f), .25f);
            yield return new WaitForSeconds(0.05f);
        }
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
